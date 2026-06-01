using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightness_System : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image panelBrillo;
    public float valueWhite;
    public float valueBlack;

    // Start is called before the first frame update
    void Start()
    {
        float brillo = SavePlay.Instance.brightness;
        slider.value = brillo;
        sliderValue = brillo;
        panelBrillo.color = new Color(panelBrillo.color.r,panelBrillo.color.g,panelBrillo.color.b,slider.value/3);

        SavePlay.Instance.brightness = slider.value;

        SavePlay.Instance.SaveData();
    }

    void Update()
    {
        valueBlack = 1 - sliderValue - 0.5f;
        valueWhite = sliderValue - 0.5f;
        if (sliderValue < 0.5f)
        {
            panelBrillo.color = new Color(0, 0, 0, valueBlack);
        }
        if (sliderValue > 0.5f)
        {
            panelBrillo.color = new Color(255, 255, 255, valueWhite);
        }
    }

    public void ChangeSlider (float valor)
    {
        sliderValue=valor;
        PlayerPrefs.SetFloat("brillo", sliderValue);
        panelBrillo.color = new Color(panelBrillo.color.r, panelBrillo.color.g, panelBrillo.color.b, slider.value/3);

        SavePlay.Instance.brightness = valor;
        SavePlay.Instance.SaveData();
    }

    public void ApplyBrightness(float valor)
    {
        slider.value = valor;
        sliderValue = valor;

        panelBrillo.color = new Color(
            panelBrillo.color.r,
            panelBrillo.color.g,
            panelBrillo.color.b,
            valor / 3
        );
    }

}

