using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Puerta_Templo : MonoBehaviour
{
   

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 1)
            { SceneManager.LoadScene(2); }
            if (currentScene==2)
            { SceneManager.LoadScene(1); }
        }
    }
}
