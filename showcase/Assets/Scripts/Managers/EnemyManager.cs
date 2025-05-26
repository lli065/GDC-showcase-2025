using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public int maxEnemies = 10;
    public int numEnemies = 0;
    public Vector3 bossFightCenter;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanSpawnEnemy()
    {
        return numEnemies < maxEnemies;
    }

    public void AddEnemy()
    {
        numEnemies++;
    }

    public void RemoveEnemy()
    {
        numEnemies = Mathf.Max(0, numEnemies - 1);
    }

    public void RemoveEnemiesInBossFight()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(bossFightCenter, enemy.transform.position) <= 100)
            {
                Destroy(enemy);
            }
        }
    }
}
