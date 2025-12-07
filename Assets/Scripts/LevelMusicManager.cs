using UnityEngine;

public class LevelMusicManager : MonoBehaviour
{
    public static LevelMusicManager Instance;

    [Header("Music Clips")]
    public AudioClip level1Music;
    public AudioClip level2Music;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayLevelMusic(int level)
    {
        AudioClip clip = null;

        switch (level)
        {
            case 1:
                clip = level1Music;
                break;
            case 2:
                clip = level2Music;
                break;
        }

        if (clip != null && audioSource.clip != clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log($"Reproduciendo música del nivel {level}");
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
