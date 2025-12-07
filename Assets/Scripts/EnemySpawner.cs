using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public Transform[] puntosSpawn;

    void Start()
    {
        foreach (var punto in puntosSpawn)
        {
            Instantiate(enemigoPrefab, punto.position, Quaternion.identity);
        }
    }
}
