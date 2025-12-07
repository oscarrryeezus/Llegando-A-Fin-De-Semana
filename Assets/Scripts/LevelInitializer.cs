using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInitializer : MonoBehaviour
{
    [Header("Configuración")]
    public int numeroNivel = 1;

    private void Start()
    {
        // Registrar la escena actual en GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegistrarEscena(SceneManager.GetActiveScene().name);
        }

        // Reproducir música del nivel
        if (LevelMusicManager.Instance != null)
        {
            LevelMusicManager.Instance.PlayLevelMusic(numeroNivel);
        }

        Debug.Log($"Nivel {numeroNivel} inicializado");
    }
}