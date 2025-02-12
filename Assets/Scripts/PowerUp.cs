using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    private float _speed = 3.0f;
    [SerializeField] private GameObject[] allVisuals;

    public enum PowerUpType {
        FIRE_RATE = 0,
        TRIPLE_ROCKET = 1,
        PROTECTIVE_FIELD = 2
    }

    [SerializeField] private PowerUpType _type;
        
    private void SetVisual()
    {
        int i = (int)_type; // Convert enum to int to get the index
        if (i >= 0 && i < allVisuals.Length)
        {
            allVisuals[i].SetActive(true);
        }
        //switch (_type)
        //{
        //    case PowerUpType.FIRE_RATE:
        //        allVisuals[0].SetActive(true);
        //        break;

        //    case PowerUpType.TRIPLE_ROCKET:
        //        allVisuals[1].SetActive(true);
        //        break;
        //    case PowerUpType.PROTECTIVE_FIELD:
        //        allVisuals[2].SetActive(true);
        //        break;
        //    default: break;
        //}
    }


    public void SetType(PowerUpType type) {
        _type = type;
        SetVisual();
    }

    private void Update() {
        var p = transform.position;
        p += Vector3.down * (_speed * Time.deltaTime);
        transform.position = p;
    }

    private void OnTriggerEnter(Collider other) {
        
        var player = other.GetComponent<Player>();
        if (player == null) return;

        player.AddPowerUp(_type);
        Destroy(gameObject);

    }
}
