using UnityEngine;
using System;

public class BossHealth : MonoBehaviour
{
    public event Action OnDeath;

    [Header("Config")]
    public float vidaMax = 300f;
    private float vidaActual;
    private bool muerto = false;

    private void Start()
    {
        vidaActual = vidaMax;
    }

    public void TomarDaño(float dmg)
    {
        if (muerto) return;

        vidaActual -= dmg;

        if (vidaActual <= 0)
        {
            muerto = true;
            OnDeath?.Invoke();
            Destroy(gameObject, 1f);
        }
    }
}
