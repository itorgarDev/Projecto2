using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    [Header("AudioSources")]

    private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup musicGroup;

    [Space(10)]
    [Header("OST")]

    public AudioClip tutoZone;
    public AudioClip tutoVillage;
    public AudioClip temple;
    public AudioClip zone1;
    public AudioClip boss1;
    public AudioClip village1;
    public AudioClip village2;
    public AudioClip boss2;

    private Coroutine fadeCoroutine;
    [SerializeField] private float fadeDuration = 1.5f; // duración del fade en segundos

    public void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Crear AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = musicGroup;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }



    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // Si ya está sonando esta música, no hacer nada
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;


        // Si ya hay un fade en curso, cancelarlo
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusic(clip));
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        // FADE OUT
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        // FADE IN
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;
    }

    public void StopMusicSmooth()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
