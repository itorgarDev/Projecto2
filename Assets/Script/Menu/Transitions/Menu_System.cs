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
        SceneManager.LoadScene(0);
    }

}
