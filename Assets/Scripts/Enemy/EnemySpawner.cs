using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private GameObject devilPrefab;
    [SerializeField]
    private List<EnemyWave> wavesToSpawn;

    [SerializeField]
    private float initialSpawnDelay;

    private int currentWaveIndex = 0;

    [SerializeField]
    private ZombieManager zombieManager;

    private Transform player;

    private float countdown;

    private bool hasFinishedSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = initialSpawnDelay;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTestZombieInput();

        if (hasFinishedSpawning)
            return;

        if (wavesToSpawn[currentWaveIndex].WaveFinished) 
        {
            if (currentWaveIndex + 1 < wavesToSpawn.Count) 
            {
                currentWaveIndex++;
                countdown = wavesToSpawn[currentWaveIndex].TimeBetweenSpawn;
            }   
            else 
            {
                hasFinishedSpawning = true;
                return;
            }    
        }

        countdown -= Time.deltaTime;
        EnemyWave wave = wavesToSpawn[currentWaveIndex];
        if (countdown <= 0 && !wave.WaveFinished  && zombieManager.CanSpawnNewZombie) 
        {
            EnemyAI enemy = Instantiate(GetZombiePrefabByType(wave.EnemyType), transform.position, Quaternion.identity, transform.root).GetComponent<EnemyAI>();
            enemy.Initialize(player, zombieManager);
            wave.Counter++;
            countdown = wave.TimeBetweenSpawn;

            zombieManager.AddZombie(enemy.gameObject);
        } 
    }

    private void CheckTestZombieInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            EnemyAI enemy = Instantiate(devilPrefab, transform.position, Quaternion.identity, transform.root).GetComponent<EnemyAI>();
            enemy.Initialize(player, zombieManager);
        }
    }

    private GameObject GetZombiePrefabByType(EnemyType type) 
    {
        if (type == EnemyType.Zombie)
            return zombiePrefab;
        else
            return
                devilPrefab;
    }
}
