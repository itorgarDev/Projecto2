using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pruebaEscenas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 1)
            { SceneManager.LoadScene(2); }
            if (currentScene == 2)
            { SceneManager.LoadScene(1); }
        }

    }
}
