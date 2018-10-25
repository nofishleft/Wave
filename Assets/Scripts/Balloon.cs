using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Enemy {

    public Transform Barrel;
    public Transform ProjectileSpawner;
    
    public override void Move()
    {
        
    }

    public override void OnDeath()
    {
        Destroy(this.transform.parent.gameObject);
    }  
}
