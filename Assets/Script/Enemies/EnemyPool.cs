using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // Singleton para acceder al pool desde cualquier parte
    public static EnemyPool Instance;

    [SerializeField] private EnemyFSMManager enemyPrefab;
    [SerializeField] private int initialSize = 10; // Tamaño del pool

    // Cola para guardar a los enemigos
    private Queue<EnemyFSMManager> pool = new Queue<EnemyFSMManager>();
   

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < initialSize; i++) 
        {
            EnemyFSMManager enemy = Instantiate(enemyPrefab);
            enemy.gameObject.SetActive(false); // los crea desactivado para que no molesten
            pool.Enqueue(enemy);               // los guardamos en el pool
        }
    }

    // Método para sacar un enemigo del pool
    public EnemyFSMManager GetFromPool(Vector3 position)
    {
        EnemyFSMManager enemy;
        // Si hay en el Pool Lo saca para usarlo
        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else // Si el pool está vacío, instanciamos uno nuevo
        {
            enemy = Instantiate(enemyPrefab);
        }

        // Le damos la posicion donde vaya
        enemy.transform.position = position;

        // lo activamos en escena
        enemy.gameObject.SetActive(true);

        return enemy;
    }

    // Método para devolver un enemigo al pool
    public void ReturnToPool(EnemyFSMManager enemy)
    {
        enemy.gameObject.SetActive(false); // se oculta
        pool.Enqueue(enemy);               // se guarda en la cola 
    }
}
