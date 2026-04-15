using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_System : MonoBehaviour
{
  
        [SerializeField] private PlayerMovement player;



    public void Return()
        {
            player.ClosePauseMenu();
        }

    public void CheckpointPoint()
    {

        player.transform.position = RespawnSystem.LastCheckpointPos + new Vector3(2, 0, 0);
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("Menu cerrado");

        player.ClosePauseMenu();

    }

}
