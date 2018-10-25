using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float MaxHealth;
    public float Health;
    public float MovementSpeed;
    public float PlayerProjectilleDamage;
    public float PlayerCollisionDamage;
    public Transform Player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Move();
	}

    
    public virtual void Move() {
        
    }

    public virtual void OnDeath() {

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
        print(1);
    }
            

}
