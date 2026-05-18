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
          //  RespawnSystem.CurrentCheckpointIndex = numeroCkeckpoint;

            PlayerPrefs.SetInt("SavedCheckpoint", numeroCkeckpoint);
            PlayerPrefs.SetFloat("CP_X", transform.position.x);
            PlayerPrefs.SetFloat("CP_Y", transform.position.y);
            PlayerPrefs.SetFloat("CP_Z", transform.position.z);
            PlayerPrefs.Save();

            Debug.Log("Checkpoint " + numeroCkeckpoint + " activado. Pos = " + transform.position);
        }
    }
    
}

