using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint_System : MonoBehaviour
{
  
    private PlayerMovement player;
  //  public int keptScene;

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

        Vector3 pos = RespawnSystem.GetCheckpointPosition();

        playerGet.transform.position = pos + new Vector3(2, 0, 0);

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        player.ClosePauseMenu();
    }


    /*   public void MainMenu()
       {
           keptScene=SceneManager.GetActiveScene().buildIndex;
           PlayerPrefs.SetInt("LastScene", keptScene);
           SceneManager.LoadScene(0);
       }

       public void ReturnToScene(int currentScene)
       {
           int lastScene = PlayerPrefs.GetInt("LastScene", 0);

           SceneManager.LoadScene(currentScene);
       }
    */

}