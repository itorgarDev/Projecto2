using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutscenePass : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    public float transitionTime = 1f;
    public GameObject imageOut;           
    public Animator transitionFadeout;    
    private bool isTransitioning = false;
    
    void Awake()
    {
       
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        
        videoPlayer.loopPointReached += EndCutscene;
    }

    void Update()
    {
        // Si se pulsa cualquier tecla se termina de emitir la cinemática
        if (Input.anyKeyDown && !isTransitioning)
        {
           
            if (videoPlayer != null && videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
            }

            StartCoroutine(TransitionAndLoadRoutine());
        }
    }

    void OnDisable()
    {
     
        videoPlayer.loopPointReached -= EndCutscene;
    }

    void EndCutscene(VideoPlayer vp)
    {

        videoPlayer.loopPointReached -= EndCutscene;
        StartCoroutine(TransitionAndLoadRoutine());
    }

    private IEnumerator TransitionAndLoadRoutine()
    {
       //Se muestra la imagen del panel de transicion
        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        //  Se espera s que se ejecute la animación
        yield return new WaitForSecondsRealtime(transitionTime);

        //Se busca el indice de la escena para determinar el destino de la escena
        bool startingGame = PlayerPrefs.GetInt("WhereCutscene", 0) == 1;
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        int sceneDestination;

        if (currentScene == 4)
        {
            sceneDestination = 0;
        }
        else if (currentScene == 8)
        {
            sceneDestination = 4;
        }
        else
        {
            if (startingGame)
            {
                sceneDestination = 5; 
            }
            else
            {
                sceneDestination = 0;
            }
           
        }
        //Se cambia a la escena debida
        if (sceneDestination < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneDestination);
        }

    }
}
