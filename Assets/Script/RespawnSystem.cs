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

        Checkpoint correctCheckpoint = null;
        foreach (Checkpoint cp in checkpoints)
        {
            if (cp.numeroCkeckpoint == CurrentCheckpointIndex)
            {
                correctCheckpoint = cp;
                break; // Lo encontramos, dejamos de buscar
            }
        }

        // 4. Mover al jugador si se encontró, o usar el primero disponible por seguridad
        if (correctCheckpoint != null)
        {
            player.position = correctCheckpoint.transform.position;
            Debug.Log($"RespawnSystem: Jugador colocado con éxito en checkpoint número {CurrentCheckpointIndex}");
        }
        else
        {
            // Si por alguna razón el índice guardado no existe en esta escena (ej: vas a otra escena), 
            // lo movemos al primer checkpoint que encuentre Unity por descarte.
            player.position = checkpoints[0].transform.position;
            Debug.LogWarning($"RespawnSystem: No se encontró un checkpoint con el número {CurrentCheckpointIndex}. Usando respawn por defecto.");
        }
        //  Mover al jugador al checkpoint guardado
       // player.position = checkpoints[CurrentCheckpointIndex].transform.position;
        //Debug.Log($"RespawnSystem: Jugador colocado en checkpoint {CurrentCheckpointIndex}");
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
