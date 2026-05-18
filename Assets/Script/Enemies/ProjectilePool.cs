using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 30;

    private List<GameObject> pooledProjectiles = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pooledProjectiles.Add(obj);
        }
    }

    public GameObject GetProjectile(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < pooledProjectiles.Count; i++)
        {
            if (!pooledProjectiles[i].activeInHierarchy)
            {
                GameObject obj = pooledProjectiles[i];
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                // ¡OJO! Hemos quitado el SetActive(true) de aki pa k no de bucle infinito
                return obj;
            }
        }

        GameObject newObj = Instantiate(projectilePrefab, position, rotation);
        newObj.SetActive(false); // nace apagada x si acaso
        pooledProjectiles.Add(newObj);
        return newObj;
    }

    public void ReturnToPool(GameObject projectile)
    {
        projectile.SetActive(false);
    }
}