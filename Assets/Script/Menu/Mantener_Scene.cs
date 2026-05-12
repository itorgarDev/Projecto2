using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mantener_Scene : MonoBehaviour
{
    public int keptScene;
    public static Mantener_Scene Instance;
    private void Awake()
    {
        // Si ya existe una instancia, destruye la nueva para evitar duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Asigna esta instancia y haz que persista entre escenas
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

  
}
