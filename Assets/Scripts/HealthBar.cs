using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("UI")]
    public Image fillBar;
    public PlayerHealth playerHealth;

    private void Start()
    {
        if (!playerHealth)
            playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        if (!playerHealth) return;

        float pct = (float)playerHealth.vidaActual / playerHealth.vidaMaxima;
        fillBar.fillAmount = pct;
    }
}
