using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuBreaker : MonoBehaviour
{
    private static PauseMenuBreaker instance;

    public GameObject panelScroll;
    public GameObject panelOptions;
    public GameObject panelVideo;
    public GameObject panelAudio;
    public GameObject panelBrillo;
    public Animator scrollAnimator;

  /* void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
  */
}
