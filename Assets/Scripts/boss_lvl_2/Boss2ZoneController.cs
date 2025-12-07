using UnityEngine;

public class Boss2ZoneController : MonoBehaviour
{
    [Header("Config Zona")]
    public Boss2Controller boss;           // Referencia al boss 2
    public GameObject leftWall;
    public GameObject rightWall;

    private bool zonaActiva = false;

    private void Start()
    {
        if (leftWall) leftWall.SetActive(false);
        if (rightWall) rightWall.SetActive(false);

        // Desactivar el boss al inicio
        if (boss != null)
        {
            boss.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (zonaActiva) return;
        if (!other.CompareTag("Player")) return;

        zonaActiva = true;
        ActivarZona();
    }

    private void ActivarZona()
    {
        // Activar paredes
        if (leftWall) leftWall.SetActive(true);
        if (rightWall) rightWall.SetActive(true);

        // Activar el boss
        if (boss != null)
        {
            boss.gameObject.SetActive(true);

            // Activar UI del boss 2
            if (UI_Boss2Health.Instance != null)
            {
                UI_Boss2Health.Instance.Inicializar(boss);
            }

            // Suscribirse al evento de muerte del boss
            boss.OnDeath += OnBossDerrotado;
        }
    }

    private void OnBossDerrotado()
    {
        // Desactivar paredes
        if (leftWall) leftWall.SetActive(false);
        if (rightWall) rightWall.SetActive(false);

        // Mostrar flecha de dirección si existe
        if (ArrowUIManager.Instance != null)
        {
            ArrowUIManager.Instance.ShowArrow();
        }

        // Cargar escena de Victoria después de derrotar al boss 2
        StartCoroutine(CargarVictoria());
    }

    private System.Collections.IEnumerator CargarVictoria()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Boss 2 derrotado - Cargando escena Victoria (juego completado)");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");
    }
}
