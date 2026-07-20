using UnityEngine;


public class AudioManager : MonoBehaviour
{


    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); // must be a root object for DontDestroyOnLoad to work
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;   // for background music (looping)
    [SerializeField] private AudioSource sfxSource;     // for sound effects (one shot)

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic; // swamp ambient loop

    [Header("SFX")]
    [SerializeField] private AudioClip jumpClip;        // frog jump
    [SerializeField] private AudioClip coinClip;        // coin collect
    [SerializeField] private AudioClip deathClip;       // water splash
    [SerializeField] private AudioClip winClip;         // win jingle
    [SerializeField] private AudioClip loseClip;
    private void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length >= 2)
        {
            musicSource = sources[0];
            sfxSource = sources[1];
        }
        else if (sources.Length == 1)
        {
            musicSource = sources[0];
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = 0.4f;

        if (!musicSource.isPlaying)
            musicSource.Play();

        sfxSource.loop = false;
        sfxSource.volume = 1f;
    }



    public void PlayJump()
    {
        PlaySFX(jumpClip);
    }

    public void PlayCoin()
    {
        PlaySFX(coinClip);
    }

    /// Call from DeathZone when frog dies.
    public void PlayDeath()
    {
        PlaySFX(deathClip);
    }

    /// Call from WinZone when frog wins.
    public void PlayWin()
    {
        // Stop background music when winning
        if (musicSource != null)
            musicSource.Stop();

        PlaySFX(winClip);
    }
    public void PlayLose()
    {
        PlaySFX(loseClip);
    }

    // Private Helper

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }
}