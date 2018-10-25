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
        EnemyHPModifier = 1f;
        EnemyDamageModifier = 1f;
        Enemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        accumTime += Time.deltaTime;
        if (EnemyCount == 0)
        {
            if (EnemiesLeftToSpawn == 0)
            {
                ++Wave;
                switch (Random.Range(0, 3)) {
                    case 0:
                        ++EnemyPerWave;
                        break;
                    case 1:
                        EnemyHPModifier *= 1.1f;
                        break;
                    case 2:
                        EnemyDamageModifier *= 1.1f;
                        break;
                }
                EnemiesLeftToSpawn = EnemyPerWave;
                accumTime = 0;
            }
        }
        else if (accumTime >= 2)
        {
            Spawn();
            accumTime = 0;
        }
    }

    void Spawn() { }
}
