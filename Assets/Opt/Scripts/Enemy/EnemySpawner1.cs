using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class EnemySpawner1 : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float minimumSpawnTime = 1f;
    public float maximumSpawnTime = 4f;
    private bool canSpawn = true;

    private float _spawnTime;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (canSpawn)
        {
            _spawnTime = UnityEngine.Random.Range(minimumSpawnTime, maximumSpawnTime);
            yield return new WaitForSeconds(_spawnTime);
            
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
