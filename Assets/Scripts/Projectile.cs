using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private float _speed = 0.0f;
    [SerializeField] private Vector3 _direction = Vector3.up;
    [SerializeField] private int _damage = 1;

    public virtual void Init(float speed, int damage = 1)
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
            player.Hit(_damage);
            DestroyProjectile();
        }
    }

    protected void SetDamage(int damage)
    {
        _damage = damage;
    }

    private void DestroyProjectile()
    {
        GameController.Instance.pool.PlayerProjectile.Despawn(gameObject);
    }
}
