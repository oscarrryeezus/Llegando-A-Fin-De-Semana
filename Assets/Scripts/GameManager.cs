using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [HideInInspector]
    public string ultimaEscenaJugada;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarEscena(string nombreEscena)
    {
        ultimaEscenaJugada = nombreEscena;
        Debug.Log($"Escena registrada: {nombreEscena}");
    }
}
