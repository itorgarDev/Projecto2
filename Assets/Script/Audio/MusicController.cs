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

    private Coroutine transitionRoutine;

    public void Awake()
    {
       // if (clip == null) return;

        // Configurar el AudioSource para ambiente
        //audioSource.clip = clip;
        //audioSource.loop = true;
        //audioSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        // Si ya hay una transición en curso, la detenemos
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(TransitionMusic(clip, 1f));
    }

    private IEnumerator TransitionMusic(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;

        //  Fase 1: bajar volumen
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        //  Fase 2: subir volumen
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / duration);
            yield return null;
        }

        audioSource.volume = startVolume;
    }

}
