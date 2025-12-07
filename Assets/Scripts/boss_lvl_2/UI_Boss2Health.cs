using UnityEngine;
using UnityEngine.UI;

public class UI_Boss2Health : MonoBehaviour
{
    public static UI_Boss2Health Instance;

    [Header("UI")]
    public GameObject contenedor;
    public Image barraVida;

    [Header("Data")]
    private Boss2Controller boss;

    private void Awake()
    {
        Instance = this;
        contenedor.SetActive(false);
    }

    private void Update()
    {
        if (!boss) return;

        // Calcular porcentaje
        float pct = boss.vida / boss.vidaMaxima;

        // Actualizar imagen
        barraVida.fillAmount = pct;
    }

    /// <summary>
    /// Se llama cuando inicia el combate contra el boss.
    /// </summary>
    public void Inicializar(Boss2Controller bossObj)
    {
        boss = bossObj;
        barraVida.fillAmount = 1f; 
        contenedor.SetActive(true);
    }

    /// <summary>
    /// Se llama cuando el boss muere.
    /// </summary>
    public void Ocultar()
    {
        contenedor.SetActive(false);
        boss = null;
    }
}
