using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private PlayerAttack attack;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        //linea para que se pueda hacer el test
        if (animator == null) return;
        // Control de caminar
        bool isWalking = movement.MoveInput.magnitude > 0.01f && !movement.IsDashing;
        animator.SetBool("isWalking", isWalking);

        // Control de dash
        animator.SetBool("isDashing", movement.IsDashing);

        if (attack.isAttacking)
        {
            animator.SetTrigger("Attack");
        }
    }


}



