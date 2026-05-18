using System.Collections.Generic;
using UnityEngine;
/*public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem Instance;
    public static Vector3 LastCheckpointPos;
    public Transform player;
    public static int CurrentCheckpointIndex = 0;

    void Awake()
    {
        Instance = this;
        LastCheckpointPos = player.position; // posición inicial
    }

    public void Respawn()
    {
        player.position = LastCheckpointPos;
    }*/

using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem Instance;

    public static Vector3 LastCheckpointPos;
    public static int CurrentCheckpointIndex = 0;

    public static Dictionary<int, Vector3> Checkpoints = new Dictionary<int, Vector3>();

    public Transform player;

    void Awake()
    {
        Instance = this;

        // Si hay posición guardada, usarla
        if (PlayerPrefs.HasKey("CP_X"))
        {
            float x = PlayerPrefs.GetFloat("CP_X");
            float y = PlayerPrefs.GetFloat("CP_Y");
            float z = PlayerPrefs.GetFloat("CP_Z");

            LastCheckpointPos = new Vector3(x, y, z);
        }
        else
        {
            // Primera vez: posición inicial del jugador
            LastCheckpointPos = player.position;
        }

        player.position = LastCheckpointPos;


        Debug.Log("RespawnSystem Awake -> LastCheckpointPos = " + LastCheckpointPos);
    }

    public static Vector3 GetCheckpointPosition()
    {
        return LastCheckpointPos;
    }
} 

    /*public static Vector3 GetCheckpointPosition()
    {
        // Si el diccionario tiene el checkpoint, úsalo
        if (Checkpoints.ContainsKey(CurrentCheckpointIndex))
            return Checkpoints[CurrentCheckpointIndex];

        // Si no, usa la posición guardada en PlayerPrefs
        if (PlayerPrefs.HasKey("CP_X"))
        {
            float x = PlayerPrefs.GetFloat("CP_X");
            float y = PlayerPrefs.GetFloat("CP_Y");
            float z = PlayerPrefs.GetFloat("CP_Z");
            return new Vector3(x, y, z);
        }

        // Fallback final
        return LastCheckpointPos;
    }*/
