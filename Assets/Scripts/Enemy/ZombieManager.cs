using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    private List<GameObject> zombies;

    [Tooltip("The max amount of zombies that can pe present at the same time.")]
    public int maxAmountOfZombies;

    // Start is called before the first frame update
    void Start()
    {
        zombies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            Debug.Log(zombies.Count);
    }

    public void AddZombie(GameObject zombie) 
    {
        zombies.Add(zombie);
    }

    public void RemoveZombie(GameObject zombie) 
    {
        zombies.Remove(zombie);
    }

    public int GetCurrentZombieCount => zombies.Count;

    public bool CanSpawnNewZombie => zombies.Count < maxAmountOfZombies;

    public void PlayHitSound() 
    {
        //TODO PLay Zombie Sound effect
    }
}
