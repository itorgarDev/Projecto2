using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem Instance;
    public static Vector3 LastCheckpointPos;
    public Transform player;

    void Awake()
    {
        Instance = this;
        LastCheckpointPos = player.position; // posición inicial
    }

    public void Respawn()
    {
        player.position = LastCheckpointPos;
    }
}
