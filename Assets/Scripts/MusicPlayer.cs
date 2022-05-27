using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static AudioSource audioSource;
    
    private void Awake()
    {
        if (audioSource)
        {
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }
}