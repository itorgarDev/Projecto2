using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ButtonSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioClip slider;
    [SerializeField] private AudioMixerGroup sfxGroup;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        if (sfxGroup != null)
            audioSource.outputAudioMixerGroup = sfxGroup;
    }
    public void ActivateSound ()
    {
        audioSource.PlayOneShot(sfx);
    }

    public void ActiveSliderSound()
    {
        audioSource.PlayOneShot(slider);
    }
}
