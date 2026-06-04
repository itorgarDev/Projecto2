using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem Instance;

    public static Vector3 LastCheckpointPos;
    public static int CurrentCheckpointIndex = 0;

  //  public static Dictionary<int, Vector3> Checkpoints = new Dictionary<int, Vector3>();

    public Transform player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //  Buscar al jugador
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        //  Buscar todos los checkpoints
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

        if (checkpoints.Length == 0)
        {
            Debug.LogWarning("RespawnSystem: No hay checkpoints en esta escena.");
            return;
        }

        //  Validar índice
        if (CurrentCheckpointIndex < 0 || CurrentCheckpointIndex >= checkpoints.Length)
        {
            Debug.LogWarning("RespawnSystem: Índice inválido, usando 0.");
            CurrentCheckpointIndex = 0;
        }

        //  Mover al jugador al checkpoint guardado
        player.position = checkpoints[CurrentCheckpointIndex].transform.position;
        Debug.Log($"RespawnSystem: Jugador colocado en checkpoint {CurrentCheckpointIndex}");
    }

    void Start()
    {
       


       // LoadCheckpointData();
       // player.position = LastCheckpointPos;
        Debug.Log("RespawnSystem Start -> LastCheckpointPos = " + LastCheckpointPos);
    }

  /*  private void LoadCheckpointData()
    {
        if (PlayerPrefs.HasKey("CP_X"))
        {
            float x = PlayerPrefs.GetFloat("CP_X");
            float y = PlayerPrefs.GetFloat("CP_Y");
            float z = PlayerPrefs.GetFloat("CP_Z");
            LastCheckpointPos = new Vector3(x, y, z);
        }
        else
        {
            LastCheckpointPos = player.position;
        }
    }
  */

    public static Vector3 GetCheckpointPosition()
    {
        return LastCheckpointPos;
    }
}
