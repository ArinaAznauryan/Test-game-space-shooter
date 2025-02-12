using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    private float _speed = 3.0f;
    private GameObject visual;
    [SerializeField] private GameObject[] allVisuals;

    public enum PowerUpType {
        FIRE_RATE = 0,
        TRIPLE_ROCKET = 1
    }

    [SerializeField] private PowerUpType _type;

    void OnEnable()
    {
        visual = transform.GetChild(0).gameObject;
        SetVisual();
    }
        
    private void SetVisual()
    {
        switch (_type)
        {
            case PowerUpType.FIRE_RATE:
                visual = allVisuals[0];
                break;
            case PowerUpType.TRIPLE_ROCKET:
                visual = allVisuals[1];
                break;
            default: break;
        }
    }

    public void SetType(PowerUpType type) {
        _type = type;
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
