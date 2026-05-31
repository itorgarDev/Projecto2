using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necesario para mostrar texto en pantalla

public class TakeDrop : MonoBehaviour
{
    public string itemName; // Nombre del ítem
    public GameObject pickupEffect; // Efecto visual opcional

    public ItemEffect effectType; // enum
    public int amount = 1;       // Cuánto sube vida o ataque
    
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

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

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
                SavePlay.Instance.vida = stats.currentHealth;
                SavePlay.Instance.SaveData();
                break;
        }
    }

}

public enum ItemEffect
{
    None,
    Heal,
    Attack,
    MaxHealthUp
}
