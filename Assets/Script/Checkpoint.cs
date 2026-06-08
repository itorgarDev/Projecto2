using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int numeroCkeckpoint;
    private static float tiempoProteccionCarga = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ==========================================
            // CERROJO A LO BRUTO: Si la escena lleva menos de 0.3 segundos cargada,
            // ignoramos por completo el trigger para evitar activaciones fantasma al nacer.
            // ==========================================
            if (Time.timeSinceLevelLoad < tiempoProteccionCarga)
            {
                Debug.Log($"<color=orange>[CHECKPOINT BLOQUEADO BRUTO]</color> Se evitó activar el CP {numeroCkeckpoint} en el frame de carga.");
                return;
            }

            RespawnSystem.LastCheckpointPos = transform.position;

            PlayerPrefs.SetInt("SavedCheckpoint", numeroCkeckpoint);
            PlayerPrefs.SetFloat("CP_X", transform.position.x);
            PlayerPrefs.SetFloat("CP_Y", transform.position.y);
            PlayerPrefs.SetFloat("CP_Z", transform.position.z);

            if (FindObjectOfType<SavePlay>() != null)
            {
                FindObjectOfType<SavePlay>().lastCheckpoint = numeroCkeckpoint;
                FindObjectOfType<SavePlay>().SaveData();
            }

            Debug.Log("Checkpoint " + numeroCkeckpoint + " activado. Pos = " + transform.position);

            if (SoundController.Instance != null)
            {
                SoundController.Instance.PlaySFX(SoundController.Instance.checkpoint);
            }
        }
    }
}