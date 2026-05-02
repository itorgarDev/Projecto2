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
        itemName = transform.root.name;
    }

    public void PickUp()
    {
        Debug.Log("Has recogido: " + itemName);

        if (itemText != null)
        {
            itemText.text = "Has recogido: " + itemName;
            itemPanel.gameObject.SetActive(true);
            itemText.gameObject.SetActive(true);

            CancelInvoke(nameof(HideText));
            Invoke(nameof(HideText), 2f);
        }

        ApplyEffect();

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        Destroy(transform.root.gameObject);
    }

    private void HideText()
    {
        if (itemText != null)
            itemPanel.gameObject.SetActive(false);
            itemText.gameObject.SetActive(false);
    }

    // --- NUEVO: lógica del efecto ---
    private void ApplyEffect()
    {
        switch (effectType)
        {
            case ItemEffect.Heal:
                Debug.Log("Vida aumentada +" + amount);
                break;

            case ItemEffect.Attack:
                Debug.Log("Ataque aumentado +" + amount);
                break;

            case ItemEffect.None:
            default:
                break;
        }
    }
}

public enum ItemEffect
{
    None,
    Heal,
    Attack
}
