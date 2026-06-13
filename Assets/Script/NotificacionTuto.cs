using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificacionTuto : MonoBehaviour
{
   
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundController.Instance.PlaySFX(SoundController.Instance.sfxNoti);
        }
           
  
    }

   
}
