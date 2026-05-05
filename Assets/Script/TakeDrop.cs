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

    public GameObject itemPanel;
    public TMP_Text itemText; 

    private void Start()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = gameObject.name;
    }

    public void PickUp()
    {
        StartCoroutine(PickupRoutine());
    }

    private IEnumerator PickupRoutine()
    {
        Debug.Log("Has recogido: " + itemName);

        if (itemText != null)
        {
            itemText.text = "Has recogido: " + itemName;
            if (itemPanel != null) itemPanel.SetActive(true);
            itemText.gameObject.SetActive(true);
        }

        ApplyEffect();

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        // Espera 2 segundos antes de ocultar el texto y destruir el ítem
        yield return new WaitForSeconds(1.5f);

        HideText();

        Destroy(transform.root.gameObject);
    }

    private void HideText()
    {
        if (itemPanel != null)
            itemPanel.SetActive(false);

        if (itemText != null)
            itemText.gameObject.SetActive(false);
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
                break;

            case ItemEffect.Attack:
                stats.AddAttack(amount);
                break;
            case ItemEffect.MaxHealthUp:
                stats.IncreaseMaxHealth(amount);
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
