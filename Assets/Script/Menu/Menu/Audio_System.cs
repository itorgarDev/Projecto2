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
        //sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        //AudioListener.volume=sliderMaster.value;
        //sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        //sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        
        //MASTER
        float master = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        sliderMaster.value = master;
        AudioListener.volume = master;

        // MUSIC
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sliderMusic.value = music;
        mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(music, 0.0001f)) * 20);

        // SFX
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        sliderSFX.value = sfx;
        mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(sfx, 0.0001f)) * 20);
    }

    public void ChangeSlider(float valor)
    {
        sliderMaster.value = valor;
        float volumen = Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20;
        mixer.SetFloat("MasterVolume", volumen);
        PlayerPrefs.SetFloat("MasterVolume", valor);



    }

    public void ChangeSliderMusic(float valor)
    {
        sliderMusic.value = valor;
        float volumen = Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20;
        mixer.SetFloat("MusicVolume", volumen);
        PlayerPrefs.SetFloat("MusicVolume", valor);

    }

    public void ChangeSliderSFX(float valor)
    {
        sliderSFX.value = valor;
        float volumen = Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20;
        mixer.SetFloat("SFXVolume", volumen);
        PlayerPrefs.SetFloat("SFXVolume", valor);


    }
}
