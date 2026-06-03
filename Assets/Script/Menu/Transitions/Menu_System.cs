using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_System : MonoBehaviour
{
    public int sceneDestination;

   
    


    public float transitionTime = 1f;
    public Animator transitionFadeout;
   // public Animator transitionFadein;

    public GameObject imageOut;
    //  public GameObject imageIn;


    public GameObject menuPrincipal;
    public GameObject menuAreUSure;

    public bool whereCutscene;

    bool firstGame=false;
    
    int current;

    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs borrados");

        if (SavePlay.Instance != null)
        {
            //  Reiniciar valores internos
            SavePlay.Instance.lastScene = 0;
            SavePlay.Instance.lastCheckpoint = 0;
            SavePlay.Instance.firstGameActive = false;

            SavePlay.Instance.vida = 5;       // valor base de vida
            SavePlay.Instance.ataque = 1;     // valor base de ataque
            SavePlay.Instance.maxHealth = 5;  // si usas maxHealth en SavePlay
                                              //SavePlay.Instance.bolsaItem1 = false;
            SavePlay.Instance.masterVolume = 0.5f;
            SavePlay.Instance.musicVolume = 0.5f;
            SavePlay.Instance.sfxVolume = 0.5f;

            // Reiniciar configuración de vídeo
            SavePlay.Instance.brightness = 0.5f;
            SavePlay.Instance.quality = 2; // calidad media
            SavePlay.Instance.fullScreen = true;

            // Reiniciar ítems únicos
            SavePlay.Instance.collectedItems.Clear();
            // Guardar los nuevos valores vacios
            SavePlay.Instance.SaveData();
            Debug.Log("Datos reiniciados en memoria y guardados correctamente");
        }

        firstGame = false;
    }

    public void Start()
    {
        int originScene = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("OriginScene", originScene);

        firstGame = SavePlay.Instance.firstGameActive;
        //firstGame = true;

        whereCutscene = PlayerPrefs.GetInt("WhereCutscene", 0) == 1;
        Debug.Log("Start() firstGame = " + firstGame);
        imageOut.SetActive(false);
        // imageIn.SetActive(false);
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
            Debug.Log("StartGame pulsado");
            SavePlay.Instance.SetFirstGame(true);
            whereCutscene = true;
            SavePlay.Instance.SetFirstGame(true);
            PlayerPrefs.SetInt("WhereCutscene", whereCutscene ? 1 : 0);
            GoToDestination(3);
        }
    }

    public void AreYouSure()
    {
        menuPrincipal.SetActive(false);
        menuAreUSure.SetActive(true);
    }

    public void YesIAm()
    {
        ResetPrefs();
        SavePlay.Instance.SetFirstGame(true);
        menuAreUSure.SetActive(false);
        menuPrincipal.SetActive(true);

      //  SavePlay.Instance.SetFirstGame(true); 
       

        //       whereCutscene = true;
        PlayerPrefs.SetInt("WhereCutscene", 1);
        //SavePlay.Instance.SetFirstGame(true);

        GoToDestination(3);
    }

    public void NoImNot()
    { 
        menuAreUSure.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void GoToDestination(int valor)
    {
        sceneDestination = valor;
        

        //Time.timeScale = 0f;
        imageOut.SetActive(true);
        transitionFadeout.SetTrigger("StartFade");
        StartCoroutine(LoadSceneWithTransition());
       
    }


    IEnumerator LoadSceneWithTransition()
    {

        int originScene = PlayerPrefs.GetInt("OriginScene", -1);

        switch (sceneDestination)
        {
            case 5: // Escena 1
                if (originScene == 6)
                    RespawnSystem.CurrentCheckpointIndex = 11; // entrada desde escena 2  puente
                else if (originScene == 2)
                    RespawnSystem.CurrentCheckpointIndex = 11; // entrada desde escena 3  templo
                else
                    RespawnSystem.CurrentCheckpointIndex = 0; // por defecto
                break;
            case 6:
                if (originScene == 5)
                    RespawnSystem.CurrentCheckpointIndex = 10; // entrada desde escena 1  puente
                else if (originScene == 7)
                    RespawnSystem.CurrentCheckpointIndex = 1; // entrada desde escena 3  templo
                else
                    RespawnSystem.CurrentCheckpointIndex = 0;
                break;

            case 7: // Escena 3
                RespawnSystem.CurrentCheckpointIndex = 0;
                break;

            case 2: // Escena templo
                RespawnSystem.CurrentCheckpointIndex = 0;
                break;


        }
        // Espera en tiempo real para que funcione aunque el juego esté pausado
        yield return new WaitForSecondsRealtime(transitionTime);

        Debug.Log("Cargando escena: " + sceneDestination);
        SceneManager.LoadScene(sceneDestination);

    }

    public void MainMenu()
    {
        current = SceneManager.GetActiveScene().buildIndex;

        // Guardar correctamente la escena actual
        if (current != 3 && current != 4)
        {
            SavePlay.Instance.lastScene = current;
            Debug.Log("Escena guardada correctamente: " + current);
        }
        else
        {
            Debug.Log("Escena " + current + " no se guarda (cinemática o créditos).");
        }
        // firstGame = true;


        Time.timeScale = 1f;

        SavePlay.Instance.lastCheckpoint = RespawnSystem.CurrentCheckpointIndex;
        //SavePlay.Instance.SaveData();


        // SavePlay.Instance.lastCheckpoint = RespawnSystem.CurrentCheckpointIndex;
        Debug.Log("Checkpoint guardado correctamente: " + SavePlay.Instance.lastCheckpoint);

        // SavePlay.Instance.SetFirstGame(true);
        GoToDestination(0);

        Debug.Log("MainMenu ejecutado desde: " + gameObject.name + " | destino: " + sceneDestination);

    }


    public void ReturnToScene()
    {
        Time.timeScale = 1f;

        Debug.Log("ReturnToScene pulsado. firstGame = " + firstGame);
        if (!firstGame) return;

        int lastScene = SavePlay.Instance.lastScene;

        RespawnSystem.CurrentCheckpointIndex = SavePlay.Instance.lastCheckpoint;
        Debug.Log("ReturnToScene -> Restaurando checkpoint " + RespawnSystem.CurrentCheckpointIndex);

        GoToDestination(lastScene);

        //GoToDestination(sceneDestination);
    }

    public void GoToCutscene (int valorScene)
    {
//        whereCutscene = false;
        PlayerPrefs.SetInt("WhereCutscene", 0);
        imageOut.SetActive(true);
        GoToDestination(valorScene);
    }

    public void WhereToCutscene()
    {
        if(whereCutscene) GoToDestination(5);
        else GoToDestination(0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
           

            GoToDestination(sceneDestination);
        }
    }


}
