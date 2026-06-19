using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Configuración de Transición")]
    public float transitionTime = 1f;
    public GameObject imageOut;             
    public Animator transitionFadeout;     

    [Header("Paneles del Menú")]
    public GameObject menuPrincipal;
    public GameObject menuAreUSure;

    [Header("Destinos de Escena / Triggers (Menu_System)")]
    public int sceneDestination;
    public int checkpointDestination;
    public bool whereCutscene;

   
    public static bool returningToScene = false;
    public static bool isResumingGame = false;
    public static bool comingFromCheckpointButton = false;

    
    public bool firstGame = false;
    private int current;


    private void Start()
    {
        if (imageOut != null) imageOut.SetActive(false);
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
        if (menuAreUSure != null) menuAreUSure.SetActive(false);

        
        if (SavePlay.Instance != null)
        {
            SavePlay.Instance.LoadData();
            firstGame = SavePlay.Instance.firstGameActive;
            Debug.Log("MainMenuController inicializado. ¿Partida activa?: " + firstGame);
        }

       
        whereCutscene = PlayerPrefs.GetInt("WhereCutscene", 0) == 1;
        Debug.Log("Estado de whereCutscene recuperado: " + whereCutscene);
    }

    
    public void StartGame()
    {
       
        if (firstGame)
        {
            AreYouSure();
        }
        else
        {
            ResetPrefs();
            Debug.Log("StartGame pulsado (Nueva Partida)");
            if (SavePlay.Instance != null)
            {
                SavePlay.Instance.SetFirstGame(true);
                SavePlay.Instance.lastCheckpoint = 0;
            }
            whereCutscene = true;
            PlayerPrefs.SetInt("WhereCutscene", 1);
            RespawnSystem.CurrentCheckpointIndex = 0;

            
            GoToDestination(3);
        }
    }

  //Metodo usado para reiniciar datos
    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs borrados por completo");

        if (SavePlay.Instance != null)
        {
            SavePlay.Instance.lastScene = 5; 
            SavePlay.Instance.lastCheckpoint = 0;
            SavePlay.Instance.firstGameActive = false;
            SavePlay.Instance.vida = 5;
            SavePlay.Instance.ataque = 1;
            SavePlay.Instance.maxHealth = 5;
            SavePlay.Instance.SaveData();
        }

        RespawnSystem.CurrentCheckpointIndex = 0;
    }

    public void MainMenu()
    {
        GoToDestination(0);
    }

    public void GoToDestination(int valor)
    {
        sceneDestination = valor;
        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        StartCoroutine(LoadSceneWithTransition());
    }

   

    public void GoToCutscene(int valorScene)
    {
        PlayerPrefs.SetInt("WhereCutscene", 0);
        if (imageOut != null) imageOut.SetActive(true);
        GoToDestination(valorScene);
    }

    public void WhereToCutscene()
    {
        if (whereCutscene) GoToDestination(5);
        else GoToDestination(0);
    }

   
    public void AreYouSure()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(false);
        if (menuAreUSure != null) menuAreUSure.SetActive(true);
    }

    public void NoImNot()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
        if (menuAreUSure != null) menuAreUSure.SetActive(false);
    }

    public void YesIAm()
    {
        
        ResetPrefs();

        if (SavePlay.Instance != null)
        {
            SavePlay.Instance.SetFirstGame(true);
            SavePlay.Instance.lastCheckpoint = 0;
        }

        whereCutscene = true;
        PlayerPrefs.SetInt("WhereCutscene", 1);
        RespawnSystem.CurrentCheckpointIndex = 0;

        if (menuAreUSure != null) menuAreUSure.SetActive(false);
        if (menuPrincipal != null) menuPrincipal.SetActive(true);

        GoToDestination(3);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    
    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadFromCheckpoint()
    {
        if (SavePlay.Instance != null)
        {
            
            SavePlay.Instance.LoadData();

            isResumingGame = true;

            
            RespawnSystem.CurrentCheckpointIndex = SavePlay.Instance.lastCheckpoint;

            
            sceneDestination = SavePlay.Instance.lastScene;

            
            StartCoroutine(LoadSceneWithTransition());
        }
        else
        {
            Debug.LogError("No se puede cargar la partida porque SavePlay.Instance es nulo.");
        }
    }

    // NUEVA CORRUTINA CLONADA EXACTAMENTE DE ZONE_TRANSITION
    private IEnumerator LoadSceneWithTransition()
    {
        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        // Esperamos el tiempo del fadeout en tiempo real (ideal para menús)
        yield return new WaitForSecondsRealtime(transitionTime);

        // =========================================================================
        // TRUCO DE INMORTALIDAD: Volvemos este objeto del menú inmortal temporalmente 
        // para que Unity NO lo destruya al cambiar de escena y pueda mover al jugador.
        // =========================================================================
        DontDestroyOnLoad(gameObject);

        // Carga asíncrona exactamente igual a la de tu puerta
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneDestination);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Esperamos a que el motor físico y los objetos de la nueva escena terminen de nacer
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();

        Debug.Log($"<color=green>[MENU - SEGURO ACTIVO]</color> Escena cargada desde Reanudar. Moviendo jugador al checkpoint {RespawnSystem.CurrentCheckpointIndex}...");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // Desactivamos el Rigidbody temporalmente para que las físicas no hagan cosas raras al nacer
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
            Checkpoint puntoDeAparicion = null;

            foreach (Checkpoint cp in checkpoints)
            {
                if (cp.numeroCkeckpoint == RespawnSystem.CurrentCheckpointIndex)
                {
                    puntoDeAparicion = cp;
                    break;
                }
            }

            if (puntoDeAparicion != null)
            {
                player.transform.position = puntoDeAparicion.transform.position;
                RespawnSystem.LastCheckpointPos = puntoDeAparicion.transform.position;
                Debug.Log($"<color=green>[ÉXITO REANUDAR]</color> Jugador reposicionado en {puntoDeAparicion.transform.position}");
            }
            else if (checkpoints.Length > 0)
            {
                player.transform.position = checkpoints[0].transform.position;
                RespawnSystem.LastCheckpointPos = checkpoints[0].transform.position;
            }

            // Le devolvemos el estado físico normal tras moverlo
            yield return new WaitForFixedUpdate();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = false;
            }
        }
        else
        {
            Debug.LogError("<color=red>[ERROR MENU]</color> No se encontró al objeto con el Tag 'Player' al reanudar.");
        }

        // Una vez terminado el trabajo con éxito, destruimos este objeto de menú que ya no necesitamos
        // para evitar que se quede duplicado si vuelves al menú principal más tarde.
        Destroy(gameObject);
    }
}