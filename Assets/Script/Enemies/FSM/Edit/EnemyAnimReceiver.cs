using UnityEngine;

public class EnemyAnimReceiver : MonoBehaviour
{
    private EnemyFSMManager fsm;

    private void Awake()
    {
        // Busca el FSM en el padre
        fsm = GetComponentInParent<EnemyFSMManager>();
    }

    // Esta función la llama el Animation Event
    public void EnemyHit()
    {
        fsm.weaponCollider.enabled = true;
        fsm.Invoke(nameof(fsm.DisableCollider), fsm.hitboxDuration);
    }
}
