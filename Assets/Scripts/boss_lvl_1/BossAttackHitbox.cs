using UnityEngine;

public class BossAttackHitbox : MonoBehaviour
{
    public float daño = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var d = other.GetComponent<IDamageable>();
            if (d != null)
                d.TomarDaño(daño);
        }
    }
}
