using nz.Rishaan.DynamicCuboidTerrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text WaveText;
    public Text EnemiesAliveText;
    public Text EnemiesToSpawnText;
    public static int EnemyCountTextSet;

    public static void RemoveEnemy(Enemy e)
    {
        Enemies.Remove(e);
        EnemyCount = Enemies.Count;
        EnemyCountTextSet = EnemyCount;

    }

    public void AddEnemy(Enemy e)
    {
        Enemies.Add(e);
        EnemyCount = Enemies.Count;
        EnemyCountTextSet = EnemyCount;
    }

    // Use this for initialization
    void Start()
    {
        Wave = 1;
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
        EnemiesAliveText.text = "Alive: " + EnemyCountTextSet;
        accumTime += Time.deltaTime;
        if (EnemyCount == 0 && EnemiesLeftToSpawn == 0)
        {
            ++Wave;
            WaveText.text = "Wave: " + Wave;
            switch (Random.Range(0, 3))
            {
                case 0:
                    ++EnemyPerWave;
                    break;
                case 1:
                    EnemyHPModifier *= 1.1f;
                    break;
                case 3:
                    EnemyDamageModifier *= 1.1f;
                    break;
            }
            EnemiesLeftToSpawn = EnemyPerWave;
            EnemiesToSpawnText.text = "Spawning: " + EnemiesLeftToSpawn;
            accumTime = -3;
        }
        if (accumTime >= 2 && EnemiesLeftToSpawn > 0)
        {
            Spawn();
            --EnemiesLeftToSpawn;
            EnemiesToSpawnText.text = "Spawning: " + EnemiesLeftToSpawn;
            accumTime = 0;
        }
    }

    void Spawn()
    {
        Enemy e = Instantiate(BalloonPrefab).GetComponentInChildren<Balloon>();
        e.Target = Target;
        e.MaxHealth *= EnemyHPModifier;
        e.Health *= EnemyHPModifier;
        e.Damage *= EnemyDamageModifier;
        Vector3 circ = Random.insideUnitCircle * (TRenderer.render / 4);
        circ.z = circ.y;
        circ.y = 0;
        e.transform.parent.position = new Vector3(TRenderer.render / 2, e.transform.parent.position.y, TRenderer.render / 2) + circ;
        AddEnemy(e);
    }
}
