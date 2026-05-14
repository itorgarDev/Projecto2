using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_System : MonoBehaviour
{
    public float transitionTime = 1f;   // Duración de la animación

    public Animator transitionFadeout;
    public void Start()
    {

    }


    public void Play()
    {
        StartCoroutine(LoadSceneWithTransition());
    }

    IEnumerator LoadSceneWithTransition()
    {
        //Time.timeScale = 0f;
        transitionFadeout.SetTrigger("StartFade"); // Activa el Fade_out
        yield return new WaitForSeconds(transitionTime); // Espera a que termine
        SceneManager.LoadScene(1); // Carga la escena del juego
        Time.timeScale = 1f; // Asegura que el juego esté activo
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Debug.Log("Saliendo");
        Application.Quit();
    }
}
