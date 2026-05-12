using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Audio_System : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider sliderMaster;
    public Slider sliderMusic;
    public Slider sliderSFX;


    // Start is called before the first frame update
    void Start()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);


        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);


        sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

    }
    void Update()
    { 
        ChangeSlider(sliderMaster.value);
        ChangeSliderMusic(sliderMusic.value);
        ChangeSliderSFX(sliderSFX.value);

    }

    public void ChangeSlider(float valor)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("MasterVolume", valor);

    }

    public void ChangeSliderMusic(float valor)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("MusicVolume", valor);

    }

    public void ChangeSliderSFX(float valor)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(valor) * 20);
        PlayerPrefs.SetFloat("SFXVolume", valor);

    }
}
