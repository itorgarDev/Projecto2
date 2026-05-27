using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            Debug.Log("Escena de menú detectada — no se actualiza lastScene.");
            return;
        }
        // Actualiza automáticamente la escena actual
        lastScene = scene.buildIndex;
        SaveData();
        Debug.Log($"Escena actual guardada automáticamente: {lastScene}");
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

    public void SetFirstGame(bool value)
    {
        firstGameActive = value;
        PlayerPrefs.SetInt("FirstGame", value ? 1 : 0);
        SaveData();
        Debug.Log($"SetFirstGame ejecutado: firstGameActive = {firstGameActive}");
    }
}
