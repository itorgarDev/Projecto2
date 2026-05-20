using UnityEngine;

public class ShootState : TemplateStateMachine
{
    PhoenixFSM phoenix;
    float shootTimer;
    float shootDuration = 1f;

    private PhoenixFuzzyController fuzzyBrain;

    public ShootState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ENTER Shoot");
        shootTimer = 0f;

        if (fuzzyBrain == null && phoenix != null)
        {
            fuzzyBrain = phoenix.GetComponent<PhoenixFuzzyController>();
        }

        if (phoenix != null && phoenix.firePoint != null)
        {
            if (phoenix.target != null)
            {
                Vector3 targetBodyPosition = phoenix.target.position + new Vector3(0f, 1.1f, 0f);
                Vector3 targetDirection = targetBodyPosition - phoenix.firePoint.position;
                phoenix.firePoint.rotation = Quaternion.LookRotation(targetDirection);
            }

            // le pasamos la distancia y estamina actuales de la fsm para que el fuzzy decida el patron
            Projectile.ShootPattern chosenPattern = Projectile.ShootPattern.SingleBullet;
            if (fuzzyBrain != null)
            {
                chosenPattern = fuzzyBrain.EvaluateShootPattern(phoenix.PlayerDistance, phoenix.stamina);
            }

            GameObject bulletMother = ProjectilePool.Instance.GetProjectile(phoenix.firePoint.position, phoenix.firePoint.rotation);

            Projectile pScript = bulletMother.GetComponent<Projectile>();
            if (pScript != null)
            {
                pScript.isClone = false;
                // aqui le metes el patron que ha ganado en el fuzzy a la bala
                pScript.shootPattern = chosenPattern;

            }

            bulletMother.SetActive(true);
        }
        else
        {
            Debug.LogError("[ShootState] Te falta arrastrar el firePoint en el Phoenix!");
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootDuration)
            phoenix.ChangeState(phoenix.flyState);
    }
}