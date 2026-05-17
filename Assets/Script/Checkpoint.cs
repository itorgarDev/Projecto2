using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int numeroCkeckpoint;

    private void Awake()
    {

        RespawnSystem.Checkpoints[numeroCkeckpoint] = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnSystem.LastCheckpointPos = transform.position;
            RespawnSystem.CurrentCheckpointIndex = numeroCkeckpoint;

            PlayerPrefs.SetInt("SavedCheckpoint", numeroCkeckpoint);
            PlayerPrefs.Save();
        }
    }
}
