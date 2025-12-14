using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

[Header("Audio Sources")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip sfxClip;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [Header("Volumes")]
    [Range(0f, 1f)] public float bgmVolume = 0.3f;       // Lower BGM volume
    [Range(0f, 1f)] public float sfxVolume = 1f;         // Full volume for SFX

    void Awake()
    {
        instance = this;

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.volume = 0.3f;
        bgmSource.Play();

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = sfxClip;
        sfxSource.loop = false;
        sfxSource.volume = 1f;
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource == null || clip == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null)
            bgmSource.volume = bgmVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }


}
