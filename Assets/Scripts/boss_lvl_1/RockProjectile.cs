using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    public float daño = 15f;
    public float lifetime = 5f;
    public GameObject playerPrefab;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Solo detectar colisión con objetos que tengan el tag "PlayerRockHitbox"
        // Esto asegura que solo el BoxCollider hijo específico reaccione a las piedras
        if (other.collider.CompareTag("PlayerRockHitbox"))
        {
            // Buscar PlayerHealth en el padre
            PlayerHealth ph = other.collider.GetComponentInParent<PlayerHealth>();
            
            if (ph != null)
            {
                ph.RecibirDano(daño, transform.position);
            }
        }

        Destroy(gameObject);
    }
}
