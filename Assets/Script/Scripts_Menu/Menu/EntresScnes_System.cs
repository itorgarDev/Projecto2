using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntresScnes_System : MonoBehaviour
{
    private void Awake()
    {
        var dontDestroyScene = FindObjectsOfType<EntresScnes_System>();
        if(dontDestroyScene.Length>1)
        {
            Destroy(gameObject);
                return;
        }

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
