using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutscenePass : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    public float transitionTime = 1f;
    public GameObject imageOut;           // Panel negro de UI para fadeout
    public Animator transitionFadeout;    // Animator con el trigger "StartFade"
    private bool isTransitioning = false;
    int currentScene=SceneManager.GetActiveScene().buildIndex;
    void Awake()
    {
        // Obtenemos la referencia al componente VideoPlayer
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        // Nos suscribimos al evento que se activa al terminar el video
        videoPlayer.loopPointReached += EndCutscene;
    }

    void Update()
    {
        // Si el usuario pulsa cualquier tecla o botón del ratón, y NO ha empezado ya la transición...
        if (Input.anyKeyDown && !isTransitioning)
        {
            // Opcional: Detener el video inmediatamente al saltarlo para que deje de sonar
            if (videoPlayer != null && videoPlayer.isPlaying)
            {
                videoPlayer.Stop();
            }

            StartCoroutine(TransitionAndLoadRoutine());
        }
    }

    void OnDisable()
    {
        // Nos desuscribimos del evento por buena práctica y evitar errores de memoria
        videoPlayer.loopPointReached -= EndCutscene;
    }

    void EndCutscene(VideoPlayer vp)
    {
        // Desuscribimos inmediatamente para evitar que se ejecute dos veces si el video se bugea
        videoPlayer.loopPointReached -= EndCutscene;
        StartCoroutine(TransitionAndLoadRoutine());
    }

    private IEnumerator TransitionAndLoadRoutine()
    {
        // 1. Activamos los componentes visuales del fundido (igual que en tu MainMenuController)
        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        // 2. Esperamos en tiempo real lo que tarde en completarse el fundido a negro
        yield return new WaitForSecondsRealtime(transitionTime);
        // Recuperamos el estado de "WhereCutscene" guardado por el MainMenuController
        // Si es 1 (true) significa que viene de "Iniciar Juego". Si es 0 (false) viene del botón "Cinemática".
        bool startingGame = PlayerPrefs.GetInt("WhereCutscene", 0) == 1;

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
                sceneDestination = 5; // Destino si pulsó Jugar/Iniciar Juego
            }
            else
            {
                sceneDestination = 0; // Destino si pulsó el botón de Ver Cinemática directamente
            }

            // Validación de seguridad para asegurarse de que la escena existe en Build Settings
            if (sceneDestination < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(sceneDestination);
            }
        }
        
    }
}
