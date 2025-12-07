using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryController : MonoBehaviour
{
    // Este script se usa en la ESCENA de Victoria
    // Los botones llaman directamente a estos métodos desde el Inspector

    private void Start()
    {
        // Asegurar que el tiempo esté corriendo
        Time.timeScale = 1f;
    }

    public void Siguiente()
    {
        // Determinar qué nivel cargar basado en el nivel actual
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.ultimaEscenaJugada))
        {
            string escenaActual = GameManager.Instance.ultimaEscenaJugada;
            
            if (escenaActual == "Level1")
            {
                Debug.Log("Victoria en Level1, cargando Level2");
                SceneManager.LoadScene("Level2");
            }
            else if (escenaActual == "Level2")
            {
                Debug.Log("Victoria en Level2, juego completado - volviendo al menú");
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                // Por defecto, volver al menú
                Debug.LogWarning("Nivel desconocido, volviendo al menú");
                SceneManager.LoadScene("MainMenu");
            }
        }
        else
        {
            // Fallback
            Debug.LogWarning("No se encontró información del nivel, volviendo al menú");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void SalirAlMenu()
    {
        Debug.Log("Volviendo al menú principal");
        SceneManager.LoadScene("MainMenu");
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
