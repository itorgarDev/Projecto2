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
    public GameObject imageOut;             // Tu panel negro de UI
    public Animator transitionFadeout;     // Tu Animator con el trigger "StartFade"

    private bool viajando = false;

    private void Start()
    {
        if (imageOut != null)
            imageOut.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !viajando)
        {
            StartCoroutine(LoadSceneRoutine());
        }
    }

    private IEnumerator LoadSceneRoutine()
    {
        viajando = true;

        Debug.Log($"<color=cyan>[TRANSICIÓN - PASO 1]</color> Jugador tocó la puerta. Configurando destino... " +
                  $"Escena objetivo: {sceneDestination}, Checkpoint objetivo: {checkpointDestination}");

        RespawnSystem.CurrentCheckpointIndex = checkpointDestination;

        if (SavePlay.Instance != null)
        {
            SavePlay.Instance.lastCheckpoint = checkpointDestination;
            SavePlay.Instance.lastScene = sceneDestination;
            SavePlay.Instance.SaveData();
        }

        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        yield return new WaitForSeconds(transitionTime);

        // ==========================================
        // SOLUCIÓN A LO BRUTO: Volvemos la puerta inmortal temporalmente
        // para que Unity NO mate este script al cambiar de escena.
        // ==========================================
        DontDestroyOnLoad(gameObject);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneDestination);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Esperamos a que el motor físico y los objetos de la nueva escena terminen de nacer
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();

        Debug.Log($"<color=green>[TRANSICIÓN - SEGURO ACTIVO]</color> Escena estabilizada. Moviendo jugador al checkpoint {checkpointDestination}...");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // Desactivamos el Rigidbody temporalmente para que las físicas no lo succionen a la cama
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
                Debug.Log($"<color=green>[ÉXITO TOTAL]</color> Moviendo jugador al checkpoint {checkpointDestination} en {puntoDeAparicion.transform.position}");
            }
            else if (checkpoints.Length > 0)
            {
                player.transform.position = checkpoints[0].transform.position;
                RespawnSystem.LastCheckpointPos = checkpoints[0].transform.position;
            }

            // Le devolvemos el estado físico normal tras moverlo
            yield return new WaitForFixedUpdate();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = false;
            }
        }
        else
        {
            Debug.LogError("<color=red>[ERROR]</color> No se encontró al objeto con el Tag 'Player' tras la carga asíncrona.");
        }

        viajando = false;

        // Una vez terminado el trabajo con éxito, destruimos la puerta vieja de la escena anterior para no dejar basura
        Destroy(gameObject);
    }

}