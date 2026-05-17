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

    int current;

    public void Start()
    {
        imageOut.SetActive(false);
       // imageIn.SetActive(false);
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
       // sceneDestination = 0;
        Time.timeScale = 1f;
        GoToDestination();
        Debug.Log("MainMenu ejecutado desde: " + gameObject.name + " | destino: " + sceneDestination);

    }

    public void ReturnToScene()
    {
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
