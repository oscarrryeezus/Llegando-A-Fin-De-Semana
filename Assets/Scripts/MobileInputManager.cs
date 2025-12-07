using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    [Header("Configuración")]
    public bool usarControlesMobile = true;
    
    [Header("Joystick Virtual (Opcional)")]
    public GameObject joystickCanvas; // Arrastra el canvas del joystick aquí si lo usas
    
    private void Start()
    {
        // Detectar automáticamente si estamos en mobile
        #if UNITY_ANDROID || UNITY_IOS
            usarControlesMobile = true;
        #else
            usarControlesMobile = false;
        #endif
        
        // Activar/desactivar UI de joystick
        if (joystickCanvas != null)
        {
            joystickCanvas.SetActive(usarControlesMobile);
        }
        
        Debug.Log($"Controles Mobile: {usarControlesMobile}");
    }
    
    public static bool IsMobile()
    {
        #if UNITY_ANDROID || UNITY_IOS
            return true;
        #else
            return Application.isMobilePlatform;
        #endif
    }
}
