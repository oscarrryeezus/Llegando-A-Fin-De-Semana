# Guía Completa de Implementación - Game Over, Audio y Música

## 📋 Tabla de Contenidos
1. [Pantalla de Game Over](#1-pantalla-de-game-over)
2. [Sistema de Audio del Jugador](#2-sistema-de-audio-del-jugador)
3. [Sistema de Música por Nivel](#3-sistema-de-música-por-nivel)
4. [Integración Final](#4-integración-final)

---

## 1. Pantalla de Game Over

### Paso 1.1: Crear UI de Game Over en Level 1
1. Abre la escena **Level1**
2. Ve al Canvas existente (o crea uno si no existe)
3. Crea la siguiente estructura:

```
Canvas
├── (Otros elementos UI existentes...)
│
└── PanelGameOver (Panel NUEVO - desactivado por defecto)
    ├── PanelFondo (Image - color negro semi-transparente)
    ├── TextoGameOver (Text)
    │   └── Text: "GAME OVER"
    ├── BotónReintentar (Button)
    │   └── Text: "Reintentar"
    └── BotónMenuPrincipal (Button)
        └── Text: "Menú Principal"
```

### Paso 1.2: Configurar Panel de Game Over
1. Selecciona **PanelGameOver**:
   - Rect Transform: Estíralo a pantalla completa (Anchor Presets: Stretch-Stretch)
   - **Desactívalo** (checkbox al lado del nombre)

2. Configura **PanelFondo**:
   - Rect Transform: Pantalla completa
   - Image → Color: Negro (0, 0, 0, 200) - semi-transparente

3. Configura **TextoGameOver**:
   - Font Size: 80
   - Alignment: Center
   - Color: Rojo
   - Position: Centro-superior

4. Configura los **Botones**:
   - Width: 300, Height: 60
   - Centrados horizontalmente
   - BotónReintentar más arriba que BotónMenuPrincipal

### Paso 1.3: Agregar GameOverController
1. Crea un GameObject vacío en Level1
2. Nómbralo `GameOverController`
3. Agrégale el componente `GameOverController.cs`
4. En el Inspector, arrastra:
   - **Panel Game Over**: El panel que creaste
   - **Texto Game Over**: El texto "GAME OVER"
   - **Boton Reintentar**: El botón de reintentar
   - **Boton Menu Principal**: El botón de menú
   - **Tiempo Antes De Activar**: 2 segundos

### Paso 1.4: Repetir para Level 2
1. Copia el **PanelGameOver** completo de Level1
2. Pégalo en el Canvas de **Level2**
3. Crea otro `GameOverController` en Level2
4. Configura las mismas referencias

---

## 2. Sistema de Audio del Jugador

### Paso 2.1: Preparar AudioClips
1. Importa tus archivos de audio a Unity:
   - **Sonido de pasos** (caminar/correr)
   - **Sonido de martillo** (ataque)
2. Colócalos en `Assets/Audio/` (crea la carpeta si no existe)

### Paso 2.2: Configurar Audio en el Prefab del Player
1. Abre el prefab `player.prefab` en `Assets/Prefabs/`
2. Selecciona el GameObject raíz del player
3. En el componente `PlayerScript` verás nuevos campos:
   - **Sonido Caminar**: Arrastra el AudioClip de pasos
   - **Sonido Ataque**: Arrastra el AudioClip del martillo
4. **NO necesitas agregar AudioSource manualmente** - el script lo crea automáticamente

### Paso 2.3: Ajustar Configuración de Audio (Opcional)
Si quieres ajustar el volumen:
1. Inicia el juego en Play Mode
2. Selecciona el player en la jerarquía
3. Verás el `AudioSource` creado automáticamente
4. Ajusta el `Volume` al nivel deseado (ej: 0.7)
5. Para guardar estos cambios:
   - Sal de Play Mode
   - En el prefab, agrega manualmente un `AudioSource`
   - Configura el volumen deseado
   - El script usará este AudioSource en lugar de crear uno nuevo

---

## 3. Sistema de Música por Nivel

### Paso 3.1: Preparar Canciones
1. Importa dos canciones diferentes:
   - **Música Level 1** (ej: tema épico, acción)
   - **Música Level 2** (ej: tema más intenso)
2. Colócalas en `Assets/Audio/Music/`

### Paso 3.2: Crear LevelMusicManager
1. En la escena **MainMenu** (o cualquier escena inicial):
2. Crea un GameObject vacío
3. Nómbralo `LevelMusicManager`
4. Agrégale el componente `LevelMusicManager.cs`
5. En el Inspector:
   - **Level 1 Music**: Arrastra el AudioClip del nivel 1
   - **Level 2 Music**: Arrastra el AudioClip del nivel 2
6. Este GameObject persiste entre escenas automáticamente

### Paso 3.3: Configurar Level 1
1. Abre la escena **Level1**
2. Crea un GameObject vacío en la jerarquía
3. Nómbralo `LevelInitializer`
4. Agrégale el componente `LevelInitializer.cs`
5. En el Inspector:
   - **Numero Nivel**: `1`

### Paso 3.4: Configurar Level 2
1. Abre la escena **Level2** (o créala si no existe)
2. Crea un GameObject vacío
3. Nómbralo `LevelInitializer`
4. Agrégale el componente `LevelInitializer.cs`
5. En el Inspector:
   - **Numero Nivel**: `2`

### Paso 3.5: Verificar Build Settings
1. Ve a **File → Build Settings**
2. Asegúrate de que todas las escenas estén en el build:
   - MainMenu (índice 0)
   - IntroCinematic (índice 1)
   - Level1 (índice 2)
   - Level2 (índice 3 o más)
3. Arrastra las escenas al panel si faltan

---

## 4. Integración Final

### Paso 4.1: Configurar Flujo de Escenas
El flujo es ahora mucho más simple:
```
MainMenu → IntroCinematic → Level1 → Level2 → MainMenu
```

### Paso 4.2: Transición Level 1 → Level 2
Ya está configurado automáticamente:
- Al derrotar al Boss 1, `BossZoneController` carga Level2 después de 2 segundos
- No hay cinemática intermedia

### Paso 4.3: Transición Level 2 → MainMenu
Ya está configurado:
- Al derrotar al Boss 2, `Boss2ZoneController` vuelve al menú después de 3 segundos

### Paso 4.4: Verificar PlayerHealth
1. Abre `PlayerHealth.cs`
2. Confirma que el método `Morir()` llama a `GameOverController.Instance.MostrarGameOver()`
3. Ya debe estar implementado en el código modificado

### Paso 4.5: Probar el Sistema Completo

**Test 1: Game Over**
1. Inicia Level1
2. Muere intencionalmente
3. Espera 2 segundos → Aparece pantalla de Game Over ✓
4. Click "Reintentar" → Reinicia Level1 ✓
5. Muere de nuevo
6. Click "Menú Principal" → Vuelve al MainMenu ✓

**Test 2: Audio del Player**
1. Inicia juego
2. Mueve al personaje → Debe sonar pasos ✓
3. Detente → Pasos se detienen ✓
4. Ataca → Sonido de martillo ✓

**Test 4: Música de Niveles**
1. Inicia juego
2. En Level1 → Suena música 1 ✓
3. Pasa a Level2 → Cambia a música 2 ✓

**Test 5: Flujo Completo**
1. MainMenu → IntroCinematic → Level1 ✓
2. Derrota Boss 1 → Carga Level2 después de 2s ✓
3. Derrota Boss 2 → Vuelve a MainMenu después de 3s ✓

---

## 🐛 Solución de Problemas Comunes

### La pantalla de Game Over no aparece:
- Verifica que `GameOverController` esté en cada nivel (Level1 y Level2)
- Confirma que `PanelGameOver` esté desactivado al inicio
- Revisa que todas las referencias estén asignadas en el Inspector
- Verifica que `PlayerHealth` tenga el código actualizado

### El juego se congela en Game Over:
- El juego usa `Time.timeScale = 0` para pausar
- Los botones deben usar `Time.unscaledDeltaTime` o `Time.timeScale = 1` antes de cargar escenas
- Ya está implementado correctamente en `GameOverController`

### La música no suena:
- Verifica que `LevelMusicManager` esté en la escena inicial
- Confirma que los AudioClips estén asignados
- Revisa que `LevelInitializer` esté en cada nivel

### Los sonidos del player no funcionan:
- Verifica que los AudioClips estén asignados en `PlayerScript`
- Asegura que los archivos de audio sean formato WAV o MP3
- Revisa la configuración de Import Settings de los AudioClips (3D/2D Sound)

### El nivel no cambia al derrotar al boss:
- Verifica que las escenas estén en Build Settings
- Confirma que `BossZoneController` esté suscrito al evento `OnDeath` del boss
- Revisa la consola por errores

### El player no muere correctamente:
- Verifica que `PlayerHealth.cs` tenga el código actualizado
- Asegura que `GameOverController.Instance` exista en la escena
- Revisa la consola por errores de NullReference

---

## ✅ Checklist Final

**Escenas:**
- [ ] MainMenu tiene LevelMusicManager
- [ ] Level1 tiene LevelInitializer (nivel = 1)
- [ ] Level1 tiene GameOverController configurado
- [ ] Level2 tiene LevelInitializer (nivel = 2)
- [ ] Level2 tiene GameOverController configurado
- [ ] Todas las escenas están en Build Settings: MainMenu, IntroCinematic, Level1, Level2

**Audio:**
- [ ] Player tiene AudioClips asignados (pasos + ataque)
- [ ] LevelMusicManager tiene las 2 canciones asignadas
- [ ] Los AudioClips tienen configuración correcta (2D Sound)

**UI:**
- [ ] PanelGameOver existe en Level1 y Level2
- [ ] PanelGameOver está desactivado por defecto
- [ ] Botones de Game Over conectados correctamente
- [ ] TextoGameOver configurado

**Scripts:**
- [ ] Todos los scripts nuevos están en Assets/Scripts/
- [ ] PlayerHealth.cs tiene código de muerte actualizado
- [ ] PlayerScript.cs tiene código de audio actualizado
- [ ] BossZoneController carga Level2 al derrotar Boss1
- [ ] Boss2ZoneController vuelve al menú al derrotar Boss2

**Pruebas:**
- [ ] Game Over aparece al morir
- [ ] Botón Reintentar funciona
- [ ] Botón Menú Principal funciona
- [ ] Audio del player se reproduce
- [ ] Música cambia entre niveles
- [ ] Flujo completo: MainMenu → IntroCinematic → Level1 → Level2 → MainMenu

---

## 📝 Notas Adicionales

### Flujo Simplificado:
- **Sin selección de dificultad**: El juego inicia directamente desde el botón "Iniciar Juego"
- **Sin cinemática intermedia**: Level1 → Level2 directo al derrotar al boss
- **Game Over en lugar de reinicio automático**: Mayor control del jugador

### Audio 2D vs 3D:
Para que el audio del player se escuche siempre al mismo volumen (recomendado para juegos top-down):
1. Selecciona los AudioClips en Project
2. Inspector → 3D Sound Settings
3. Spatial Blend = 0 (2D)

### Time.timeScale en Game Over:
El juego pausa completamente cuando aparece Game Over (`Time.timeScale = 0`). Esto detiene:
- Movimiento de enemigos
- Animaciones
- Física
- Pero NO detiene la UI ni los botones

### Optimización:
Los GameObjects con DontDestroyOnLoad (LevelMusicManager) solo deben crearse UNA VEZ. Unity los mantiene entre escenas automáticamente.

---

¡Implementación completa! 🎉
