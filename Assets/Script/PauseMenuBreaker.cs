using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuBreaker : MonoBehaviour
{
    private static PauseMenuBreaker instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
