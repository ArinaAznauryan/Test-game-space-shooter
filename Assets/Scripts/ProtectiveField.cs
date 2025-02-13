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
        GetPlayer().SetFireDamage(20); //The protective field kills all the enemies touching it
        GetPlayer().MakeVulnerable(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Die(); //Kill the enemy
        }
    }

    private void LateUpdate()
    {
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 0f, transform.rotation.w); //Freeze its rotation across Z axis
    }

    private void Deactivate()
    {
        transform.parent = null; //Reset its parent for more maintainability
        gameObject.SetActive(false);
        GetPlayer().MakeVulnerable(true); //Make the player vulnerable again when the protective field is gone
    }

    private Player GetPlayer() {
        return FindObjectOfType<Player>();
    }
}
