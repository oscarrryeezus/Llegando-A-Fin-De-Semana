using System.Collections;
using UnityEngine;

public enum TipoEnemigo
{
    Ligero,
    Pesado,
    Agil
}

public class EnemyScript : MonoBehaviour
{
    [Header("Tipo de enemigo")]
    public TipoEnemigo tipo;

    [Header("Atributos")]
    public float vida = 100f;
    public float velocidad = 2f;
    public float rangoDeteccion = 5f;
    public float rangoAtaque = 1.5f;
    public float daño = 10f;
    public float tiempoEntreAtaques = 1.5f;

    [Header("Comportamientos avanzados")]
    public bool usaEsquiva = false;
    public float probEsquiva = 0.2f;

    public bool usaKnockback = false;
    public float fuerzaKnockback = 10f;

    [Header("Referencias")]
    public Transform jugador;
    private Animator animator;
    private Rigidbody2D rb;

    private bool atacando;
    private bool puedeAtacar = true;
    private bool vivo = true;
    public event System.Action OnDeath;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (jugador == null)
            jugador = GameObject.FindGameObjectWithTag("Player").transform;

        ConfigurarTipo();
    }

    void Update()
    {
        if (!vivo || jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeteccion && !atacando)
            PerseguirJugador();
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }

        if (distancia <= rangoAtaque && puedeAtacar && !atacando)
            StartCoroutine(Atacar());

        // Flip
        if (jugador.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void PerseguirJugador()
    {
        Vector2 dir = (jugador.position - transform.position).normalized;

        if (tipo == TipoEnemigo.Agil)
        {
            float zigzag = Mathf.Sin(Time.time * 8f) * 0.5f;
            dir.y += zigzag;
        }

        rb.velocity = dir * velocidad;
        animator.SetBool("isWalking", true);
    }

    IEnumerator Atacar()
    {
        atacando = true;
        puedeAtacar = false;
        rb.velocity = Vector2.zero;
        animator.SetBool("isAttacking", atacando);

        yield return new WaitForSeconds(0.25f);
        if (Vector2.Distance(transform.position, jugador.position) <= rangoAtaque)
        {
            Vector2 origen = transform.position;
            PlayerHealth ph = jugador.GetComponent<PlayerHealth>();
            ph.RecibirDano(daño, origen);
        }

        yield return new WaitForSeconds(tiempoEntreAtaques);
        atacando = false;
        animator.SetBool("isAttacking", atacando);
        puedeAtacar = true;
    }

    public void RecibirDaño(float cantidad, Vector2 origen = default)
    {
        if (!vivo) return;
        StartCoroutine(RecibirDañoRoutine(cantidad, origen));
    }

    private IEnumerator RecibirDañoRoutine(float cantidad, Vector2 origen)
    {
        // Esquiva chance
        if (usaEsquiva && Random.value < probEsquiva)
        {
            yield return StartCoroutine(Esquivar());
            yield break;
        }

        vida -= cantidad;
        if (animator != null) animator.SetBool("isHitted", true);

        // Espera la animación de HIT
        yield return new WaitForSeconds(0.25f);

        // Knockback
        if (usaKnockback && rb != null)
        {
            Vector2 dir = ((Vector2)transform.position - origen).normalized;
            rb.AddForce(dir * fuerzaKnockback, ForceMode2D.Impulse);
        }

        if (animator != null) animator.SetBool("isHitted", false);

        if (vida <= 0)
            Morir();
    }

    IEnumerator Esquivar()
    {
        animator.SetTrigger("dodge");
        rb.velocity = new Vector2(-transform.localScale.x * 4, 0);
        yield return new WaitForSeconds(0.35f);
        rb.velocity = Vector2.zero;
    }

    void Morir()
    {
        if (!vivo) return;
        OnDeath?.Invoke();
        StartCoroutine(MorirRoutine());
    }

    private IEnumerator MorirRoutine()
    { 
        vivo = false;

        rb.velocity = Vector2.zero;
        this.enabled = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (animator != null)
            animator.SetBool("isDeath", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void ConfigurarTipo()
    {
        switch (tipo)
        {
            case TipoEnemigo.Ligero:
                vida = 40;
                velocidad = 3.5f;
                daño = 8;
                rangoDeteccion = 6;
                rangoAtaque = 1.2f;
                tiempoEntreAtaques = 0.8f;
                usaEsquiva = false;
                usaKnockback = false;
                break;

            case TipoEnemigo.Pesado:
                vida = 150;
                velocidad = 1.5f;
                daño = 25;
                rangoDeteccion = 5;
                rangoAtaque = 2.2f;
                tiempoEntreAtaques = 2.5f;
                usaKnockback = true;
                fuerzaKnockback = 13f;
                break;

            case TipoEnemigo.Agil:
                vida = 70;
                velocidad = 5f;
                daño = 12;
                rangoDeteccion = 8f;
                rangoAtaque = 2f;
                tiempoEntreAtaques = 1.2f;
                usaEsquiva = true;
                probEsquiva = 0.25f;
                break;
        }
    }
}
