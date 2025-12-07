# AI Coding Agent Instructions - "dawg" 2D Action Game

## Project Overview
Unity 2021.3.45f1 top-down 2D action game with player combat, enemy AI, boss battles, and scene management. Uses legacy Unity Input System and 2D physics.

## Architecture Patterns

### Component Organization
- **Player**: `PlayerScript` (movement/input) + `PlayerHealth` (damage/death) on single GameObject with `Rigidbody2D`, `Animator`, `SpriteRenderer`
- **Enemies**: `EnemyScript` (AI/behavior) + `EnemyHealth` (damage/death) - health component may be separate or integrated
- **Boss**: Unified `BossController` handles AI, health, and attacks in one script (pattern differs from regular enemies)
- **Hitboxes**: Separate child GameObjects with trigger colliders (`HitboxMartillo`, `BossAttackHitbox`) - activated/deactivated via parent scripts

### Damage System
- **IDamageable Interface**: `TomarDaño(float dmg)` - used by Boss and projectiles
- **Direct Component Access**: Player/Enemy health use direct component methods (`RecibirDano`, `RecibirDaño`, `RecibirDanio` - note spelling inconsistencies exist in codebase)
- **Knockback Pattern**: Applied via `Rigidbody2D.AddForce()` with `ForceMode2D.Impulse`, direction calculated from attacker position
- Boss uses `IDamageable` interface while player uses direct `PlayerHealth.RecibirDano()`

### Animation State Management
- Animator parameters: `isWalking` (bool), `isAttacking` (bool), `isHitted` (bool), `jump` (trigger), `hit` (trigger), `death` (trigger), `die` (trigger)
- Animation-driven hitboxes: Activate/deactivate colliders during attack coroutines with timing (e.g., 0.25s delay, 0.3s active)
- State locks: Use `atacando`/`saltando` flags to prevent input during animations

### Enemy AI Architecture
- **TipoEnemigo Enum**: `Ligero`, `Pesado`, `Agil` - configure behavior via `ConfigurarTipo()` in `Start()`
- **Range-based Behavior**: `rangoDeteccion` → chase, `rangoAtaque` → attack, else idle
- **Attack Pattern**: Coroutine with velocity stop, timer cooldown (`tiempoEntreAtaques`), and range check before damage application
- **Agil Type Specific**: Adds zigzag movement: `dir.y += Mathf.Sin(Time.time * 8f) * 0.5f`

### Boss Fight System (`boss_lvl_1/`)
- **Dual Attack System**: Melee (close range, direct damage) + Ranged (instantiate rock prefab from `puntoDisparo`)
- **Separate Cooldowns**: `cdMelee` and `cdRoca` tracked independently with timers
- **UI Integration**: Singleton `UI_BossHealth.Instance.Inicializar(this)` called in `Start()`, uses `Image.fillAmount` for health bar
- **Event-Driven Death**: `public event System.Action OnDeath` - subscribe for level progression logic
- `IDamageable` interface for damage handling

## Critical Conventions

### Movement & Physics
- **Top-Down 2D**: Set `Rigidbody2D.gravityScale = 0` and `freezeRotation = true`
- **Movement**: Use `Rigidbody2D.MovePosition()` in `FixedUpdate()` with normalized direction vectors
- **Sprint System**: Toggle speed between `velocidadBase` and `velocidadSprint` via Input.GetButton("Sprint")
- **Visual Jump**: Coroutine modifying `transform.position.y` temporarily (not physics-based), locks movement during execution

### Camera System (`CameraFollow.cs`)
- **Horizontal-Only Tracking**: Clamps X position between calculated map bounds, Y and Z fixed
- **Bounds Calculation**: Requires `SpriteRenderer mapa` reference, calculates limits using `mapa.bounds` and camera orthographic size
- **Smooth Following**: `Vector3.Lerp()` with `suavizado` multiplier in `LateUpdate()`

### Scene Flow
- `MainMenu` → `IntroCinematic` (video playback with `VideoPlayer.loopPointReached`) → `Level1`
- Use `SceneManager.LoadScene(string)` for transitions
- Scene names: "MainMenu", "IntroCinematic", "Level1"

### Prefab Structure
- **Prefabs Location**: `Assets/Prefabs/` - `player.prefab`, `Boss.prefab`, `enemigo_ligero.prefab`, `piedra.prefab`, `projectil.prefab`
- **Spawning**: Use `Instantiate(prefab, position, Quaternion.identity)` (see `EnemySpawner.cs` pattern)
- **Boss/Enemy Prefabs**: Must have Animator, Rigidbody2D, SpriteRenderer, and appropriate health/controller scripts

### Input Configuration
- **Input Manager**: ProjectSettings/InputManager.asset defines axes
- **Fire1**: Left Ctrl or Mouse 0 (attack)
- **Jump**: Space (visual effect, not gameplay mechanic)
- **Sprint**: Shift (toggle speed)
- **Horizontal/Vertical**: WASD or Arrow Keys

## Development Workflows

### Adding New Enemy Types
1. Create prefab with Animator, Rigidbody2D (gravityScale=0, freezeRotation=true), Collider2D, SpriteRenderer
2. Add `EnemyScript` and set `TipoEnemigo` enum value
3. Configure stats: `velocidad`, `rangoDeteccion`, `rangoAtaque`, `daño`, `tiempoEntreAtaques`
4. Add `EnemyHealth` with `vidaMaxima` or integrate health into controller
5. Setup animator with triggers: `isWalking`, `isAttacking`, `hit`, `death`
6. Tag as "Enemy" if player attacks need to detect it

### Adding New Boss Phase/Attack
1. Add cooldown timer float field (e.g., `cdNuevoAtaque`) and public cooldown config
2. Increment timer in `Update()`: `timerNuevoAtaque += Time.deltaTime`
3. Add range check condition in `Update()` branching logic
4. Create attack method checking `if (timerNuevoAtaque < cdNuevoAtaque) return`
5. Implement attack coroutine pattern: set flags, stop velocity, animate, execute, cleanup
6. For projectiles: `Instantiate(prefab, puntoDisparo.position, Quaternion.identity)` with directional Rigidbody2D velocity

### Debugging Combat
- Check collider trigger settings: hitboxes use `OnTriggerEnter2D`, projectiles may use `OnCollisionEnter2D`
- Verify layer collision matrix in Physics2D settings
- Component spelling variations: `RecibirDano` vs `RecibirDaño` vs `RecibirDanio` - search codebase before adding new methods
- Use `Debug.Log()` statements - existing pattern in health scripts (e.g., `Debug.Log("Vida del player -> " + vidaActual)`)

## Key Files Reference
- **Player Core**: `Assets/Scripts/PlayerScript.cs`, `PlayerHealth.cs`
- **Enemy System**: `Assets/Scripts/EnemyScript.cs`, `EnemyHealth.cs`, `EnemySpawner.cs`
- **Boss System**: `Assets/Scripts/boss_lvl_1/BossController.cs`, `BossHealth.cs`, `IDamageable.cs`, `RockProjectile.cs`
- **UI**: `Assets/Scripts/UI_BossHealth.cs`, `HealthBar.cs`
- **Scene Management**: `Assets/Scripts/MainMenuController.cs`, `CinematicController.cs`
- **Camera**: `Assets/Scripts/CameraFollow.cs`

## Game Systems

### Difficulty System (`GameManager.cs`)
- **Singleton pattern with DontDestroyOnLoad**
- Two difficulty modes: `Medio` (restart level) and `Dificil` (restart from menu)
- `OnPlayerDeath()` handles respawn logic based on difficulty
- Call `GameManager.Instance.GuardarEscenaActual(sceneName)` at level start

### Audio System
- **Player Audio**: `AudioSource` on player with `sonidoCaminar` (looped) and `sonidoAtaque` (one-shot)
  - Walking sound starts/stops based on movement in `FixedUpdate()`
  - Attack sound plays in `Atacar()` coroutine
- **Level Music**: `LevelMusicManager.cs` singleton with `DontDestroyOnLoad`
  - Call `LevelMusicManager.Instance.PlayLevelMusic(levelNumber)` at level start
  - Different AudioClip for each level

### Level Management
- **LevelInitializer.cs**: Add to each level scene
  - Sets `numeroNivel` (1 or 2)
  - Registers scene name with GameManager
  - Starts level music automatically
- **Text Cinematics**: `TextCinematicController.cs` for story text between levels
  - Typewriter effect with configurable speed
  - Skip with any key press
  - Auto-advance to next scene

### Difficulty Selection Flow
- MainMenu → `DifficultySelector.cs` shows difficulty panel → IntroCinematic → Level1
- `DifficultySelector` creates GameManager instance if missing
- Buttons call `SeleccionarDificultad()` then load IntroCinematic

## Notes
- No namespaces used - all classes in global namespace
- Spanish naming in comments/variables is common (e.g., `vidaMaxima`, `daño`, `RecibirDaño`)
- Inconsistent damage method naming exists across codebase - check existing patterns before extending
- Boss uses singleton pattern for UI (`UI_BossHealth.Instance`), enemies use direct reference passing
- Multiple singleton managers use DontDestroyOnLoad (GameManager, LevelMusicManager, UI_BossHealth, UI_Boss2Health)
