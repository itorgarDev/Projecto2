using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_System : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;

    public Slider sliderMusic;
    public float sliderValueMusic;

    public Slider sliderSFX;
    public float sliderValueSFX;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("VolumenAudio",0.5f);
        AudioListener.volume=slider.value;

        sliderMusic.value = PlayerPrefs.GetFloat("VolumenMusic", 0.5f);
        AudioListener.volume = sliderMusic.value;

        sliderSFX.value = PlayerPrefs.GetFloat("VolumenSFX", 0.5f);
        AudioListener.volume = sliderSFX.value;

    }

    public void ChangeSlider(float valor)
    {
        slider.value = valor;
        PlayerPrefs.SetFloat("volumenAudio", 0.5f);
        AudioListener.volume= slider.value;

    }

    public void ChangeSliderMusic(float valor)
    {
        sliderMusic.value = valor;
        PlayerPrefs.SetFloat("VolumenMusic", 0.5f);
        AudioListener.volume = sliderMusic.value;
        
    }

    public void ChangeSliderSFX(float valor)
    {
        sliderSFX.value = valor;
        PlayerPrefs.SetFloat("VolumenSFX", 0.5f);
        AudioListener.volume = sliderSFX.value;

    }
}
