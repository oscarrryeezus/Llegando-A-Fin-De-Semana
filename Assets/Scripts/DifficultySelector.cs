using UnityEngine;
using UnityEngine.SceneManagement;

// Este script ya no se usa - el juego inicia directamente sin selección de dificultad
// Se mantiene por compatibilidad pero puede ser eliminado
public class DifficultySelector : MonoBehaviour
{
    // Método simple para iniciar el juego directamente
    public void IniciarJuego()
    {
        // Asegurar que existe el GameManager
        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }
        
        // Iniciar el juego
        SceneManager.LoadScene("IntroCinematic");
    }
}
