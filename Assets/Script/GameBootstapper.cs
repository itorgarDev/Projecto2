using UnityEngine;

public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private GameObject pauseMenuPrefab;

    void Awake()
    {
        // PLAYER
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player == null)
        {
            GameObject p = Instantiate(playerPrefab);
            player = p.GetComponent<PlayerMovement>();
        }

        // CÁMARA
        SimpleCameraFollow cam = FindObjectOfType<SimpleCameraFollow>();
        if (cam == null)
        {
            GameObject c = Instantiate(cameraPrefab);
            cam = c.GetComponent<SimpleCameraFollow>();
        }

        // Conectar cámara → player
        cam.SetTarget(player.transform);

        // MENÚ
        if (FindObjectOfType<PauseMenuBreaker>() == null)
        {
            Instantiate(pauseMenuPrefab);
        }
    }
}
