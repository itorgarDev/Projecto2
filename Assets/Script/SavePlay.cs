using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlay : MonoBehaviour
{
    public int lastScene;
    public int lastCheckpoint;

    public bool firstGameActive;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadData();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("sceneEnd",lastScene);
        PlayerPrefs.SetInt("checkyCheck", lastCheckpoint);
        PlayerPrefs.SetInt("firstGame", firstGameActive ? 0 : 1);
        PlayerPrefs.Save();
        Debug.Log("Datos guardados");
    }

    public void LoadData()
    {
        lastScene=PlayerPrefs.GetInt("sceneEnd",lastScene) ;
        lastCheckpoint=PlayerPrefs.GetInt("checkyCheck",lastCheckpoint) ;
        firstGameActive=PlayerPrefs.GetInt("firstGame",0)==1;

        Debug.Log("Datos guardados");
    }
}
