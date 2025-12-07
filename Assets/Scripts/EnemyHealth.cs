using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int vidaMaxima = 100;
    private int vidaActual;

    private Animator animator;

    void Start()
    {
        vidaActual = vidaMaxima;
        animator = GetComponent<Animator>();
    }

    public void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log(vidaActual);

        if (animator != null)
            animator.SetTrigger("hit");

        if (vidaActual <= 0)
            Morir();
    }

    void Morir()
    {
        if (animator != null)
            animator.SetTrigger("death");

        Destroy(gameObject, 0.5f);
    }
}
