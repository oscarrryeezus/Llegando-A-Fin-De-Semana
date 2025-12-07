using UnityEngine;
using System.Collections.Generic;

public class EnemyZone : MonoBehaviour
{
    [Header("Config Zona")]
    public Transform spawner;              // Contiene los enemigos ya colocados
    public GameObject leftWall;
    public GameObject rightWall;

    private bool zonaActiva = false;
    private List<GameObject> enemigosVivos = new List<GameObject>();

    private void Start()
    {
        if (leftWall) leftWall.SetActive(false);
        if (rightWall) rightWall.SetActive(false);

        // Desactivar todos los enemigos al inicio
        foreach (Transform child in spawner)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (zonaActiva) return;
        if (!other.CompareTag("Player")) return;

        zonaActiva = true;
        ActivarZona();
    }

    private void ActivarZona()
    {
        // Activar paredes
        if (leftWall) leftWall.SetActive(true);
        if (rightWall) rightWall.SetActive(true);

        // Activar enemigos precargados
        foreach (Transform child in spawner)
        {
            GameObject enemy = child.gameObject;
            enemy.SetActive(true);
            enemigosVivos.Add(enemy);

            EnemyScript e = enemy.GetComponent<EnemyScript>();
            e.OnDeath += () =>
            {
                enemigosVivos.Remove(enemy);
                ValidarFinDeZona();
            };
        }
    }

    private void ValidarFinDeZona()
    {
        if (enemigosVivos.Count == 0)
        {
            if (leftWall) leftWall.SetActive(false);
            if (rightWall) rightWall.SetActive(false);
            ArrowUIManager.Instance.ShowArrow();
            Destroy(gameObject, 0.5f);
        }
    }
}
