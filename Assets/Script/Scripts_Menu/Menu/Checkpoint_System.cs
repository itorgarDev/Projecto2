using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_System : MonoBehaviour
{
  
        [SerializeField] private PlayerMovement player;

        public void Return()
        {
            player.ClosePauseMenu();
        }

}
