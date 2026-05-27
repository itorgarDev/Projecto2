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
        Instance = this;
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
