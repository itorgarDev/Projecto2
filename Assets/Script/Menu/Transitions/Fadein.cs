using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadein : MonoBehaviour
{
    public Animator transitionFadein;
    public GameObject imageIn;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        imageIn.SetActive(true);
        transitionFadein.SetTrigger("EndFade");
        yield return new WaitForSeconds(1f);
        imageIn.SetActive(false);
    }
}
