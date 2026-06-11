using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [Header("Tiempo")]
    [Range(0, 1)] public float time; // 0 = dia, 1 = atardecer
    public float cycleDurationInSeconds = 1800f; // 30 minutos = 1800s (El ciclo completo de Dia-Atardecer y Atardecer-Día)

    [Header("Referencias")]
    public Light sun;

    [Header("Luz")]
    public Gradient lightColor;
    public AnimationCurve intensityCurve;

    [Header("Ambiente (sombras)")]
    public Color ambientDay = new Color(0.6f, 0.7f, 0.9f);
    public Color ambientSunset = new Color(0.3f, 0.2f, 0.5f);

    void Update()
    {
        // Tiempo en bucle suave (ida y vuelta)
        float speed = 2f / cycleDurationInSeconds;
        time = Mathf.PingPong(Time.time * speed, 1);

        // Rotación del sol
        float angle = Mathf.Lerp(60f, 170f, time);
        sun.transform.rotation = Quaternion.Euler(angle, -30f, 0);

        // Color de la luz
        sun.color = lightColor.Evaluate(time);

        // Intensidad
        sun.intensity = intensityCurve.Evaluate(time);

        // Color ambiente (sombras)
        RenderSettings.ambientLight = Color.Lerp(ambientDay, ambientSunset, time);
    }
}