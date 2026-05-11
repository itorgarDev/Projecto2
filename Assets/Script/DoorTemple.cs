using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTemple : MonoBehaviour
{
   

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            FindObjectOfType<PlayerStats>().SaveStats();

            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 1)
            { SceneManager.LoadScene(2); }
            if (currentScene==2)
            { SceneManager.LoadScene(1); }
        }
    }
}
