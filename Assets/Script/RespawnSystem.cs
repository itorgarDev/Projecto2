using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem Instance;

    public static Vector3 LastCheckpointPos;
    public static int CurrentCheckpointIndex = 0;

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

    // Este método lo puedes llamar desde el script de daño/muerte del jugador 
    // cuando su vida llegue a 0 para devolverlo al último checkpoint activado.
    public void RespawnPlayer()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        if (player != null)
        {
            player.position = LastCheckpointPos;
            Debug.Log("[RespawnSystem] Jugador revivido en el último checkpoint físico.");
        }
    }

    public static Vector3 GetCheckpointPosition()
    {
        return LastCheckpointPos;
    }
}