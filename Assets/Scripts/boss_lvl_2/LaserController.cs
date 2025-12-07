using UnityEngine;

public class LaserController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform puntoDisparo;
    public GameObject playerPrefab;
    public LineRenderer lineRenderer;

    [Header("Configuración")]
    public float rangoMaximo = 8f;
    public float daño = 30f;
    public float duracionVisual = 0.5f;

    [Header("Visual")]
    public Color colorLaser = Color.red;
    public float grosorLaser = 0.15f;

    private Transform playerTransform;

    private void Start()
    {
        // Encontrar el player
        if (playerPrefab != null)
        {
            playerTransform = playerPrefab.transform;
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }

        // Configurar LineRenderer
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
            lineRenderer.startWidth = grosorLaser;
            lineRenderer.endWidth = grosorLaser;
            lineRenderer.startColor = colorLaser;
            lineRenderer.endColor = colorLaser;
            lineRenderer.positionCount = 2;
        }
    }

    /// <summary>
    /// Dispara el rayo láser hacia el jugador
    /// </summary>
    public void DispararLaser()
    {
        if (lineRenderer == null || puntoDisparo == null || playerTransform == null)
        {
            Debug.LogError("LaserController: Referencias faltantes para disparar láser");
            return;
        }

        StartCoroutine(DispararLaserRoutine());
    }

    private System.Collections.IEnumerator DispararLaserRoutine()
    {
        Debug.Log("=== LÁSER DISPARADO ===");

        lineRenderer.enabled = true;
        Vector2 direccion = (playerTransform.position - puntoDisparo.position).normalized;
        
        Debug.Log($"Desde: {puntoDisparo.position} hacia Player: {playerTransform.position}");
        Debug.Log($"Dirección: {direccion}, Rango: {rangoMaximo}");

        // Raycast para detectar colisión
        RaycastHit2D hit = Physics2D.Raycast(puntoDisparo.position, direccion, rangoMaximo);
        
        Vector3 puntoFinal;
        bool golpeoAlPlayer = false;

        if (hit.collider != null)
        {
            puntoFinal = hit.point;
            Debug.Log($"Hit: {hit.collider.name} (Tag: {hit.collider.tag}) en {hit.point}");

            // Verificar si golpeó al player (por tag o por referencia)
            if (hit.collider.CompareTag("Player") || hit.transform == playerTransform)
            {
                golpeoAlPlayer = true;
                PlayerHealth ph = hit.collider.GetComponent<PlayerHealth>();
                
                if (ph == null)
                    ph = hit.collider.GetComponentInParent<PlayerHealth>();

                if (ph != null)
                {
                    ph.RecibirDano(daño, puntoDisparo.position);
                    Debug.Log($"¡DAÑO APLICADO! {daño} puntos");
                }
                else
                {
                    Debug.LogWarning("Player sin componente PlayerHealth");
                }
            }
            else
            {
                Debug.Log("Golpeó algo que no es el player");
            }
        }
        else
        {
            puntoFinal = puntoDisparo.position + (Vector3)(direccion * rangoMaximo);
            Debug.Log("Sin colisión - láser a máximo rango");
        }

        // Configurar LineRenderer
        lineRenderer.SetPosition(0, puntoDisparo.position);
        lineRenderer.SetPosition(1, puntoFinal);
        Debug.Log($"Distancia láser: {Vector3.Distance(puntoDisparo.position, puntoFinal):F2}");

        yield return new WaitForSeconds(duracionVisual);
        
        lineRenderer.enabled = false;
        Debug.Log("=== LÁSER DESACTIVADO ===");
    }

    /// <summary>
    /// Verifica si el láser puede dispararse (referencias válidas)
    /// </summary>
    public bool PuedeDisparar()
    {
        return lineRenderer != null && puntoDisparo != null && playerTransform != null;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualizar rango del láser en el editor
        if (puntoDisparo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDisparo.position, rangoMaximo);
        }
    }
}
