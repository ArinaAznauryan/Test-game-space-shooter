using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private float _speed = 0.0f;
    [SerializeField] private Vector3 _direction = Vector3.up;
    private int _damage = 1;

    public void Init(int damage, float speed)
    {
        _damage = damage;
        _speed = speed*1.75f; //1.75 is the ratio of the enemy's and its projectile's speed: enemy - 2f, projectile - 3.5f
    }


    void Update() {
        transform.position += _direction * (_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        var enemy = other.GetComponent<Enemy>();
        var player = other.GetComponent<Player>();

        if (enemy != null)
        {
            enemy.Hit(_damage);
            DestroyProjectile();
        }
        else if (player != null)
        {
            player.Hit();
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        GameController.Instance.pool.PlayerProjectile.Despawn(gameObject);
    }
}
