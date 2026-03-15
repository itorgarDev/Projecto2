using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_System : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("VolumenAudio",0.5f);
        AudioListener.volume=slider.value;
        
    }

    public void ChangeSlider(float valor)
    {
        slider.value = valor;
        PlayerPrefs.SetFloat("volumenAudio", 0.5f);
        AudioListener.volume= slider.value;

    }
}
