using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private static List<Enemy> Enemies;
    public static int EnemyCount;
    int EnemiesLeftToSpawn;
    int Wave;
    float accumTime;
    int EnemyPerWave;
    float EnemyHPModifier;
    float EnemyDamageModifier;
    public GameObject BalloonPrefab;
    public Transform Target;

    public static void RemoveEnemy(Enemy e)
    {
        Enemies.Remove(e);
        EnemyCount = Enemies.Count;
    }

    public void AddEnemy(Enemy e)
    {
        Enemies.Add(e);
        EnemyCount = Enemies.Count;
    }

    // Use this for initialization
    void Start()
    {
        EnemyPerWave = 1;
        EnemyCount = 1;
        EnemiesLeftToSpawn = 1;
        EnemyHPModifier = 1f;
        EnemyDamageModifier = 1f;
        Enemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        accumTime += Time.deltaTime;
        if (EnemyCount == 0 && EnemiesLeftToSpawn == 0)
            {
                ++Wave;
                switch (Random.Range(0, 5)) {
                    case 0:
                        ++EnemyPerWave;
                        break;
                    case 1:
                        EnemyHPModifier *= 1.1f;
                        break;
                    case 2:
                        EnemyHPModifier *= 1.1f;
                        break;
                    case 3:
                        EnemyDamageModifier *= 1.1f;
                        break;
                    case 4:
                        EnemyDamageModifier *= 1.1f;
                        break;
            }
                EnemiesLeftToSpawn = EnemyPerWave;
                accumTime = -3;
            }
        if (accumTime >= 2 && EnemiesLeftToSpawn > 0)
        {
            Spawn();
            --EnemiesLeftToSpawn;
            accumTime = 0;
        }
    }

    void Spawn() {
        Enemy e = Instantiate(BalloonPrefab).GetComponentInChildren<Balloon>();
        e.Target = Target;
        e.MaxHealth *= EnemyHPModifier;
        e.Health *= EnemyHPModifier;
        e.Damage *= EnemyDamageModifier;
        Vector3 circ = Random.insideUnitCircle * (TRenderer.render / 4);
        circ.z = circ.y;
        circ.y = 0;
        e.transform.parent.position = new Vector3(TRenderer.render/2, e.transform.parent.position.y, TRenderer.render/2) + circ;
        AddEnemy(e);
    }
}
