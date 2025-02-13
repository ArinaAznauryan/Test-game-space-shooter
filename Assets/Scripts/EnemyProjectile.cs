using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyTools.Tools;

public enum EnemyProjectileType
{
    DEFAULT = 0,
    LETHAL = 1
}


public class EnemyProjectile : Projectile
{
    [SerializeField] private GameObject[] allVisuals;

    [SerializeField] private EnemyProjectileType _type;

    private void SetVisual()
    {
        int i = (int)_type; // Convert enum to int to get the index
        if (i >= 0 && i < allVisuals.Length)
        {
            allVisuals[i].SetActive(true);
        }
    }

    private int GetCorrespondingDamage(EnemyProjectileType type)
    {
        switch (type)
        {
            case EnemyProjectileType.DEFAULT:
                return 1;
            case EnemyProjectileType.LETHAL:
                return 3;
            default:
                return 1;
        }
    }

    public void SetType(EnemyProjectileType type)
    {
        _type = type;
        SetVisual();
        SetDamage(GetCorrespondingDamage(type));
    }

    public override void Init(float speed, int damage = 1)
    {
        base.Init(speed);

        int randomIndex = GetRandomValueInt(
            new RandomSelection(0f, 0f, .8f),
            new RandomSelection(1f, 1f, .2f)
        );

        var randomType = (EnemyProjectileType)randomIndex; //Choosing a random projectile type (DEFAULT - red with damage 1; LETHAL - purplish with damage 3)
        
        SetType(randomType);
    }

    private void OnDisable()
    {
        //Since the projectile doesnt get destroyed but just get disabled, its children won't get disabled individually, so disable them manually like this
        foreach (Transform child in transform) child.gameObject.SetActive(false);
    }

}