using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public float suavizado = 5f;

    [Header("Referencia al mapa")]
    public SpriteRenderer mapa; // arrastra el sprite del mapa aquí

    private float limiteIzquierdo;
    private float limiteDerecho;

    void Start()
    {
        CalcularLimites();
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        float xDeseado = Mathf.Clamp(objetivo.position.x, limiteIzquierdo, limiteDerecho);

        Vector3 posicionDeseada = new Vector3(
            xDeseado,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            posicionDeseada,
            Time.deltaTime * suavizado
        );
    }

    void CalcularLimites()
    {
        Camera cam = Camera.main;

        float mitadPantalla = cam.orthographicSize * cam.aspect;

        // Obtiene los bordes reales del mapa usando el SpriteRenderer
        float mapaIzquierda = mapa.bounds.min.x;
        float mapaDerecha = mapa.bounds.max.x;

        // Ajusta los límites en base al tamaño de la cámara
        limiteIzquierdo = mapaIzquierda + mitadPantalla;
        limiteDerecho = mapaDerecha - mitadPantalla;
    }
}
