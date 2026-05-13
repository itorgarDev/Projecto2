public class BossChase : Chase
{
    private BossEvokerFSMManager boss;

    public BossChase(BossEvokerFSMManager fsm) : base(fsm)
    {
        boss = fsm;
    }

    public override void Enter()
    {
        base.Enter();

        boss.animatorBoss.SetBool("isIdle", false);
        boss.animatorBoss.SetBool("isChasing", true);
    }
}

