using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGameOver : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private PlayerMovement movement;
    public void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Buscamos el componente PlayerMovement en el objeto que entró en el Trigger
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.StartCoroutine(player.DeathSequence());
                player.GameOver();
            }
            SoundController.Instance.PlaySFX(SoundController.Instance.deathSfx);
        }
    }
}
