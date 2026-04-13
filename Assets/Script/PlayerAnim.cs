using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private PlayerAttack playerAttack;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        
    }

    void Update()
    {
        //linea para que se pueda hacer el test
        if (animator == null) return;
        // Control de caminar
        bool isWalking = movement.MoveInput.magnitude > 0.01f && !movement.IsDashing;
        animator.SetBool("isWalking", isWalking);

        //ataque
        animator.SetBool("isAttacking", playerAttack.IsAttacking);

        // Control de dash
        animator.SetBool("isDashing", movement.IsDashing);
       

    }
}


