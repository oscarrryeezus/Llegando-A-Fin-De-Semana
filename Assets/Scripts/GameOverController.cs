using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    // Este script se usa en la ESCENA de Game Over
    // Los botones llaman directamente a estos métodos desde el Inspector

    private void Start()
    {
        // Asegurar que el tiempo esté corriendo en la escena de Game Over
        Time.timeScale = 1f;
    }

    public void Reiniciar()
    {
        // Obtener el nombre del nivel que estaba jugando desde GameManager
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.ultimaEscenaJugada))
        {
            Debug.Log($"Reiniciando nivel: {GameManager.Instance.ultimaEscenaJugada}");
            SceneManager.LoadScene(GameManager.Instance.ultimaEscenaJugada);
        }
        else
        {
            // Fallback: cargar Level1 por defecto
            Debug.LogWarning("No se encontró última escena, cargando Level1");
            SceneManager.LoadScene("Level1");
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
