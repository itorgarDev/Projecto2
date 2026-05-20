using UnityEngine;

public class PhoenixFuzzyController : MonoBehaviour
{
    private float distance;
    private float stamina;
    private float healthPercent;

    // a partir de 15 el grado de pertenencia es 0 pero ya esta "cerca"
    float GetFuzzyDistanceClose() => Mathf.Clamp01((15f - distance) / 6f);

    // 10 y 26 son pertenencia 0 siendo los extremos y de 16 a 20 son de pertenencia 1 
    float GetFuzzyDistanceMedium()
    {
        if (distance < 10f || distance > 26f) return 0f;
        if (distance >= 16f && distance <= 20f) return 1f;
        if (distance < 16f) return (distance - 10f) / 6f;
        return (26f - distance) / 6f;
    }

   // a partir de 20 el grado de pertenencia es 0 pero ya esta "lejos"
    float GetFuzzyDistanceFar() => Mathf.Clamp01((distance - 20f) / 8f);

    // Si baja de 0.5 se cansa y 0.0 es cansancio maximo
    float GetFuzzyStaminaLow() => Mathf.Clamp01((0.5f - stamina) / 0.5f);

    // Empieza en 0.5 y 1.0 es energia maxima
    float GetFuzzyStaminaHigh() => Mathf.Clamp01((stamina - 0.5f) / 0.5f);

    // a partir de 0.7 deja de estar sano en 0.3 ya no lo esta 
    float GetFuzzyHealthHealthy() => Mathf.Clamp01((healthPercent - 0.3f) / 0.4f);

    // Empieza a agobiarse al 40% y se vuelve un kamikaze total al 10% de vida. Ancho = 0.3
    float GetFuzzyHealthCritical() => Mathf.Clamp01((0.4f - healthPercent) / 0.3f);

    public Projectile.ShootPattern EvaluateShootPattern(float currentDistance, float currentStamina)
    {
        this.distance = currentDistance;
        this.stamina = currentStamina;

        float close = GetFuzzyDistanceClose();
        float medium = GetFuzzyDistanceMedium();
        float far = GetFuzzyDistanceFar();
        float lowStam = GetFuzzyStaminaLow();
        float highStam = GetFuzzyStaminaHigh();

        // r1: si estas cerca pues rafaga obligada pa alejar al pive
        float r1 = close;
        // r2: si estas a media distancia y encima tienes estamina alta pos metes rafaga
        float r2 = Mathf.Min(medium, highStam);
        // r3: si estas cansaote (estamina baja) tiras bala unica pa no gastar
        float r3 = lowStam;
        // r4: si el tio esta lejos pos disparas de uno en uno pa molestar
        float r4 = far;

        // desdifunificasion de esa: miramos que pesa mas si la rafaga o el tiro unico
        float weightBurst = Mathf.Max(r1, r2);
        float weightSingle = Mathf.Max(r3, r4);

        // si el peso de rafaga es mayor rafaga sino un disparo solo
        return (weightBurst >= weightSingle) ? Projectile.ShootPattern.Burst : Projectile.ShootPattern.SingleBullet;
    }

    public float EvaluateOrbitalSpeedMultiplier(float currentDistance, float currentStamina)
    {
        this.distance = currentDistance;
        this.stamina = currentStamina;

        float close = GetFuzzyDistanceClose();
        float highStam = GetFuzzyStaminaHigh();
        float lowStam = GetFuzzyStaminaLow();

        // r1: si tiene energia y estas cerca se vuelve loco dando vueltas rapido
        float r1 = Mathf.Min(highStam, close);
        // r2: si esta rebentao pos se frena pa recuperar el aire el bicho
        float r2 = lowStam;

        //aqui se hace la media pondera para devolver un numero que sera lo que varie la velocidad. el 0.1f se usa para asegurarno de que haya una velocidad normal
        float totalWeights = r1 + r2 + 0.1f;
        return ((r1 * 1.6f) + (r2 * 0.5f) + (0.1f * 1.0f)) / totalWeights;
    }

    public float EvaluateHarassmentWeight(float currentDistance, float currentHealth, float maxHealth)
    {
        this.healthPercent = currentHealth / maxHealth;
        this.distance = currentDistance;

        float far = GetFuzzyDistanceFar();
        float critical = GetFuzzyHealthCritical();
        float healthy = GetFuzzyHealthHealthy();

        // r1: si le queda poquísima vida y el tio esta lejos va a saco a por el en plan kamikaze
        float r1 = Mathf.Min(critical, far);
        // r2: si aun esta sano prefiere dar vueltas trankilamente sin meterse en fregaos
        float r2 = healthy;

        // otra media ponderada que devuelve como de agresivo tiene que ser el bicho
        float totalWeights = r1 + r2 + 0.05f;
        return ((r1 * 1.0f) + (r2 * 0.1f) + (0.05f * 0.0f)) / totalWeights;
    }
}