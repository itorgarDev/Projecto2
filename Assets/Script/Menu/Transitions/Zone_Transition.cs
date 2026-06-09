using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneTransition : MonoBehaviour
{
    [Header("Escena de Destino")]
    public int sceneDestination;

    [Header("Checkpoint de Destino")]
    public int checkpointDestination;

    [Header("Configuración Visual")]
    public float transitionTime = 1f;
    public GameObject imageOut;           
    public Animator transitionFadeout;    

    private bool travelling = false;

    private void Start()
    {
        if (imageOut != null)
            imageOut.SetActive(false);
    }

    //Metodo para los objetos "Door", para los cambios de escena
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !travelling)
        {
            StartCoroutine(LoadSceneRoutine());
        }
    }

    //Corrutina para ejecutar el cambio de escenas
    private IEnumerator LoadSceneRoutine()
    {
        travelling = true;

        RespawnSystem.CurrentCheckpointIndex = checkpointDestination;

        if (SavePlay.Instance != null)
        {
            SavePlay.Instance.lastCheckpoint = checkpointDestination;
            SavePlay.Instance.lastScene = sceneDestination;
            SavePlay.Instance.SaveData();
        }

        // Se activa la animacion de la transicion y se espera el tiempo indicado con "transitionTime"

        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        yield return new WaitForSeconds(transitionTime);


        // No se destruye el objeto puerta al cambiar de escena

        DontDestroyOnLoad(gameObject);

        //Se hace un cambio de escena asíncrona para evitar que la pantalla se congele y para que se ejecuten las animaciones de transicion

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneDestination);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            //Se desactiva el Rigidbody para que no de problemas a la hora de cambiar de escena

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();
            Checkpoint puntoDeAparicion = null;

            foreach (Checkpoint cp in checkpoints)
            {
                if (cp.numeroCkeckpoint == checkpointDestination)
                {
                    puntoDeAparicion = cp;
                    break;
                }
            }

            if (puntoDeAparicion != null)
            {
                player.transform.position = puntoDeAparicion.transform.position;
                RespawnSystem.LastCheckpointPos = puntoDeAparicion.transform.position;
                Debug.Log($"Moviendo al jugador al checkpoint {checkpointDestination} en {puntoDeAparicion.transform.position}");
            }
            else if (checkpoints.Length > 0)
            {
                player.transform.position = checkpoints[0].transform.position;
                RespawnSystem.LastCheckpointPos = checkpoints[0].transform.position;
            }

            // Se vuelve a activar el Rigidbidy del jugador
            yield return new WaitForFixedUpdate();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = false;
            }
        }
        travelling = false;

        // Se destruye la puerta
        Destroy(gameObject);
    }

}