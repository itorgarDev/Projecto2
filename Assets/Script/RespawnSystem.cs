using UnityEngine;
using System.Collections.Generic;

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

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        PlayerPrefs.SetFloat("CP_X", player.position.x);
        PlayerPrefs.SetFloat("CP_Y", player.position.y);
        PlayerPrefs.SetFloat("CP_Z", player.position.z);
        PlayerPrefs.Save();

        LastCheckpointPos = player.position;
        player.position = LastCheckpointPos;
        Debug.Log("RespawnSystem -> Player reposicionado en " + LastCheckpointPos);
    }

    void Start()
    {
        LoadCheckpointData();
        player.position = LastCheckpointPos;
        Debug.Log("RespawnSystem Start -> LastCheckpointPos = " + LastCheckpointPos);
    }

    private void LoadCheckpointData()
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

    public static Vector3 GetCheckpointPosition()
    {
        return LastCheckpointPos;
    }
}
