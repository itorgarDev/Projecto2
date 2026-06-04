using UnityEngine;

public class BossArenaDetector : MonoBehaviour
{
    [Header("Configuración")]
    public string bossDisplayName = "Nombre del Jefe";
    public float evokerMaxHealth = 15f;

    private PhoenixFSM fenix;
    private EnemyFSMManager evoker;

    void Awake()
    {
        // Busca los componentes en el cuerpo principal del jefe (el padre)
        fenix = GetComponentInParent<PhoenixFSM>();
        evoker = GetComponentInParent<EnemyFSMManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && HUDController.Instance != null)
        {
            float maxHp = fenix != null ? fenix.maxHealth : evokerMaxHealth;
            // Le dice al HUD: "Empieza a mirar la vida de este jefe"
            HUDController.Instance.TrackBoss(fenix, evoker, bossDisplayName, maxHp);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && HUDController.Instance != null)
        {
            // Si te vas lejos, apaga la interfaz
            HUDController.Instance.UntrackBoss();
        }
    }
}