using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    void OnEnable()
    {
        Invoke(nameof(Disappear), 2f); //The explosion disappear after 2 seconds
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
}
