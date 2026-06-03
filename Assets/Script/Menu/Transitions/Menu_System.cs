using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_System : MonoBehaviour
{
    public int sceneDestination;


    public static bool returningToScene = false;
    public static bool isResumingGame = false;


    public static bool comingFromCheckpointButton = false;


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
            RespawnSystem.CurrentCheckpointIndex = 0;
            SavePlay.Instance.lastCheckpoint = 0;
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
        RespawnSystem.CurrentCheckpointIndex = 0;
        SavePlay.Instance.lastCheckpoint = 0;
        GoToDestination(3);
    }

    public void NoImNot()
    { 
        menuAreUSure.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void LoadFromCheckpoint()
    {
        isResumingGame = true;
        RespawnSystem.CurrentCheckpointIndex = SavePlay.Instance.lastCheckpoint;
        GoToDestination(SavePlay.Instance.lastScene);
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
        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(sceneDestination);
        isResumingGame = false;
    }


    /*private void SetCheckpointForScene(int originScene)
    {
        // Caso especial: StartGame  escena 5 checkpoint 1
        if (sceneDestination == 5 && SavePlay.Instance.lastCheckpoint == 0)
        {
            RespawnSystem.CurrentCheckpointIndex = 1;
            return;
        }

        switch (sceneDestination)
        {
            case 5:
                if (originScene == 6)
                    RespawnSystem.CurrentCheckpointIndex = 11;
                else if (originScene == 2)
                    RespawnSystem.CurrentCheckpointIndex = 11;
                else
                    RespawnSystem.CurrentCheckpointIndex = 0;
                break;

            case 6:
                if (originScene == 5)
                    RespawnSystem.CurrentCheckpointIndex = 11;
                else if (originScene == 7)
                    RespawnSystem.CurrentCheckpointIndex = 1;
                else
                    RespawnSystem.CurrentCheckpointIndex = 0;
                break;

            case 7:
                RespawnSystem.CurrentCheckpointIndex = 0;
                break;

            case 2:
                RespawnSystem.CurrentCheckpointIndex = 0;
                break;
        }
    }*/



    public void MainMenu()
    {
        current = SceneManager.GetActiveScene().buildIndex;

        if (current != 3 && current != 4)
            SavePlay.Instance.lastScene = current;

        Time.timeScale = 1f;

        if (!comingFromCheckpointButton)
            SavePlay.Instance.lastCheckpoint = RespawnSystem.CurrentCheckpointIndex;

        comingFromCheckpointButton = false;

        GoToDestination(0);
    }



    public void ReturnToScene()
    {
        Time.timeScale = 1f;

        Debug.Log("ReturnToScene pulsado. firstGame = " + firstGame);
        if (!firstGame) return;
        //returningToScene = false;
        isResumingGame = true;
        int lastScene = SavePlay.Instance.lastScene;

        RespawnSystem.CurrentCheckpointIndex = SavePlay.Instance.lastCheckpoint;
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
          //  if (!isResumingGame && other.CompareTag("CheckpointTrigger"))
                //SetCheckpointForScene(sceneDestination);

            GoToDestination(sceneDestination);
        }
    }


}
