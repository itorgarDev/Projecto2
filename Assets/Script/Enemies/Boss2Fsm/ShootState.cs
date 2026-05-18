using UnityEngine;

public class ShootState : TemplateStateMachine
{
    PhoenixFSM phoenix;
    float shootTimer;
    float shootDuration = 1f;

    public ShootState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow; //
    }

    public override void Enter()
    {
        base.Enter(); //

        Debug.Log("ENTER Shoot"); //
        shootTimer = 0f; //

        if (phoenix != null && phoenix.firePoint != null) //
        {
            // 1. Apuntamos a la barriga del jugador sumando un desfase vertical (1.1 metros hacia arriba)
            if (phoenix.target != null) //
            {
                // CAMBIADO: Modificamos el punto objetivo sumándole altura al transform base del jugador
                Vector3 targetBodyPosition = phoenix.target.position + new Vector3(0f, 1.1f, 0f);

                // Calculamos la dirección inclinando el firePoint hacia el torso
                Vector3 targetDirection = targetBodyPosition - phoenix.firePoint.position;
                phoenix.firePoint.rotation = Quaternion.LookRotation(targetDirection); //
            }

            // 2. Pillamos la bala apagada del pool
            GameObject bulletMother = ProjectilePool.Instance.GetProjectile(phoenix.firePoint.position, phoenix.firePoint.rotation); //

            // 3. La configuramos bien
            Projectile pScript = bulletMother.GetComponent<Projectile>(); //
            if (pScript != null) //
            {
                pScript.isClone = false; //
            }

            // 4. La encendemos a salvo
            bulletMother.SetActive(true); //
        }
        else
        {
            Debug.LogError("[ShootState] Te falta arrastrar el firePoint en el Phoenix!"); //
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic(); //
        shootTimer += Time.deltaTime; //

        // cuando pasa el segundo de ataque, vuelve a volar
        if (shootTimer >= shootDuration) //
            phoenix.ChangeState(phoenix.flyState); //
    }
}