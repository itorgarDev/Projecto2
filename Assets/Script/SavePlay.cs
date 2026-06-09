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
        public Dictionary<string, bool> collectedItems = new Dictionary<string, bool>();

        [Space(10)]
        [Header("Logica menús")]
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;
        public float brightness;
        public float quality;
        public bool fullScreen;

        //Antes de Start, se cargan los datos y se encarga de que el gameObject del script se instancie
        //Se instancia una variable con el indice de la escena actual

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
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        //Metodo al terminar de cargar una escena
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
        //Se inicializa una variable que recoge el indice de la escena
        //Para las escenas de cinematicas no se actualiza el lastScene para que no de problemas a la hora de reanudar escena desde el menu
            int index=scene.buildIndex;    

            if (scene.buildIndex == 0 || scene.buildIndex == 3 || scene.buildIndex == 4)
            {
                Debug.Log("Escena de menú detectada — no se actualiza lastScene.");
                return;
            }
        // Para las escenas in-game, nos aseguramos de que el valor del lastScene se actualice con el valor del indice y se llama al metodo SaveData

        if (index == 5 || index == 6 || index == 7)
        {
            lastScene = index;
            SaveData();

        }

        }


    private void MoverJugadorAlEntrar(Scene scene, LoadSceneMode mode)
    {
       
        SceneManager.sceneLoaded -= MoverJugadorAlEntrar;

        // Al entrar en la nueva escena, inicializa una variable GameObject y busca al jugador para que sea su valor
        // También crea un array de clase Checkpoint para analizar y meter a todos los checkpoints
        //Aquí llama a cada checkpoint del array y si el indice coincide con el valor currentCheckpointSystem, 

        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
        Checkpoint puntoDeAparicion = null;

        foreach (Checkpoint cp in checkpoints)
        {
            if (cp.numeroCkeckpoint == RespawnSystem.CurrentCheckpointIndex)
            {
                puntoDeAparicion = cp;
                break;
            }
        }

        //Si el numero coincide con un indice, el jugador se transforma a la posicion. Si no, devuelve al jugador al checkpoint con el indice 0 (valor inicial)
        if (puntoDeAparicion != null)
        {    

            player.transform.position = puntoDeAparicion.transform.position;
            RespawnSystem.LastCheckpointPos = puntoDeAparicion.transform.position;
        }
        else if (checkpoints.Length > 0)
        {
            
            Debug.LogWarning($"<color=red>[FALLO CRÍTICO]</color> No existe ningún script Checkpoint con el número {RespawnSystem.CurrentCheckpointIndex} " +
                             $"en esta escena. El jugador será enviado al index 0 que está en: {checkpoints[0].transform.position}");

            player.transform.position = checkpoints[0].transform.position;
            RespawnSystem.LastCheckpointPos = checkpoints[0].transform.position;
        }
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

        collectedItems.Clear();

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
