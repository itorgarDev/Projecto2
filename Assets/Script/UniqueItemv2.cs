using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueItemv2 : MonoBehaviour
{
    public string id;

    void Start()
    {
        if (SavePlay.Instance.IsItemCollected(id))
        {
            Debug.Log($"Item {id} ya recogido — destruyendo objeto raíz");
            Destroy(transform.root.gameObject);
        }
    }
}
