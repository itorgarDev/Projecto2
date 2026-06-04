using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTemple : MonoBehaviour
{
    public int sceneDestination;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            FindObjectOfType<PlayerStats>().SaveStats();

            SceneManager.LoadScene(sceneDestination);
        }
    }
}
