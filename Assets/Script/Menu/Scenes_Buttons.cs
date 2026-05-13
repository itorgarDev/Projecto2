using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scenes_Buttons : MonoBehaviour
{
    int current;
   

    public void MainMenu()
    {
        current = SceneManager.GetActiveScene().buildIndex;
        Mantener_Scene.Instance.keptScene = current;
        PlayerPrefs.SetInt("LastScene", current);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ReturnToScene()
    {
        int lastScene = PlayerPrefs.GetInt("LastScene", 0);
        SceneManager.LoadScene(lastScene);
    }

}
