using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using PrimeTweenDemo;

public class HealPickUp : MonoBehaviour
{
    private float _speed = 3.0f;
    [SerializeField] private float hitFrequency = 10f;
    private SpriteRenderer spriteRenderer;

    private void Update()
    {
        transform.position += Vector3.down * (_speed * Time.deltaTime); //Going down
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player == null) return;

        player.AddHealth();
        GameController.Instance.pool.HealPickUp.Despawn(gameObject);
    }

    private void OnEnable()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);

        PulseAnimation();
    }

    private void PulseAnimation()
    {
        //This tween punches the heart down as if appearing, and changes its scale in intervals as if the heart pounding
        var punchDir = -transform.up;
        Tween.PunchLocalPosition(transform, strength: punchDir, duration: 1f, frequency: hitFrequency);

        Tween.Scale(transform, endValue: .7f, duration: 1, cycles: -1, cycleMode: CycleMode.Yoyo);
    }
}
