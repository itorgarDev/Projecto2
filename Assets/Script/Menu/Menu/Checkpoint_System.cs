using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint_System : MonoBehaviour
{
    private PlayerMovement player;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reasigna el jugador al cargar una nueva escena
        player = FindObjectOfType<PlayerMovement>();
        if (player != null)
            Debug.Log($"PlayerMovement re-referenciado tras cargar escena: {scene.name}");
        else
            Debug.LogWarning($"No se encontró PlayerMovement en la escena: {scene.name}");
    }

    private PlayerMovement GetPlayer()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        if (player == null)
        {
            Debug.LogWarning("No se encontró PlayerMovement al intentar usar Checkpoint_System.");
        }

        return player;
    }

    public void Return()
    {
        var p = GetPlayer();
        if (p != null)
            p.ClosePauseMenu();
    }

    public void CheckpointPoint()
    {
        var p = GetPlayer();
        if (p == null) return;

        Vector3 pos = RespawnSystem.GetCheckpointPosition();
        p.transform.position = pos + new Vector3(2, 0, 0);

        Rigidbody rb = p.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        p.ClosePauseMenu();
    }
}
