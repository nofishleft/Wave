using BeautifulDissolves;
using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : Enemy {

    public Transform Barrel;
    public Transform ProjectileSpawner;
    public Dissolve one;
    public Dissolve two;
    bool rotating = true;
    float rotation = 360f;
    bool dead;
    public GameObject o;
    public GameObject EffectPrefab;

    public float barrelRotationVert
    {
        get
        {
            return Barrel.transform.localRotation.eulerAngles.x;
        }
        set
        {
            Barrel.transform.localRotation = Quaternion.RotateTowards(Barrel.transform.localRotation, Quaternion.Euler(value + 90f, 0, 0), rotation * Time.deltaTime);
        }
    }

    public float barrelRotationHor
    {
        get
        {
            return transform.parent.rotation.eulerAngles.y;
        }
        set
        {
            transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, Quaternion.Euler(0, value, 0), rotation * Time.deltaTime);
        }
    }

    void Update()
    {
        if (rotating && !dead)
        {
            if (PlayerHealth.dead) return;
            FireWait += Time.deltaTime;
            Quaternion q = Quaternion.LookRotation(Target.position - Barrel.transform.position);
            barrelRotationHor = q.eulerAngles.y;
            barrelRotationVert = q.eulerAngles.x;
            if (FireWait >= FireRate) {
                Vector3 rot = ProjectileSpawner.position - Barrel.position;
                GameObject ob = Instantiate(o, ProjectileSpawner.position, Quaternion.LookRotation(rot));
                ob.layer = Constants.LAYER_PROJECTILE_ENEMY;
                Thruster t = ob.GetComponent<Thruster>();
                t.target = Target;
                t.Eff = Instantiate(EffectPrefab, ProjectileSpawner.position, Quaternion.Euler(0, 15, 0) * Quaternion.LookRotation(rot));
                t.damage = Damage;
                FireWait = 0;
            }
        }
    }

    public override void OnStart()
    {
        
    }

    public override void OnDeath()
    {
        if (dead) return;
        one.TriggerDissolve();
        two.TriggerDissolve();
        StartCoroutine("StopRotating");
        Spawner.RemoveEnemy(this);
        //--Spawner.EnemyCount;
        Destroy(this.transform.parent.gameObject, 5f);
    }

    IEnumerator StopRotating ()
    {
        rotation = 90f;
        while (rotation > 10f) {
            yield return new WaitForSeconds(0.1f);
            rotation *= 0.5f;
        }
        yield return new WaitForSeconds(1f);
        Destroy(this);
    }

}
