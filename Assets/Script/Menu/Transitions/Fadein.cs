using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadein : MonoBehaviour
{
    public Animator transitionFadein;
    public GameObject imageIn;

    void Start()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        imageIn.SetActive(true);
        transitionFadein.SetTrigger("EndFade");
        yield return new WaitForSecondsRealtime(2f);
        imageIn.SetActive(false);
    }
}
