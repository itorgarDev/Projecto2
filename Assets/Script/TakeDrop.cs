using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDrop : MonoBehaviour
{
    public string itemName; // Nombre del ítem
    public GameObject pickupEffect;          // Efecto visual opcional

    private void Start()
    {
        itemName = transform.root.name;
    }
    public void PickUp()
    {
        Debug.Log("Has recogido: " + itemName);

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        Destroy(transform.root.gameObject);
    }
}
