using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlay : MonoBehaviour
{
    public static SavePlay Instance;
    public int lastScene;
    public int lastCheckpoint;

    public bool firstGameActive;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("LastScene", lastScene);
        PlayerPrefs.SetInt("LastCheckpoint", lastCheckpoint);
        PlayerPrefs.SetInt("FirstGame", firstGameActive ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Datos guardados correctamente");
    }

    public void LoadData()
    {
        lastScene = PlayerPrefs.GetInt("LastScene", 0);
        lastCheckpoint = PlayerPrefs.GetInt("LastCheckpoint", 0);
        firstGameActive = PlayerPrefs.GetInt("FirstGame", 0) == 1;

        Debug.Log("Datos cargados correctamente");
    }
}
