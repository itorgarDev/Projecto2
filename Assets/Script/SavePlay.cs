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
        public bool bolsaItem1;

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
            maxHealth = PlayerPrefs.GetInt("MaxHealth", 3);
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

        PlayerPrefs.SetInt("MaxHealth", maxHealth);
        PlayerPrefs.Save();
            Debug.Log("Datos guardados correctamente");
        }

        public void LoadData()
        {
            //valores escenas
            lastScene = PlayerPrefs.GetInt("LastScene", 0);
            lastCheckpoint = PlayerPrefs.GetInt("LastCheckpoint", 0);
            firstGameActive = PlayerPrefs.GetInt("FirstGame", 0) == 1;

            //valores Barun
            vida = PlayerPrefs.GetInt("Vida", 5);      // valor por defecto
            maxHealth = PlayerPrefs.GetInt("maxHealth", 5);      // valor por defecto
            ataque = PlayerPrefs.GetInt("Ataque", 1);  // valor por defecto
            bolsaItem1 = PlayerPrefs.GetInt("BolsaItem1", 0) == 1;

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
