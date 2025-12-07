using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour, IDamageable
{
    public event System.Action OnDeath;

    [Header("Vida")]
    public float vidaMaxima = 300f;
    public float vida;

    [Header("Stats")]
    public float velocidad = 2.5f;
    public float dañoMelee = 15f;

    [Header("Rangos")]
    public float rangoPersecucion = 10f;
    public float rangoMelee = 1.3f;
    public float rangoRoca = 6f;

    [Header("Cooldowns")]
    public float cdMelee = 1.2f;
    public float cdRoca = 2f;

    [Header("Refs")]
    public Transform player;
    public Transform puntoDisparo;
    public GameObject rocaPrefab;
    public GameObject hitboxMelee;

    [Header("Knockback")]
    public bool usaKnockback = true;
    public float fuerzaKnockback = 8f;

    private float timerMelee = 0;
    private float timerRoca = 0;
    private Rigidbody2D rb;
    private Animator animator;
    private bool muerto = false;
    private bool atacando = false;
    private bool puedeMoverse = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Si no se arrastró el player en el Inspector, intentar encontrarlo
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        vida = vidaMaxima;
        hitboxMelee.SetActive(false);

        // La UI se inicializará desde BossZoneController cuando se active la zona
    }

    private void Update()
    {
        if (muerto || player == null || !puedeMoverse) return;

        timerMelee += Time.deltaTime;
        timerRoca += Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);

        // Orientación hacia el jugador
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        if (dist <= rangoMelee && !atacando)
        {
            AtaqueMelee();
        }
        else if (dist <= rangoRoca && !atacando)
        {
            AtaqueRango();
        }
        else if (dist <= rangoPersecucion)
        {
            Perseguir();
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (animator != null) animator.SetBool("isWalking", false);
        }
    }

    private void Perseguir()
    {
        if (atacando) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = dir * velocidad;

        if (animator != null) animator.SetBool("isWalking", true);
    }

    private void AtaqueMelee()
    {
        if (timerMelee < cdMelee) return;
        timerMelee = 0;

        atacando = true;
        rb.velocity = Vector2.zero;

        StartCoroutine(MeleeRoutine());
    }

    private IEnumerator MeleeRoutine()
    {
        if (animator != null) animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(0.25f);
        
        hitboxMelee.SetActive(true);
        
        if (Vector2.Distance(transform.position, player.position) <= rangoMelee)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.RecibirDano(dañoMelee, transform.position);
            }
        }

        yield return new WaitForSeconds(0.3f);
        hitboxMelee.SetActive(false);

        if (animator != null) animator.SetBool("isAttacking", false);
        atacando = false;
    }

    private void AtaqueRango()
    {
        if (timerRoca < cdRoca) return;
        timerRoca = 0;

        atacando = true;
        rb.velocity = Vector2.zero;

        StartCoroutine(RangoRoutine());
    }

    private IEnumerator RangoRoutine()
    {
        if (animator != null) animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(0.3f);

        GameObject roca = Instantiate(rocaPrefab, puntoDisparo.position, Quaternion.identity);
        Vector2 dir = (player.position - puntoDisparo.position).normalized;
        roca.GetComponent<Rigidbody2D>().velocity = dir * 8f;

        yield return new WaitForSeconds(0.4f);
        if (animator != null) animator.SetBool("isAttacking", false);
        atacando = false;
    }

    // Implementación de IDamageable
    public void TomarDaño(float dmg)
    {
        if (muerto) return;
        StartCoroutine(RecibirDañoRoutine(dmg, default));
    }

    public void RecibirDaño(float cantidad, Vector2 origen = default)
    {
        if (muerto) return;
        StartCoroutine(RecibirDañoRoutine(cantidad, origen));
    }

    private IEnumerator RecibirDañoRoutine(float cantidad, Vector2 origen)
    {
        vida -= cantidad;

        if (animator != null) animator.SetBool("isHitted", true);

        // Detener movimiento temporalmente
        puedeMoverse = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.25f);

        // Aplicar knockback
        if (usaKnockback && rb != null && origen != default)
        {
            Vector2 dir = ((Vector2)transform.position - origen).normalized;
            rb.AddForce(dir * fuerzaKnockback, ForceMode2D.Impulse);
        }

        if (animator != null) animator.SetBool("isHitted", false);
        puedeMoverse = true;

        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        if (muerto) return;

        muerto = true;
        rb.velocity = Vector2.zero;
        puedeMoverse = false;
        atacando = false;

        // Ocultar UI
        if (UI_BossHealth.Instance != null)
        {
            UI_BossHealth.Instance.Ocultar();
        }

        OnDeath?.Invoke();

        if (animator != null) animator.SetBool("isDeath", true);

        // Desactivar colisión
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        this.enabled = false;
        Destroy(gameObject, 1f);
    }
}
