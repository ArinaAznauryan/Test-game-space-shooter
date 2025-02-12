using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectiveField : MonoBehaviour
{
    private float _duration;

    void OnEnable()
    {
        AdjustToPlayer();

        Invoke(nameof(Deactivate), 15);
    }

    public void AdjustToPlayer()
    {
        transform.parent = GetPlayer().transform;
        transform.localPosition = Vector3.zero;
        GetPlayer().SetFireDamage(20);
        GetPlayer().MakeVulnerable(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if it's an enemy
        {
            other.GetComponent<Enemy>().Die(); // Kill the enemy
        }
    }

    private void LateUpdate()
    {
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0f, transform.rotation.w);
    }

    private void Deactivate()
    {
        //GetPlayer().ResetDamage();
        transform.parent = null;
        gameObject.SetActive(false);
        GetPlayer().MakeVulnerable(true);
    }

    private Player GetPlayer() {
        return FindObjectOfType<Player>();
    }
}
