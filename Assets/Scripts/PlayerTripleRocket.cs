using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTripleRocket : Player
{
    [SerializeField] private Transform[] _projectileSpawnLocations;

    [SerializeField] private float timerDuration = 10f; // Set the timer duration in seconds
    private float timer = 0f; // Tracks the time passed

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateHealth(10);
        timer = 0f; // Reset the timer every time the object is enabled
    }

    protected override void Update()
    {
        
        // Update the timer
        timer += Time.deltaTime;

        // If the timer exceeds the set duration, disable the GameObject
        if (timer >= timerDuration)
        {
            GameController.Instance.player.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        base.Update();
    }

    protected override void FireProjectile()
    {
        foreach (Transform fireLocation in _projectileSpawnLocations)
        {
            var go = GameController.Instance.pool.PlayerProjectile.Spawn();
            go.transform.position = fireLocation.position;
        }
    }
}
