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
        // Al tocar la puerta, si no estamos ya en transición, iniciamos el viaje
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

        PlayerPrefs.Save();

        Debug.Log($"<color=cyan>[TRANSICIÓN - PASO 2]</color> Datos guardados en disco duro. " +
                  $"Iniciando FadeOut...");

        if (imageOut != null) imageOut.SetActive(true);
        if (transitionFadeout != null) transitionFadeout.SetTrigger("StartFade");

        // Esperamos el tiempo de fundido a negro visual
        yield return new WaitForSecondsRealtime(transitionTime);

        // --- EL NUEVO MOTOR DE CARGA ULTRA SEGURO ---
        // Cargamos la escena de forma asíncrona en segundo plano
        AsyncOperation operacionCarga = SceneManager.LoadSceneAsync(sceneDestination);

        // Le prohibimos activar la escena hasta que no esté cargada al 100% en memoria
        operacionCarga.allowSceneActivation = false;

        // Bucle de espera: se mantiene aquí dentro mientras la escena pesada se procesa
        while (operacionCarga.progress < 0.9f)
        {
            yield return null;
        }

        // Ya se cargó todo en memoria. Ahora sí, permitimos que Unity la muestre en pantalla
        operacionCarga.allowSceneActivation = true;

        // Esperamos un único frame a que la jerarquía de objetos despierte en la nueva escena
        yield return new WaitForEndOfFrame();

        // --- TELETRANSPORTE DIRECTO (Inmune a errores de otros scripts) ---
        Debug.Log($"<color=green>[TRANSICIÓN - SEGURO ACTIVO]</color> Escena estabilizada. Buscando Checkpoint {checkpointDestination} de forma directa...");

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
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
                Debug.LogWarning($"<color=yellow>[AVISO]</color> No se encontró el CP {checkpointDestination}. Enviado al inicial de emergencia.");
            }
        }
        else
        {
            Debug.LogError("<color=red>[ERROR]</color> No se encontró al objeto con el Tag 'Player' tras la carga asíncrona.");
        }
    }
}