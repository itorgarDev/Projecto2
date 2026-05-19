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
    }

    public void Start()
    {
        firstGame = PlayerPrefs.GetInt("FirstGame", 0) == 1;
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
            Debug.Log("StartGame pulsado");
            firstGame = true;
            whereCutscene = true;
            PlayerPrefs.SetInt("FirstGame",1);
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

        menuAreUSure.SetActive(false);
        menuPrincipal.SetActive(true);
 //       whereCutscene = true;
        PlayerPrefs.SetInt("WhereCutscene", 1);
        PlayerPrefs.SetInt("FirstGame", 1);

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

       
        yield return new WaitForSeconds(transitionTime);
        Debug.Log("Cargando escena: " + sceneDestination);
        SceneManager.LoadScene(sceneDestination);

    }

    public void MainMenu()
    {
        current = SceneManager.GetActiveScene().buildIndex;

        // Guardar correctamente la escena actual
        SavePlay.Instance.lastScene = current;
        SavePlay.Instance.SaveData();

        sceneDestination = 0;
        Time.timeScale = 1f;
        GoToDestination(0);

        Debug.Log("MainMenu ejecutado desde: " + gameObject.name + " | destino: " + sceneDestination);

    }


    public void ReturnToScene()
    {
        Debug.Log("ReturnToScene pulsado. firstGame = " + firstGame);
        if (!firstGame) return;
        int lastScene = PlayerPrefs.GetInt("LastScene", 0);
        sceneDestination=lastScene;

        GoToDestination(sceneDestination);
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
        if(whereCutscene) GoToDestination(1);
        else GoToDestination(0);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (CompareTag("templo")&&other.CompareTag("Player"))
        {
            GoToDestination(2);
        }
    }

}
