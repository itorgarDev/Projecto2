using UnityEngine;

public class BossAttack : TemplateStateMachine
{
    private BossEvokerFSMManager boss;
    
    public BossAttack(BossEvokerFSMManager fsm) : base("BossAttack", fsm)
    {
        boss = fsm;
    }

    public override void Enter()
    {
        
        
        boss.animatorBoss.SetTrigger("Attack");
        SoundController.Instance.PlaySFX(SoundController.Instance.xMelee);
    }

    public override void UpdateLogic()
    {
        if (Time.timeScale == 0f) return;
        // BLOQUEAMOS LA LÓGICA DEL ENEMIGO BASE
    }

    public override void Updatephysics()
    {
        if (Time.timeScale == 0f) return;

        
        // BLOQUEAMOS TAMBIÉN LA FÍSICA DEL ENEMIGO BASE

        Vector3 dir = (boss.player.position - boss.transform.position).normalized;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
            boss.transform.rotation = Quaternion.LookRotation(dir);
    }


    public void BossHit()
    {
        boss.weaponCollider.enabled = true;
        boss.Invoke(nameof(boss.DisableCollider), boss.hitboxDuration);
    }

}
