using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadBase = 4f;
    public float velocidadSprint = 7f;
    private float velocidadActual;

    [Header("Ataque")]
    public GameObject hitboxMartillo;
    public float attackFallbackActiveTime = 0.25f;

    [Header("Salto (visual)")]
    public float fuerzaSalto = 9f;
    public float duracionSalto = 0.6f;
    private bool saltando;
    private bool atacando;
    private bool puedeMoverse = true;

    [Header("Audio")]
    public AudioClip sonidoCaminar;
    public AudioClip sonidoAtaque;
    private AudioSource audioSource;

    [Header("Mobile Controls")]
    public VariableJoystick joystick; // Arrastra el joystick aquí desde el Canvas
    public bool usarJoystick = true; // Activa/desactiva el joystick

    [Header("Referencias")]
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private float movX;
    private float movY;
    private bool estaCaminando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;

        velocidadActual = velocidadBase;

        if (hitboxMartillo != null)
            hitboxMartillo.SetActive(false);
    }

    void Update()
    {
        // Input
        if (puedeMoverse)
        {
            // Priorizar joystick si está activo y asignado
            if (usarJoystick && joystick != null)
            {
                movX = joystick.Horizontal;
                movY = joystick.Vertical;
                
                // En mobile, el sprint puede estar siempre activo o controlado por botón
                velocidadActual = velocidadBase;
            }
            else
            {
                // Input de teclado (para testing en PC)
                movX = Input.GetAxisRaw("Horizontal");
                movY = Input.GetAxisRaw("Vertical");

                bool sprintActivo = Input.GetButton("Sprint");
                velocidadActual = sprintActivo ? velocidadSprint : velocidadBase;
            }
        }

        // Controles de teclado para testing (siempre disponibles en editor)
        if (Input.GetButtonDown("Jump") && !saltando && !atacando)
            StartCoroutine(Saltar());

        if (Input.GetButtonDown("Fire1") && !saltando && !atacando)
            StartCoroutine(Atacar());

        ActualizarAnimaciones();
        AjustarProfundidad();
    }

    void FixedUpdate()
    {
        if (!puedeMoverse || saltando || atacando) return;

        Vector2 direccion = new Vector2(movX, movY).normalized;
        Vector2 destino = rb.position + direccion * velocidadActual * Time.fixedDeltaTime;
        rb.MovePosition(destino);

        // Reproducir sonido de caminar
        bool moviendose = direccion.magnitude > 0.1f;
        if (moviendose && !estaCaminando)
        {
            ReproducirSonidoCaminar();
            estaCaminando = true;
        }
        else if (!moviendose && estaCaminando)
        {
            DetenerSonidoCaminar();
            estaCaminando = false;
        }

        // Flip visual
        if (movX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (movX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    IEnumerator Saltar()
    {
        saltando = true;
        puedeMoverse = false;
        animator.SetTrigger("jump");

        float t = 0f;
        Vector3 inicio = transform.position;

        while (t < duracionSalto)
        {
            float altura = Mathf.Sin((t / duracionSalto) * Mathf.PI) * fuerzaSalto;
            transform.position = new Vector3(inicio.x, inicio.y + altura * 0.08f, inicio.z);

            t += Time.deltaTime;
            yield return null;
        }

        transform.position = inicio;
        saltando = false;
        puedeMoverse = true;
    }

    IEnumerator Atacar()
    {
        atacando = true;
        puedeMoverse = false;
        rb.velocity = Vector2.zero;

        animator.SetTrigger("attack");

        // Reproducir sonido de ataque
        if (sonidoAtaque != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoAtaque);
        }

        // Fallback: si no usas animation events, activa hitbox por tiempo
        if (hitboxMartillo != null)
        {
            // espera un pequeño delay para que la animación llegue al frame del impacto
            yield return new WaitForSeconds(0.12f);
            ActivarHitbox();
            yield return new WaitForSeconds(attackFallbackActiveTime);
            DesactivarHitbox();
        }

        // espera a que termine el resto del ataque (ajustar si tu animación es más larga)
        yield return new WaitForSeconds(0.15f);

        atacando = false;
        puedeMoverse = true;
    }

    // Estos métodos deben llamarse desde Animation Events en el clip de ataque:
    // -> frame donde el martillo impacta -> ActivarHitbox()
    // -> frame después del impacto -> DesactivarHitbox()
    public void ActivarHitbox()
    {
        if (hitboxMartillo != null)
            hitboxMartillo.SetActive(true);
    }

    public void DesactivarHitbox()
    {
        if (hitboxMartillo != null)
            hitboxMartillo.SetActive(false);
    }

    void ActualizarAnimaciones()
    {
        bool moviendo = (movX != 0 || movY != 0) && !saltando && !atacando;
        animator.SetBool("isWalking", moviendo);
        animator.SetBool("isJumping", saltando);
        animator.SetBool("isAttacking", atacando);
        //animator.SetBool("isRunning", velocidadActual == velocidadSprint);
    }

    void AjustarProfundidad()
    {
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }

    void ReproducirSonidoCaminar()
    {
        if (sonidoCaminar != null && audioSource != null && !audioSource.isPlaying)
        {
            audioSource.clip = sonidoCaminar;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void DetenerSonidoCaminar()
    {
        if (audioSource != null && audioSource.isPlaying && audioSource.clip == sonidoCaminar)
        {
            audioSource.Stop();
        }
    }

    // Métodos públicos para botones UI en mobile
    public void BotonAtacar()
    {
        if (!saltando && !atacando)
            StartCoroutine(Atacar());
    }

    public void BotonSaltar()
    {
        if (!saltando && !atacando)
            StartCoroutine(Saltar());
    }
}
