using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    //This is Object pooling manager, whenever a new object is need to get pooled, make a script for it,
    //that derives from "ObjectPool", and override its method "ResetMechanic" to despawn it whenever needed, and then put that script's reference here;
    public EnemyObjectPool Enemy;
    public EnemyProjectileObjectPool EnemyProjectile;
    public PlayerProjectileObjectPool PlayerProjectile;
    public PowerUpObjectPool PowerUp;
    public HealPickUpObjectPool HealPickUp;
}
