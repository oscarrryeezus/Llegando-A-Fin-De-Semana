# LLEGANDO A FIN DE MES - 2D Action Game

<div align="center">

![Aspectos legales](https://drive.google.com/file/d/17o8XVLbZNXnncSWzl0LELnSwp8Iiu27y/view?usp=sharing)
![Juego WEB GL](https://oscarponchallantas.itch.io/llegando-a-fin-mes)
![Juego Desktop](https://drive.google.com/drive/folders/1hJxK0ryRP_CCewcgC8ahMA0Ckhe-kp5K?usp=sharing)


**Un intenso juego de acción 2D top-down con combate contra enemigos y jefes épicos**

[🎮 Descargar](#) • [📖 Documentación](#características) • [🐛 Reportar Bug](../../issues)

</div>

---

## 🎯 Descripción

**DAWG** es un juego de acción 2D desarrollado en Unity que combina mecánicas de combate fluidas con batallas contra jefes desafiantes. El jugador debe enfrentarse a hordas de enemigos y derrotar a dos jefes únicos, cada uno con sus propios patrones de ataque y fases de combate.

## ✨ Características

### 🎮 Mecánicas de Juego
- **Sistema de Combate Dinámico**: Ataque cuerpo a cuerpo con martillo y animaciones fluidas
- **Movimiento Top-Down**: Control de 8 direcciones con sprint
- **Sistema de Salud**: Barra de vida con feedback visual
- **Knockback Inteligente**: Retroceso direccional al recibir daño

### 👾 Enemigos y Jefes

#### Enemigos Regulares
- **3 Tipos de Enemigos**: Ligero, Pesado y Ágil
- **IA Adaptativa**: Comportamiento de persecución y ataque basado en rangos
- **Movimiento Zigzag**: Los enemigos ágiles esquivan de forma impredecible

#### Boss Nivel 1 - "El Lanzapiedras"
- **Ataque Cuerpo a Cuerpo**: Golpes devastadores en rango corto
- **Proyectiles de Roca**: Lanza piedras al jugador desde distancia
- **Sistema de Hitbox Dinámico**: Colisión precisa por frame de animación

#### Boss Nivel 2 - "El Transformado"
- **Sistema de Dos Fases**: Transformación al 50% de vida
- **Fase Normal**: Ataques cuerpo a cuerpo estándar
- **Fase Potenciada**: 
  - Velocidad aumentada
  - Daño incrementado
  - **Ataque Láser**: Rayo láser con LineRenderer y raycast 2D
  - Efecto de humo en transformación

### 🎨 Sistema Visual
- **Animaciones Completas**: Idle, caminar, atacar, recibir daño, muerte
- **UI Inmersiva**: 
  - Barras de salud para bosses con portraits
  - Sistema de flechas direccionales
  - Pantallas de Game Over y Victoria
- **Ordenamiento por Profundidad**: Sprites ordenados por posición Y

### 🎵 Audio
- **Sonidos de Pasos**: Loop que inicia/detiene con el movimiento
- **Efectos de Ataque**: Sonido one-shot al atacar
- **Música por Nivel**: Tracks diferentes para Level 1 y Level 2
- **Gestor Persistente**: LevelMusicManager con DontDestroyOnLoad

### 📱 Soporte Móvil
- **Controles Táctiles**: Integración con Joystick Pack de Fenerax Studios
- **Botones de Acción**: Interfaz táctil para ataque y salto
- **Optimizado para Android**: Build settings configurados para ARM64

## 🛠️ Tecnologías

- **Motor**: Unity 2021.3.45f1
- **Lenguaje**: C#
- **Física**: Unity 2D Physics con `Rigidbody2D`
- **Animación**: Animator con State Machine
- **UI**: Unity UI (uGUI) y TextMesh Pro
- **Input**: Legacy Input System + Joystick virtual para móvil

## 📦 Arquitectura del Código

### Patrones de Diseño
- **Singleton**: GameManager, LevelMusicManager, UI_BossHealth, UI_Boss2Health, GameOverController
- **Component-Based**: Separación entre lógica de movimiento y salud
- **Interface-Driven**: `IDamageable` para sistema de daño unificado
- **Event-Driven**: Eventos de muerte de boss usando `System.Action`

### Estructura de Scripts

```
Assets/Scripts/
├── Player/
│   ├── PlayerScript.cs          # Movimiento, ataque, audio
│   ├── PlayerHealth.cs          # Sistema de vida y muerte
│   └── HitboxMartillo.cs        # Detección de colisión de ataque
│
├── Enemies/
│   ├── EnemyScript.cs           # IA y comportamiento
│   ├── EnemyHealth.cs           # Sistema de salud
│   ├── EnemySpawner.cs          # Generación de enemigos
│   └── EnemyZone.cs             # Zonas de encuentro
│
├── boss_lvl_1/
│   ├── BossController.cs        # IA y ataques del Boss 1
│   ├── BossHealth.cs            # Salud del Boss 1
│   ├── BossZoneController.cs    # Control de zona de boss
│   ├── BossAttackHitbox.cs      # Hitbox de ataques
│   ├── RockProjectile.cs        # Proyectil de roca
│   └── IDamageable.cs           # Interface de daño
│
├── boss_lvl_2/
│   ├── Boss2Controller.cs       # IA de dos fases
│   ├── LaserController.cs       # Sistema de láser modular
│   ├── Boss2ZoneController.cs   # Control de zona Boss 2
│   ├── Boss2AttackHitbox.cs     # Hitbox de Boss 2
│   └── UI_Boss2Health.cs        # UI de salud Boss 2
│
├── Managers/
│   ├── GameManager.cs           # Estado persistente del juego
│   ├── LevelMusicManager.cs     # Gestor de música por nivel
│   ├── GameOverController.cs    # Pantalla de Game Over
│   ├── VictoryController.cs     # Pantalla de Victoria
│   └── LevelInitializer.cs      # Inicialización de niveles
│
├── UI/
│   ├── UI_BossHealth.cs         # Barra de salud Boss 1
│   ├── HealthBar.cs             # Barra de salud genérica
│   ├── ArrowUIManager.cs        # Flechas direccionales
│   └── MainMenuController.cs    # Menú principal
│
└── Mobile/
    └── MobileInputManager.cs    # Detección de plataforma móvil
```

## 🎮 Controles

### PC (Teclado)
- **WASD / Flechas**: Movimiento
- **Shift**: Sprint
- **Ctrl Izq / Click Izq**: Atacar
- **Espacio**: Saltar (visual)

### Android (Táctil)
- **Joystick Virtual**: Movimiento
- **Botón de Ataque**: Atacar
- **Botón de Salto**: Saltar

## 🚀 Instalación y Compilación

### Requisitos
- Unity 2021.3.45f1 o superior
- Android Build Support (para móvil)
- Git

### Clonar el Repositorio
```bash
git clone https://github.com/TuUsuario/dawg.git
cd dawg
```

### Abrir en Unity
1. Abre Unity Hub
2. Click en "Add" → Selecciona la carpeta `dawg`
3. Asegúrate de usar Unity 2021.3.45f1
4. Abre el proyecto

### Build para Windows
1. File → Build Settings
2. Selecciona "PC, Mac & Linux Standalone"
3. Platform: Windows
4. Click "Build"

### Build para Android
1. File → Build Settings
2. Selecciona "Android"
3. Click "Switch Platform"
4. Player Settings:
   - Company Name: Tu nombre
   - Package Name: `com.tunombre.dawg`
   - Minimum API Level: 22 (Android 5.1)
   - Target Architectures: ARM64 ✅
5. Click "Build" o "Build And Run"

## 📂 Escenas

El juego tiene 6 escenas principales:

1. **MainMenu**: Menú principal del juego
2. **IntroCinematic**: Cinemática de introducción con video
3. **Level1**: Primer nivel con Boss 1
4. **Level2**: Segundo nivel con Boss 2
5. **GameOver**: Pantalla de derrota
6. **Victory**: Pantalla de victoria

### Flujo de Escenas
```
MainMenu → IntroCinematic → Level1 → Victory → Level2 → Victory → MainMenu
                                ↓                    ↓
                            GameOver             GameOver
```

## 🎨 Assets Utilizados

- **Joystick Pack**: Fenerax Studios (Controles móviles)
- **Sprites**: Arte custom para personajes y enemigos
- **Audio**: Efectos de sonido y música personalizados
- **TextMesh Pro**: Sistema de texto avanzado de Unity

## 🐛 Debugging

### Console Logs Útiles
- `Debug.Log("Vida del player -> " + vidaActual)` en PlayerHealth
- `Debug.Log("Boss 1 derrotado")` en BossZoneController
- `Debug.Log("Raycast hit: " + hit.collider.name)` en LaserController
- `Debug.Log($"Joystick: X={joystick.Horizontal}")` en PlayerScript

### Problemas Comunes

**El player no se mueve con joystick**
- Verifica que `VariableJoystick` esté asignado en el Inspector
- Marca ✅ "Usar Joystick" en PlayerScript

**Los bosses no reciben daño**
- Verifica que implementen `IDamageable`
- Chequea que el hitbox del martillo tenga trigger collider activo

**Audio no se escucha**
- Asegura que AudioClips estén asignados en Inspector
- Verifica que LevelInitializer esté en la escena

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 👨‍💻 Autor

**Oscar Daniel Morales Navarro**
- GitHub: [@0scar](https://github.com/oscarrryeezus)

## 🙏 Agradecimientos

- Unity Technologies por el motor de juego
- Fenerax Studios por el Joystick Pack
- La comunidad de desarrollo de Unity

---

<div align="center">

**⭐ Si te gusta el proyecto, dale una estrella! ⭐**

Hecho con ❤️ y ☕

</div>
