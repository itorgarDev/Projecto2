using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip sfx;
    [SerializeField] private AudioClip slider;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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
