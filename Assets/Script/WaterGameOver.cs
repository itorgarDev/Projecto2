using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGameOver : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    // Opción 1: Si el objeto es un Trigger (tiene "Is Trigger" marcado en su Collider)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Buscamos el componente PlayerMovement en el objeto que entró en el Trigger
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.GameOver();
            }
        }
    }
}
