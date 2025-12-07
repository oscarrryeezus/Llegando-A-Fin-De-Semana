using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Atributos")]
    public float vidaMaxima = 100f;
    public float vidaActual;

    [Header("Invencibilidad")]
    public float duracionInvencibilidad = 0.6f;
    private bool invencible = false;

    [Header("Knockback")]
    public float fuerzaKnockback = 8f;

    [Header("Referencias")]
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private PlayerScript playerScript;

    void Start()
    {
        vidaActual = vidaMaxima;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerScript = GetComponent<PlayerScript>();
    }

    // Implementación de IDamageable
    public void TomarDaño(float dmg)
    {
        RecibirDano(dmg, default);
    }

    public void RecibirDano(float cantidad, Vector2 origen = default)
    {
        if (invencible) return;
        vidaActual -= cantidad;
        Debug.Log("Vida del player -> " + vidaActual);
        animator.SetBool("isHitted", true);
        if (rb != null)
        {
            Vector2 dir = ((Vector2)transform.position - origen).normalized;
            rb.AddForce(dir * fuerzaKnockback, ForceMode2D.Impulse);
        }
        StartCoroutine(Invencibilidad());
        if (vidaActual <= 0)
            Morir();
    }

    IEnumerator Invencibilidad()
    {
        invencible = true;

        float flashInterval = 0.08f;
        int flashes = Mathf.CeilToInt(duracionInvencibilidad / (flashInterval * 2));

        for (int i = 0; i < flashes; i++)
        {
            sr.color = new Color(1, 1, 1, 0.4f);
            yield return new WaitForSeconds(flashInterval);
            sr.color = Color.white;
            yield return new WaitForSeconds(flashInterval);
        }

        animator.SetBool("isHitted", false);
        invencible = false;
    }

    void Morir()
    {
        if (animator != null) animator.SetTrigger("die");

        if (playerScript != null) playerScript.enabled = false;
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Debug.Log("Player muerto - Cargando escena Game Over");
        
        // Cargar escena de Game Over después de un delay
        StartCoroutine(CargarGameOver());
    }

    System.Collections.IEnumerator CargarGameOver()
    {
        yield return new WaitForSeconds(2f); // Esperar animación de muerte
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }
}
