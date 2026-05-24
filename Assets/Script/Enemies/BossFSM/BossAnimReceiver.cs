using UnityEngine;

public class BossAnimReceiver : MonoBehaviour
{
    private BossEvokerFSMManager boss;

    private void Awake()
    {
        boss = GetComponentInParent<BossEvokerFSMManager>();
    }

    public void BossHit()
    {
        boss.weaponCollider.enabled = true;
        boss.Invoke(nameof(boss.DisableCollider), boss.hitboxDuration);
    }
}
