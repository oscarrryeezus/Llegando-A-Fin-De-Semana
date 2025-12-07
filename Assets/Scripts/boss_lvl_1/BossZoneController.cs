using UnityEngine;

public class BossZoneController : MonoBehaviour
{
    [Header("Config Zona")]
    public BossController boss;           // Referencia al boss
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

            // Activar UI del boss
            if (UI_BossHealth.Instance != null)
            {
                UI_BossHealth.Instance.Inicializar(boss);
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

        // Cargar escena de Victoria después de derrotar al boss 1
        StartCoroutine(CargarVictoria());
    }

    private System.Collections.IEnumerator CargarVictoria()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Boss 1 derrotado - Cargando escena Victoria");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");
    }
}
