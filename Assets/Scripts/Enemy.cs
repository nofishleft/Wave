using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float MaxHealth = 100;
    public float Health = 100;
    public float MovementSpeed = 100f;
    public float Damage = 60f;
    public float PlayerProjectilleDamage;
    public float PlayerCollisionDamage;
    public Transform Target;
    public static Transform targ;
    public float FireRate = 1f;
    public float FireWait = -1f;

    // Use this for initialization
    void Start()
    {
        
    }

    public virtual void OnStart()
    {

    }

    public virtual void OnDeath()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        switch (layer)
        {
            case Constants.LAYER_WATER:
                break;
            case Constants.LAYER_PROJECTILE_PLAYER:
                Health -= PlayerProjectilleDamage;
                Destroy(other.gameObject);
                if (Health <= 0) OnDeath();
                break;
            case Constants.LAYER_PROJECTILE_ENEMY:
                break;
            case Constants.LAYER_PLAYER:
                TRenderer.p.Health -= PlayerCollisionDamage;
                if (TRenderer.p.Health <= 0) TRenderer.OnDeath();
                break;
            case Constants.LAYER_ENEMY:
                break;
            default:
                break;
        }
    }

    public void Path()
    {
        
    }

}
