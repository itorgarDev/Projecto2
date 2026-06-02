using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class TakeDrop : MonoBehaviour
{
    public string itemName; // Nombre del ítem
   

    public ItemEffect effectType; // enum
    public int amount = 1;       // Cuánto sube vida o ataque
    [Tooltip("Arrastra aquí la pared invisible que desaparecerá al recoger este ítem.")]
    public GameObject lockZone; // pared que no te deja avanzar
    
    private void Start()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = gameObject.name;
    }

    public void PickUp()
    {
        HUDController.Instance.ShowPickupMessage(itemName);
        ApplyEffect();

        UniqueItemv2 unique = GetComponent<UniqueItemv2>();
        if (unique != null)
        {
            SavePlay.Instance.MarkItemCollected(unique.id);
        }
       
        Destroy(transform.root.gameObject);
    }
        
    private void ApplyEffect()
    {
        PlayerStats stats = FindObjectOfType<PlayerStats>();

        if (stats == null)
        {
            Debug.LogWarning("No se encontró PlayerStats en la escena.");
            return;
        }

        switch (effectType)
        {
            case ItemEffect.Heal:
                stats.Heal(amount);
                SoundController.Instance.PlaySFX(SoundController.Instance.health);
                break;

            case ItemEffect.Attack:
                stats.AddAttack(amount);
                SoundController.Instance.PlaySFX(SoundController.Instance.woodDestroyed);
                SavePlay.Instance.ataque = stats.attack;
                SavePlay.Instance.SaveData();
                break;
            case ItemEffect.MaxHealthUp:
                stats.IncreaseMaxHealth(amount);
                SoundController.Instance.PlaySFX(SoundController.Instance.health);

                SavePlay.Instance.maxHealth = stats.maxHealth;
                SavePlay.Instance.vida = stats.currentHealth;
                SavePlay.Instance.SaveData();
                break;
            case ItemEffect.Key:
                if (lockZone != null)
                {
                    lockZone.SetActive(false);
             
                }
                break;
        }
    }

}

public enum ItemEffect
{
    None,
    Heal,
    Attack,
    MaxHealthUp,
    Key
}
