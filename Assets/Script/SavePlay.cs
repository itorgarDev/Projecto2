    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class SavePlay : MonoBehaviour
    {
        public static SavePlay Instance;

        [Space(10)]
        [Header("Logica cambio de escenas")]
        public int lastScene;
        public int lastCheckpoint;
        public bool firstGameActive;

        [Space(10)]
        [Header("Logica Barun")]
        public int ataque;
        public int vida;
        public int maxHealth;
    //        public bool bolsaItem1;
        // Diccionario de ítems únicos recogidos
        public Dictionary<string, bool> collectedItems = new Dictionary<string, bool>();

    [Space(10)]
        [Header("Logica menús")]
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public float brightness;
        public float quality;
        public bool fullScreen;


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

            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 3 || currentScene == 4)
            {
                Debug.Log($"Entrando en escena {currentScene} — se mantiene firstGameActive = {firstGameActive}");
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            //maxHealth = PlayerPrefs.GetInt("MaxHealth", 3);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            int index=scene.buildIndex;    

            if (scene.buildIndex == 0 || scene.buildIndex == 3 || scene.buildIndex == 4)
            {
                Debug.Log("Escena de menú detectada — no se actualiza lastScene.");
                return;
            }
            // Actualiza automáticamente la escena actual
            lastScene = index;
            SaveData();
            Debug.Log($"Escena actual guardada automáticamente: {lastScene}");
        }

         public void SaveData()
        {
        PlayerPrefs.SetInt("LastScene", lastScene);
        PlayerPrefs.SetInt("LastCheckpoint", lastCheckpoint);
        PlayerPrefs.SetInt("FirstGame", firstGameActive ? 1 : 0);

        PlayerPrefs.SetInt("Vida", vida);
        PlayerPrefs.SetInt("Ataque", ataque);
        PlayerPrefs.SetInt("MaxHealth", maxHealth);
        foreach (var item in collectedItems)
        {
            PlayerPrefs.SetInt(item.Key, item.Value ? 1 : 0);
        }

        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);

        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.SetInt("Quality", (int)quality);
        PlayerPrefs.SetInt("FullScreen", fullScreen ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Datos guardados correctamente");

        PlayerPrefs.Save();
    //    Debug.Log("Datos guardados correctamente — BolsaItem1 = " + bolsaItem1);
        }

        public void LoadData()
        {
        lastScene = PlayerPrefs.GetInt("LastScene", 0);
        lastCheckpoint = PlayerPrefs.GetInt("LastCheckpoint", 0);
        firstGameActive = PlayerPrefs.GetInt("FirstGame", 0) == 1;

        vida = PlayerPrefs.GetInt("Vida", 5);
        ataque = PlayerPrefs.GetInt("Ataque", 1);
        maxHealth = PlayerPrefs.GetInt("MaxHealth", 5);
        collectedItems.Clear();

        string[] itemKeys = { "BolsaItem1", "BolSaltem1", "chalecoDivino" }; // añade aquí tus IDs
        foreach (string key in itemKeys)
        {
            collectedItems[key] = PlayerPrefs.GetInt(key, 0) == 1;
        }

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        brightness = PlayerPrefs.GetFloat("Brightness", 1f);
        quality = PlayerPrefs.GetInt("Quality", 2);
        fullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;


        //     Debug.Log("Datos cargados correctamente — BolsaItem1 = " + bolsaItem1);
    }


    public void SetFirstGame(bool value)
        {
            firstGameActive = value;
            PlayerPrefs.SetInt("FirstGame", value ? 1 : 0);
            SaveData();
            Debug.Log($"SetFirstGame ejecutado: firstGameActive = {firstGameActive}");
        }

    public void MarkItemCollected(string id)
    {
        collectedItems[id] = true;
        PlayerPrefs.SetInt(id, 1);
        PlayerPrefs.Save();
        Debug.Log($"Item {id} marcado como recogido");
    }

    public bool IsItemCollected(string id)
    {
        return PlayerPrefs.GetInt(id, 0) == 1;
    }
}
