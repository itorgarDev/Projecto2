using UnityEngine;

public class BridgeSwap : MonoBehaviour
{
    public GameObject modelA;
    public GameObject modelB;
    public Animator animatorA;
    public Animator animatorB;

    private bool usingA = true;

    public void InteractWithBridge()
    {
        Debug.Log("[BridgeSwap] ¡Recibida orden de interacción desde el Panda!");
        PerformSwitch();
        /* if (!usingA)
         {
             Debug.LogWarning("[BridgeSwap] Interrupción: El puente YA ha sido reparado previamente.");
             return;
         }

         if (animatorA != null)
         {
             Debug.Log("[BridgeSwap] Lanzando Trigger 'Disappear' en animatorA...");
             animatorA.SetTrigger("Disappear");
         }
         else
         {
             Debug.LogError("[BridgeSwap] Alerta: animatorA es NULL. Cambiando de modelo directamente sin animación.");
             PerformSwitch();
         }*/
    }

    public void PerformSwitch()
    {
        Debug.Log("[BridgeSwap] ¡Animation Event ejecutado! Cambiando modelos 3D...");
        usingA = false;

        if (modelA != null) modelA.SetActive(false);
        if (modelB != null) modelB.SetActive(true);

        if (animatorB != null)
        {
            Debug.Log("[BridgeSwap] Lanzando Trigger 'Appear' en animatorB...");
            animatorB.SetTrigger("Appear");
        }
        else
        {
            Debug.LogWarning("[BridgeSwap] Nota: animatorB es NULL, el puente B apareció estático.");
        }
    }
}