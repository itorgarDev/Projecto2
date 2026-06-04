using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;
    private PlayerAttack playerAttack;
    private PlayerStats stats;

    private bool deathTriggerSent = false;


    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        stats = GetComponent<PlayerStats>();

    }

    void Update()
    {
        if (animator == null) return;
        if (stats != null && stats.currentHealth <= 0)
        {
            if (!deathTriggerSent)
            {
                animator.SetTrigger("IsDead"); // Dispara el Trigger en tu Animator
                deathTriggerSent = true;
            }
            return; // Detiene el resto de animaciones ya que está muerto
        }
        else
        {
            // Cuando revive y recupera vida, reiniciamos la bandera
            deathTriggerSent = false;
        }

        // Control de caminar
        bool isWalking = movement.MoveInput.magnitude > 0.01f && !movement.IsDashing;
        animator.SetBool("isWalking", isWalking);

        // Control de dash
        animator.SetBool("isDashing", movement.IsDashing);
       

    }

}


