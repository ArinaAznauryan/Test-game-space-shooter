using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize;
    public float screenTop;
    public float screenBottom;
    public Vector3 spawnPoint;

    protected List<GameObject> pool;


    public GameObject Spawn()
    {
        foreach (var obj in pool)
        {
            if (!obj.activeSelf)
            {
                obj.transform.position = spawnPoint;
                obj.SetActive(true);
                return obj;
            }
        }
        Debug.LogError("Couldn't find the object getting pooled!");
        return null;
    }

    public void Despawn(GameObject tarObj)
    {
        ResetObjectPosition(tarObj);
        tarObj.SetActive(false);
    }

    private void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, spawnPoint, Quaternion.identity);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    private void LateUpdate()
    {
        ResetMechanic();
    }

    public virtual void ResetMechanic() {

    }

    protected void ResetObjectPosition(GameObject obj)
    {
        obj.transform.position = spawnPoint; // Set position in world space
    }
}
