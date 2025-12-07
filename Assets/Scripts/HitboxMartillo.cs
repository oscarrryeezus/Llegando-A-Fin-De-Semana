using UnityEngine;

public class HitboxMartillo : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.RecibirDaño(damage, transform.position);
            return;
        }

        // Verificar si es el boss usando la interfaz IDamageable
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TomarDaño(damage);
        }
    }
}
