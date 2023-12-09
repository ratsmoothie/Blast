using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Enemy spawner references https://www.youtube.com/watch?v=h2cg4ucDuWw&t=7s

public class EnemySpawner : MonoBehaviour
{
    [Header("Components")]
    Transform player;

    [System.Serializable]
    //handles clusters of enemies of a certain type
    public class EnemyGroup
    {
        public string enemyName;
        public int desiredNumberOfEnemies;
        public int enemiesSpawned;
        public GameObject enemyPrefab;
    }

    //spawn handler
    [System.Serializable]
    public class Wave
    {
        public string name;
        public List<EnemyGroup> enemyGroups; //the groups of enemies currently in existence
        public int desiredNumberOfEnemies;
        public float spawnInterval;
        public int enemiesSpawned;
    }

    [Header("Wave Handling")]
    public List<Wave> waves; //list of all the waves of enemies in the game currently
    public int currentWaveNumber; //how many waves have been spawned

    [Header("Spawner")]
    float spawnTimer;
    public int currentEnemiesAlive;
    public int maxEnemiesAlive;
    public bool isMaxEnemiesAlive = false;
    public float secondsBetweenEachWave;

    bool isWaveActive = false;

    [Header("Spawn Points")]
    public List<Transform> relativeSpawnPoints;

    void HowManyEnemiesShouldWeSpawn()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveNumber].enemyGroups)
        {
            currentWaveQuota += enemyGroup.desiredNumberOfEnemies;
        }

        waves[currentWaveNumber].desiredNumberOfEnemies = currentWaveQuota;
        //Testing
        //Debug.Log(currentWaveQuota);
    }

    //come back to this and make it prettier
    void SpawnEnemies()
    {
        if (waves[currentWaveNumber].enemiesSpawned < waves[currentWaveNumber].desiredNumberOfEnemies)
        {
            foreach(var enemyGroup in waves[currentWaveNumber].enemyGroups)
            {
                //makes sure enough of each type of enemy are spawned in each wave
                if(enemyGroup.enemiesSpawned < enemyGroup.desiredNumberOfEnemies) 
                {
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.enemiesSpawned++;
                    waves[currentWaveNumber].enemiesSpawned++;
                    currentEnemiesAlive++;

                    if(currentEnemiesAlive >= maxEnemiesAlive)
                    {
                        isMaxEnemiesAlive = true;
                        return;
                    }                   
                }
            }
        }      
    }

    public void OnEnemyKilled()
    {
        currentEnemiesAlive--;

        if(currentEnemiesAlive < maxEnemiesAlive)
        {
            isMaxEnemiesAlive = false;
        }
    }

    void SpawnTiming()
    {
        if((currentWaveNumber < waves.Count) && (waves[currentWaveNumber].enemiesSpawned == 0) && (!isWaveActive))
        {
            StartCoroutine(MoveToNextWave());
        }


        spawnTimer += Time.deltaTime;

        if(spawnTimer >= waves[currentWaveNumber].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator MoveToNextWave()
    {
        isWaveActive = true;

        yield return new WaitForSeconds(secondsBetweenEachWave);

        if(currentWaveNumber < waves.Count - 1) //count - 1 so that currentWaveNumber matches index
        {
            isWaveActive = false;
            currentWaveNumber++;
            HowManyEnemiesShouldWeSpawn();
        }
    }

    void FirstWave()
    {
        if (currentWaveNumber < waves.Count && waves[currentWaveNumber].enemiesSpawned == 0)
        {
            StartCoroutine(MoveToNextWave());
        }

        spawnTimer = 0f;
        SpawnEnemies();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        HowManyEnemiesShouldWeSpawn();
        FirstWave();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnTiming();
    }
}
