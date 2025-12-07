using UnityEngine;
using System.Collections;

public class Boss2Controller : MonoBehaviour, IDamageable
{
    public event System.Action OnDeath;

    [Header("Vida")]
    public float vidaMaxima = 400f;
    public float vida;
    public float vidaParaFase2 = 200f; // Cuando llega a esta vida, pasa a fase 2

    [Header("Stats Fase 1")]
    public float velocidadFase1 = 2.5f;
    public float dañoMeleeFase1 = 15f;

    [Header("Stats Fase 2 (Potenciado)")]
    public float velocidadFase2 = 4.5f;
    public float dañoMeleeFase2 = 25f;
    public float dañoLaser = 30f;

    [Header("Rangos")]
    public float rangoPersecucion = 12f;
    public float rangoMelee = 1.5f;
    public float rangoLaser = 8f;

    [Header("Cooldowns")]
    public float cdMelee = 1.5f;
    public float cdLaser = 3f;

    [Header("Refs")]
    public Transform player;
    public GameObject hitboxMelee;
    public LaserController laserController;
    public GameObject fumarEfecto; // Efecto visual de fumar

    [Header("Knockback")]
    public bool usaKnockback = true;
    public float fuerzaKnockback = 8f;

    // Variables de estado
    private float velocidadActual;
    private float dañoMeleeActual;
    private float timerMelee = 0;
    private float timerLaser = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private bool muerto = false;
    private bool atacando = false;
    private bool puedeMoverse = true;
    private bool enFase2 = false;
    private bool transicionandoFase = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        vida = vidaMaxima;
        velocidadActual = velocidadFase1;
        dañoMeleeActual = dañoMeleeFase1;

        if (hitboxMelee != null) hitboxMelee.SetActive(false);
        if (fumarEfecto != null) fumarEfecto.SetActive(false);
    }

    private void Update()
    {
        if (muerto || player == null || !puedeMoverse || transicionandoFase) return;

        // Verificar si debe pasar a fase 2
        if (!enFase2 && vida <= vidaParaFase2)
        {
            StartCoroutine(TransicionarAFase2());
            return;
        }

        timerMelee += Time.deltaTime;
        if (enFase2) timerLaser += Time.deltaTime;

        float dist = Vector2.Distance(transform.position, player.position);

        // Orientación hacia el jugador
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // Lógica de ataque según fase
        if (dist <= rangoMelee && !atacando)
        {
            AtaqueMelee();
        }
        else if (enFase2 && dist <= rangoLaser && dist > rangoMelee && !atacando)
        {
            AtaqueLaser();
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
        rb.velocity = dir * velocidadActual;

        if (animator != null) animator.SetBool("isWalking", true);
    }

    private void AtaqueMelee()
    {
        if (timerMelee < cdMelee) return;
        timerMelee = 0;

        atacando = true;
        rb.velocity = Vector2.zero;
        if (animator != null) animator.SetBool("isAttacking", true);

        StartCoroutine(MeleeRoutine());
    }

    private IEnumerator MeleeRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        
        if (hitboxMelee != null) hitboxMelee.SetActive(true);
        
        if (Vector2.Distance(transform.position, player.position) <= rangoMelee)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.RecibirDano(dañoMeleeActual, transform.position);
            }
        }

        yield return new WaitForSeconds(0.3f);
        if (hitboxMelee != null) hitboxMelee.SetActive(false);

        if (animator != null) animator.SetBool("isAttacking", false);
        atacando = false;
    }

    private void AtaqueLaser()
    {
        if (timerLaser < cdLaser) return;
        timerLaser = 0;

        atacando = true;
        rb.velocity = Vector2.zero;

        StartCoroutine(LaserRoutine());
    }

    private IEnumerator LaserRoutine()
    {
        if (animator != null) animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(0.3f);

        // Disparar láser usando el controlador
        if (laserController != null && laserController.PuedeDisparar())
        {
            laserController.DispararLaser();
            yield return new WaitForSeconds(0.7f); // Esperar a que termine el láser
        }
        else
        {
            Debug.LogError("LaserController no está configurado o no puede disparar");
            yield return new WaitForSeconds(0.5f);
        }

        if (animator != null) animator.SetBool("isAttacking", false);
        atacando = false;
    }

    private IEnumerator TransicionarAFase2()
    {
        transicionandoFase = true;
        rb.velocity = Vector2.zero;
        puedeMoverse = false;

        // Animación de fumar
        if (animator != null) animator.SetBool("smoke", true);
        if (fumarEfecto != null) fumarEfecto.SetActive(true);

        Debug.Log("Boss entrando en Fase 2 - ¡Potenciado!");

        yield return new WaitForSeconds(2f);

        // Desactivar animación de fumar
        if (animator != null) animator.SetBool("smoke", false);

        // Cambiar stats a Fase 2
        enFase2 = true;
        velocidadActual = velocidadFase2;
        dañoMeleeActual = dañoMeleeFase2;

        if (fumarEfecto != null) fumarEfecto.SetActive(false);

        puedeMoverse = true;
        transicionandoFase = false;
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

        puedeMoverse = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.25f);

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

        if (UI_Boss2Health.Instance != null)
        {
            UI_Boss2Health.Instance.Ocultar();
        }

        OnDeath?.Invoke();

        if (animator != null) animator.SetBool("isDeath", true);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        this.enabled = false;
        Destroy(gameObject, 1f);
    }
}
