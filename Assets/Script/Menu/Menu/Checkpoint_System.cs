using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint_System : MonoBehaviour
{
  
    private PlayerMovement player;

    private PlayerMovement GetPlayer()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        return player;
    }

    public void Return()
        {
            GetPlayer().ClosePauseMenu();
        }

    public void CheckpointPoint()
    {
        var playerGet = GetPlayer();
        playerGet.transform.position = RespawnSystem.LastCheckpointPos + new Vector3(2, 0, 0);
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Debug.Log("Menu cerrado");

        player.ClosePauseMenu();

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
