using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileObjectPool : ObjectPool
{
    override public void ResetMechanic()
    {
        foreach (var obj in pool)
        {
            if (obj.activeSelf && obj.transform.position.y > screenTop)
            {
                ResetObjectPosition(obj);
                obj.SetActive(false);
            }
        }
    }
}
