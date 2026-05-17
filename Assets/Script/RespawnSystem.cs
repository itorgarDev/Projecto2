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

        // Si no hay checkpoint guardado, usa la posición inicial del jugador
        LastCheckpointPos = player.position;
    }

    public static Vector3 GetCheckpointPosition()
    {
        if (Checkpoints.ContainsKey(CurrentCheckpointIndex))
            return Checkpoints[CurrentCheckpointIndex];

        return LastCheckpointPos; // fallback
    }
}
