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

        // Guarda dinámicamente cualquier ítem registrado en el diccionario
        foreach (var item in collectedItems)
        {
            PlayerPrefs.SetInt("Item_" + item.Key, item.Value ? 1 : 0);
        }

        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("Brightness", brightness);
        PlayerPrefs.SetInt("Quality", (int)quality);
        PlayerPrefs.SetInt("FullScreen", fullScreen ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Datos guardados correctamente");
    }

    public void LoadData()
    {
        lastScene = PlayerPrefs.GetInt("LastScene", 0);
        lastCheckpoint = PlayerPrefs.GetInt("LastCheckpoint", 0);
        firstGameActive = PlayerPrefs.GetInt("FirstGame", 0) == 1;

        vida = PlayerPrefs.GetInt("Vida", 5);
        ataque = PlayerPrefs.GetInt("Ataque", 1);
        maxHealth = PlayerPrefs.GetInt("MaxHealth", 5);

        collectedItems.Clear(); // Limpiamos el diccionario para evitar datos basura

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        brightness = PlayerPrefs.GetFloat("Brightness", 1f);
        quality = PlayerPrefs.GetInt("Quality", 2);
        fullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
    }

    public void MarkItemCollected(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        collectedItems[id] = true;
        PlayerPrefs.SetInt("Item_" + id, 1); // Prefijo de seguridad
        PlayerPrefs.Save();
        Debug.Log($"Item único [{id}] guardado con éxito.");
    }

    public bool IsItemCollected(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;

        if (collectedItems.ContainsKey(id))
        {
            return collectedItems[id];
        }

        // Si no está en el diccionario de esta sesión, lo busca en el disco duro
        bool collected = PlayerPrefs.GetInt("Item_" + id, 0) == 1;
        collectedItems[id] = collected;
        return collected;
    }

    public void SetFirstGame(bool value)
    {
        firstGameActive = value;
        PlayerPrefs.SetInt("FirstGame", value ? 1 : 0);
        SaveData();
        Debug.Log($"SetFirstGame ejecutado: firstGameActive = {firstGameActive}");
    }
}
