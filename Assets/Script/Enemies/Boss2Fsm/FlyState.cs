using UnityEngine;

public class FlyState : TemplateStateMachine
{
    PhoenixFSM phoenix;
    private SphereCollider arenaCollider;
    public bool playerInside = false;

    // variables pal tiempo de recarga del tiro
    float shootCooldownTimer = 0f;
    float timeBetweenAttacks = 1.1f; // se tiene k esperar 1.1 segundos entre ataquees

    public FlyState(string name, PhoenixFSM _stateMachineFlow) : base(name, _stateMachineFlow)
    {
        phoenix = _stateMachineFlow;
    }

    public override void Enter()
    {
        Debug.Log("ENTER Fly");
        phoenix.ResetAire();
        phoenix.animator.SetBool("IsFlying", true);
        // cada vez k vuelve a volar, reiniciamos el reloj de recarga pa k no tire instantaneo
        shootCooldownTimer = 0f;

        if (arenaCollider == null && phoenix != null)
        {
            BossArenaDetector sonDetector = phoenix.GetComponentInChildren<BossArenaDetector>();
            if (sonDetector != null)
            {
                arenaCollider = sonDetector.GetComponent<SphereCollider>();
            }
            else
            {
                Debug.LogError("[FlyState] No encuentro el BossArenaDetector en los hijos del Fenix!");
            }
        }
    }

    public override void UpdateLogic()
    {
        if (!playerInside && arenaCollider != null && phoenix.target != null)
        {
            
            float distancia = Vector3.Distance(arenaCollider.transform.position, phoenix.target.position);
            if (distancia <= arenaCollider.radius)
            {
                playerInside = true; 
            }
        }
        // Si el detector nos dice que el player no ha entrado, el Fenix se queda tieso flotando
        if (!playerInside)
        {
            // Bloqueamos movimiento orbital, disparos, gasto de estamina, TODO.
            return;
        }

        // medimos la distancia directa contra el player
        if (phoenix.target != null)
            phoenix.PlayerDistance = Vector3.Distance(phoenix.transform.position, phoenix.target.position); 

        phoenix.AirTime += Time.deltaTime;

        // aqui bajamos la estamina 
        phoenix.stamina -= Time.deltaTime * 0.05f;
        if (phoenix.stamina < 0f)
            phoenix.stamina = 0f;

        // si se queda sin vida en fase uno pos va a cambiar de fase
        if (phoenix.CurrentPhase == 1 && phoenix.Health <= 0f)
        {
            phoenix.ChangeState(phoenix.phaseTransitionState);
            return;
        }

        // aumentamos el reloj de la recarga
        shootCooldownTimer += Time.deltaTime;

        if (phoenix.PlayerDistance < phoenix.maxShootRange && shootCooldownTimer >= timeBetweenAttacks)
        {
            phoenix.ChangeState(phoenix.shootState);
            return;
        }

        // si se cansa ponemos el goingup en false pa q baje
        if (phoenix.stamina < 0.2f)
        {
            phoenix.transitionAirState.goingUp = false;
            phoenix.ChangeState(phoenix.transitionAirState);
            return;
        }

        OrbitalMovement();
        LookAtPlayer();
    }

    void OrbitalMovement()
    {
        if (phoenix.target == null) return;

        Vector3 center = phoenix.target.position;
        Vector3 offset = phoenix.transform.position - center;
        float currentDist = offset.magnitude;

        if (currentDist < 0.1f)
        {
            offset = new Vector3(1f, 0f, 0f);
            currentDist = 1f;
        }

        // pillamos los multiplicadores que calcula el fuzzy usando la distancia, estamina y vida
        PhoenixFuzzyController fuzzyBrain = phoenix.GetComponent<PhoenixFuzzyController>();
        float speedMultiplier = 1f;
        float harassmentWeight = 0f;

        if (fuzzyBrain != null)
        {
            speedMultiplier = fuzzyBrain.EvaluateOrbitalSpeedMultiplier(phoenix.PlayerDistance, phoenix.stamina);
            harassmentWeight = fuzzyBrain.EvaluateHarassmentWeight(phoenix.PlayerDistance, phoenix.Health, phoenix.maxHealth);
        }

        // calculamos el radio objetivo dinámico. Si esta agonizando el acoso vale 1 y el radio baja a 8 (espiral kamikaze)
        float targetRadio = 20f - (harassmentWeight * 12f);

        // fuerza orbital para girar en circulos 
        Vector3 orbitalDirection = Vector3.Cross(offset, Vector3.up).normalized;
        float finalOrbitalSpeed = 4f * speedMultiplier; // el 4f es tu velocidad base
        Vector3 orbitalVelocity = orbitalDirection * finalOrbitalSpeed;

        // fuerza radial para corregir la distancia y que no se safe del mapa
        Vector3 radialDirection = offset.normalized;
        float radialError = currentDist - targetRadio;
        Vector3 radialVelocity = -radialDirection * radialError * 1.5f; // el 1.5f es la fuerza de atraccion pa volver al radio suave

        // combinamos las dos velocidades en este frame
        phoenix.transform.position += (orbitalVelocity + radialVelocity) * Time.deltaTime;
    }

    void LookAtPlayer()
    {
        if (phoenix.target == null) return;

        Vector3 dir = phoenix.target.position - phoenix.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
            phoenix.transform.rotation = Quaternion.LookRotation(dir);
    }
}