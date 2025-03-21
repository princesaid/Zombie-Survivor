using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<ZombieGroup> zombieGroups;

        public int waveQuote; // wave maximum nuber of zombies
        public float spawnInterval; // how may sconds before spawn new zombies
        public int spawnCount; // how many zombies spawned
    }

    [System.Serializable]
    public class ZombieGroup
    {
        public string zombieName;
        public int zombieCount;
        public int spawnCount;
        public GameObject zombiePrefab;

    }



    public List<Wave> waves;
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    public int zombiesAlive;
    public int maxZombiesAllowed;
    public bool isZombiesCountMaxed = false;
    float spawnTimer;
    public float waveInterval;
    bool isWaveActive = false;

    public List<Transform> relativeSpawnPoints; // List to store all zombies spawn possibl

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuote();


    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)
        {

            StartCoroutine(StartNewWave());
        }


        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0;
            SpawnZombies();
        }


    }

    IEnumerator StartNewWave()
    {
        isWaveActive = true;
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuote();
        }

    }
    void CalculateWaveQuote()
    {
        int currentWaveQuota = 0;
        foreach (var zombieGroup in waves[currentWaveCount].zombieGroups)
        {
            currentWaveQuota += zombieGroup.zombieCount;
        }
        waves[currentWaveCount].waveQuote = currentWaveQuota;
    }
    void SpawnZombies()
    {

        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuote && !isZombiesCountMaxed)
        {
            foreach (var zombieGroup in waves[currentWaveCount].zombieGroups)
            {
                if (zombieGroup.spawnCount < zombieGroup.zombieCount)
                {

                    Instantiate(zombieGroup.zombiePrefab, player.position + relativeSpawnPoints[UnityEngine.Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    zombieGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    zombiesAlive++;

                    if (zombiesAlive >= maxZombiesAllowed)
                    {
                        isZombiesCountMaxed = true;
                        return;
                    }
                }

            }

        }

    }

    public void OnZombieKilled()
    {
        zombiesAlive--;
        // Reset the maxZombiesAllowed flag if the number of zombies alive has dropped below the max amount
        if (zombiesAlive < maxZombiesAllowed)
        {
            isZombiesCountMaxed = false;
        }
    }
}
