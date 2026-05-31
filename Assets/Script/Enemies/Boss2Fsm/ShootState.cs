using UnityEngine;
using System.Collections; // hace falta esto pa las corrutinas

public class ShootState : TemplateStateMachine
{
    PhoenixFSM phoenix;
    float shootTimer;
    float shootDuration = 0.3f; // lo q dura el estado este

    private PhoenixFuzzyController fuzzyBrain;

    public ShootState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        base.Enter();

        // avisamos al animator q empiece el ataque ya
        phoenix.animator.SetTrigger("Attack");
        Debug.Log("ENTER Shoot");
        shootTimer = 0f;

        if (fuzzyBrain == null && phoenix != null)
        {
            fuzzyBrain = phoenix.GetComponent<PhoenixFuzzyController>();
        }

        // esto es pa q mire al jugador
        if (phoenix != null && phoenix.firePoint != null)
        {
            if (phoenix.target != null)
            {
                Vector3 targetBodyPosition = phoenix.target.position + new Vector3(0f, 1.1f, 0f);
                Vector3 targetDirection = targetBodyPosition - phoenix.firePoint.position;
                phoenix.firePoint.rotation = Quaternion.LookRotation(targetDirection);
            }

            // lanzamos la corrutina pa q dispare en el momento justo
            // como va a velocidad 4, el 1.16s se keda en 0.29s
            phoenix.StartCoroutine(DelayedShoot(0.29f));
        }
        else
        {
            Debug.LogError("[ShootState] Te falta arrastrar el firePoint en el Phoenix, espabilao!");
        }
    }

    private IEnumerator DelayedShoot(float delay)
    {
        // esperamos el tiempo q toca pa q cuadre con la animacion
        yield return new WaitForSeconds(delay);

        // a ver q patron decide el cerebro este
        Projectile.ShootPattern chosenPattern = Projectile.ShootPattern.SingleBullet;
        if (fuzzyBrain != null)
        {
            chosenPattern = fuzzyBrain.EvaluateShootPattern(phoenix.PlayerDistance, phoenix.stamina);
        }

        // sacamos la bala del pool pa no petar la memoria
        GameObject bulletMother = ProjectilePool.Instance.GetProjectile(phoenix.firePoint.position, phoenix.firePoint.rotation);

        Projectile pScript = bulletMother.GetComponent<Projectile>();
        if (pScript != null)
        {
            pScript.isClone = false;
            pScript.shootPattern = chosenPattern;
        }

        bulletMother.SetActive(true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        shootTimer += Time.deltaTime;

        // cuando pasa el tiempo volvemos a volar, tranki
        if (shootTimer >= shootDuration)
            phoenix.ChangeState(phoenix.flyState);
    }
}