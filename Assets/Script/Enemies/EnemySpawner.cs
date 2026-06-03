using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private void Start()
    {
        if (EnemyPool.Instance != null)
        {
            // Le pedimos un enemigo al pool en la posición de este objeto
            EnemyFSMManager enemy = EnemyPool.Instance.GetFromPool(transform.position);

            //le ponemos la rotacion del empty tmb
            enemy.transform.rotation = transform.rotation;
        }
        else
        {
            Debug.LogError("¡No se encontró el EnemyPool en la escena!");
        }

        // Una vez que cumple su función se  destruye 
        Destroy(gameObject);
    }

    // vision del punto en escena
    private void OnDrawGizmos()
    {
        float radioSpawn = 10f;

        // Color del cuerpo (un rojo semitransparente para que no tape todo)
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, radioSpawn);

        // Color del borde (un rojo sólido para darle definición)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioSpawn);

        // Una flecha más larga para ver hacia dónde mirará el enemigo al aparecer
        Vector3 direccionMira = transform.forward * (radioSpawn * 1.5f);
        Gizmos.DrawRay(transform.position, direccionMira);
    }
}
