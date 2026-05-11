using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject player = GameObject.FindWithTag("Player");
    [SerializeField] Transform spawn = GameObject.Find("PlayerSpawn").transform;
    void Start()
    {
        

        if (player != null && spawn != null)
        {
            player.transform.position = spawn.position;
            player.transform.rotation = spawn.rotation;
        }
    }
}
