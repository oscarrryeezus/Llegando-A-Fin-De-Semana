using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TextCinematicController : MonoBehaviour
{
    [Header("UI")]
    public Text textoNarracion;
    public GameObject panel;

    [Header("Configuración")]
    [TextArea(5, 10)]
    public string textoCompleto = "Escribe aquí la narración de lo que pasó en el nivel...";
    public float velocidadTexto = 0.05f;
    public float tiempoEsperaFinal = 3f;
    public string siguienteEscena = "Level2";

    private void Start()
    {
        if (textoNarracion != null)
            textoNarracion.text = "";

        StartCoroutine(MostrarTextoGradual());
    }

    private IEnumerator MostrarTextoGradual()
    {
        yield return new WaitForSeconds(1f);

        // Mostrar texto carácter por carácter
        for (int i = 0; i <= textoCompleto.Length; i++)
        {
            textoNarracion.text = textoCompleto.Substring(0, i);
            yield return new WaitForSeconds(velocidadTexto);
        }

        // Esperar antes de cambiar de escena
        yield return new WaitForSeconds(tiempoEsperaFinal);

        // Cargar siguiente escena
        SceneManager.LoadScene(siguienteEscena);
    }

    // Método para saltar la cinemática con cualquier tecla
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StopAllCoroutines();
            textoNarracion.text = textoCompleto;
            StartCoroutine(CargarSiguienteEscena());
        }
    }

    private IEnumerator CargarSiguienteEscena()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(siguienteEscena);
    }
}
