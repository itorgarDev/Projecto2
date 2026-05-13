using UnityEngine;
using UnityEngine.InputSystem;

public class ModelSwitcherAnimated : MonoBehaviour
{
    public GameObject modelA;
    public GameObject modelB;
    public Animator animator;

    private bool usingA = true;

    // MISMO NOMBRE DE LA ACCIÓN: Interact
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        animator.SetTrigger("Switch");
    }

    // Animation Event
    public void PerformSwitch()
    {
        usingA = !usingA;

        modelA.SetActive(usingA);
        modelB.SetActive(!usingA);
    }
}
