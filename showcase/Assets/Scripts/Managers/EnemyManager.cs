using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public int maxEnemies = 30;
    public int numEnemies = 0;

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
}
