using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public float Health;
    public float MaxHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDeath() {
        TRenderer.PAUSED = true;
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(1);
        int layer = other.gameObject.layer;
        switch (layer)
        {
            case Constants.LAYER_WATER:
                break;
            case Constants.LAYER_PROJECTILE_PLAYER:
                break;
            case Constants.LAYER_PROJECTILE_ENEMY:
                Health -= other.gameObject.GetComponent<Thruster>().damage;
                Destroy(other.gameObject);
                if (Health <= 0) OnDeath();
                break;
            case Constants.LAYER_PLAYER:
                break;
            case Constants.LAYER_ENEMY:
                break;
            default:
                break;
        }
    }
}
