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
    }


    public void SetType(PowerUpType type) {
        _type = type;
        SetVisual();
    }

    private void Update() {
        transform.position += Vector3.down * (_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        
        var player = other.GetComponent<Player>();
        if (player == null) return;

        player.AddPowerUp(_type);
        GameController.Instance.pool.PowerUp.Despawn(gameObject); //When the power-up hits the player, it despawns as lik acquiring it
    }

    private void OnDisable()
    {
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }
}
