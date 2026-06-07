using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueItemv2 : MonoBehaviour
{
    public string id;

    [Header("Referencias de Jerarquía")]
    [Tooltip("Arrastra aquí el objeto raíz (ej: Item_1) para destruirlo si ya fue recogido.")]
    public GameObject mainItemHolder;

    void Start()
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError($"¡El objeto {gameObject.name} no tiene asignada una ID única!", gameObject);
            return;
        }

        if (mainItemHolder == null)
        {
            mainItemHolder = transform.parent != null ? transform.parent.gameObject : gameObject;
        }

        if (SavePlay.Instance.IsItemCollected(id))
        {
            Debug.Log($"Item Único [{id}] ya fue recogido — destruyendo objeto de la escena.");
            Destroy(mainItemHolder);
        }
    }
}
