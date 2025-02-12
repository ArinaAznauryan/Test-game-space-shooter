using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void OnEnable()
    {
        Invoke(nameof(Disappear), 2f);
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
