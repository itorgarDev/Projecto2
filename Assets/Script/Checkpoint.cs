using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int numeroCkeckpoint;
    private static float tiempoProteccionCarga = 0.3f;

    [Header("Visual Settings")]
    public Color activatedColor = new Color(1f, 0.85f, 0f);

    [Header("Referencias Hijas (¡Arrastra aquí en el Editor!)")]
    public Light componenteLuz;

    
    private Color originalLightColor;

    private void Awake()
    {
        if (componenteLuz == null) componenteLuz = GetComponentInChildren<Light>();

        // Guardamos los colores originales con seguridad
        
        if (componenteLuz != null)
        {
            originalLightColor = componenteLuz.color;
        }
    }

    private void Start()
    {
        int lastSavedCheckpoint = PlayerPrefs.GetInt("SavedCheckpoint", -1);

        if (lastSavedCheckpoint != numeroCkeckpoint)
        {
            ResetVisualColors();
        }
        else
        {
            ApplyActivatedColors();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.timeSinceLevelLoad < tiempoProteccionCarga)
            {
                Debug.Log($"<color=orange>[CHECKPOINT BLOQUEADO BRUTO]</color> Se evitó activar el CP {numeroCkeckpoint} en el frame de carga.");
                return;
            }

            Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();
            foreach (Checkpoint cp in allCheckpoints)
            {
                if (cp != this)
                {
                    cp.ResetVisualColors();
                }
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

            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.Heal(playerStats.maxHealth);
            }

            ApplyActivatedColors();

            if (SoundController.Instance != null)
            {
                SoundController.Instance.PlaySFX(SoundController.Instance.checkpoint);
                //SoundController.Instance.PlaySFX(SoundController.Instance.health);
            }
        }
    }

    public void ApplyActivatedColors()
    {
        if (componenteLuz != null) componenteLuz.color = activatedColor;
    }

    public void ResetVisualColors()
    {
        if (componenteLuz != null) componenteLuz.color = originalLightColor;
    }
}