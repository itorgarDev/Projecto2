using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueItem : MonoBehaviour
{
    public string id = "BolsaItem1";

    void Start()
    {
        if (id == "BolsaItem1" && SavePlay.Instance.bolsaItem1)
        {
            Destroy(gameObject);
        }
    }
}
