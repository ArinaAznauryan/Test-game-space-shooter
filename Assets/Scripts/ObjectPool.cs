using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab; //Prefab of the target object
    public int poolSize; //Put it to a little bit more than the maximum amount of that object, that can appear in the screen
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
        Debug.LogError("There are more objects trying to spawn than available ones! Increase the pool size!");
        return null;
    }

    public void Despawn(GameObject tarObj)
    {
        ResetObjectPosition(tarObj); //Putting the object outside the screen to its default position
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
        //This function has to be overriden from the extended classes, so that the pooled objects despawn when getting outside the screen
        ResetMechanic();
    }

    public virtual void ResetMechanic() {

    }

    protected void ResetObjectPosition(GameObject obj)
    {
        obj.transform.position = spawnPoint;
    }
}
