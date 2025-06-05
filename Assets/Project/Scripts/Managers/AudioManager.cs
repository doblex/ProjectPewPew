using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] AudioClip throwSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip missSound;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource musicSource;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlayThrowSound()
    {
        if (throwSound != null)
        {
            audioSource.PlayOneShot(throwSound);
        }
    }

    public void PlayHitSound()
    {
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    public void PlayMissSound()
    {
        if (missSound != null)
        {
            audioSource.PlayOneShot(missSound);
        }
    }
}
