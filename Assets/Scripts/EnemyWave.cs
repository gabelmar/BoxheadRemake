using System;
using UnityEngine;

[Serializable]
public class EnemyWave
{
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private int amount;
    [SerializeField]
    private float timeBetweenSpawn;

    [HideInInspector]
    public int Counter = 0;
    public EnemyType EnemyType => enemyType;
    public float TimeBetweenSpawn => timeBetweenSpawn;
    public bool WaveFinished => Counter == amount;
}
