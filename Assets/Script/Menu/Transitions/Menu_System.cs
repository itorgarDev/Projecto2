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
            PlayerPrefs.SetInt("FirstGame", 1);
            GoToDestination();
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
        GoToDestination();
    }

    public void NoImNot()
    { 
        menuAreUSure.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void GoToDestination()
    {
        //Time.timeScale = 0f;
        imageOut.SetActive(true);
        transitionFadeout.SetTrigger("StartFade");
        StartCoroutine(LoadSceneWithTransition());
       
    }

    IEnumerator LoadSceneWithTransition()
    {

       
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneDestination);

    }

    public void MainMenu()
    {
        current = SceneManager.GetActiveScene().buildIndex;
        Mantener_Scene.Instance.keptScene = current;
        PlayerPrefs.SetInt("LastScene", current);
        sceneDestination = 0;
        Time.timeScale = 1f;
        GoToDestination();
        Debug.Log("MainMenu ejecutado desde: " + gameObject.name + " | destino: " + sceneDestination);

    }

    public void ReturnToScene()
    {
        Debug.Log("ReturnToScene pulsado. firstGame = " + firstGame);
        if (!firstGame) return;
        int lastScene = PlayerPrefs.GetInt("LastScene", 0);
        sceneDestination=lastScene;
        GoToDestination();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (CompareTag("templo")&&other.CompareTag("Player"))
        {
            GoToDestination();
        }
    }

}
