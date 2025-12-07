using UnityEngine;

public class Boss2AttackHitbox : MonoBehaviour
{
    public float daño = 25f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.RecibirDano(daño, transform.position);
            }
        }
    }
}
