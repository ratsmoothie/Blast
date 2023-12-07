using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Referenced https://www.youtube.com/watch?v=QN8dm0RD3mY&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=2
//for chunk spawning and generation
public class MapGenerator : MonoBehaviour
{
    [Header("Components")]
    public GameObject playerObject;   
    public LayerMask BackgroundMask;
    PlayerController playerController;

    [Header("Terrain Chunks")]
    public float RadiusToCheckForChunk;
    public List<GameObject> terrainChunksList;      
    public float chunkSize;
    public GameObject currentChunk;
    private Vector3 chunkSpawnPoint;

    [Header("Performance/Memory Optimizing")]
    public List<GameObject> spawnedTerrainChunks;
    GameObject lastSpawnedChunk;
    public float distanceToDisableChunk;
    private float distanceToChunk;
    float chunkOptimizerCooldown;
    public float chunkOptimizerCooldownDuration;

    // Start is called before the first frame update
    void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ShouldWeSpawnAChunk();
        ChunkPerformanceOptimizer();
    }

    void ShouldWeSpawnAChunk()
    {

        if (currentChunk == null)
        {
            return;
        }

        //get the chunks childed spawn points
        Transform chunkSpawnPoints = currentChunk.transform.Find("Chunk Spawn Points");

        //find them based on their names
        Transform right      = chunkSpawnPoints.Find("Right Spawn Point");
        Transform left       = chunkSpawnPoints.Find("Left Spawn Point");
        Transform up         = chunkSpawnPoints.Find("Up Spawn Point");
        Transform down       = chunkSpawnPoints.Find("Down Spawn Point");
        Transform upperRight = chunkSpawnPoints.Find("Upper Right Spawn Point");
        Transform lowerRight = chunkSpawnPoints.Find("Lower Right Spawn Point");
        Transform upperLeft  = chunkSpawnPoints.Find("Upper Left Spawn Point");
        Transform lowerLeft  = chunkSpawnPoints.Find("Lower Left Spawn Point");

        //set each relevant spawnPoint
        Vector3 rightSpawnPoint      = right.position;
        Vector3 leftSpawnPoint       = left.position;
        Vector3 upSpawnPoint         = up.position;
        Vector3 downSpawnPoint       = down.position;
        Vector3 upperRightSpawnPoint = upperRight.position;
        Vector3 lowerRightSpawnPoint = lowerRight.position;
        Vector3 upperLeftSpawnPoint  = upperLeft.position;
        Vector3 lowerLeftSpawnPoint  = lowerLeft.position;

        //player is going right and not going up/down
        if (playerController.movementX > 0 && playerController.movementY == 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(rightSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = rightSpawnPoint;
                ChunkSpawner(rightSpawnPoint);
            }
        }
        //player is going left and not going up/down
        else if (playerController.movementX < 0 && playerController.movementY == 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(leftSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = leftSpawnPoint;
                ChunkSpawner(leftSpawnPoint);
            }
        }
        //up
        else if (playerController.movementX == 0 && playerController.movementY > 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(upSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = upSpawnPoint;
                ChunkSpawner(upSpawnPoint);
            }
        }
        //down
        else if (playerController.movementX == 0 && playerController.movementY < 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(downSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = downSpawnPoint;
                ChunkSpawner(downSpawnPoint);
            }
        }
        //right + up
        else if (playerController.movementX > 0 && playerController.movementY > 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(upperRightSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = upperRightSpawnPoint;
                ChunkSpawner(upperRightSpawnPoint);
            }
        }
        //right + down
        else if (playerController.movementX > 0 && playerController.movementY < 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(lowerRightSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = lowerRightSpawnPoint;
                ChunkSpawner(lowerRightSpawnPoint);
            }
        }
        //left + up
        else if (playerController.movementX < 0 && playerController.movementY > 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(upperLeftSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = upperLeftSpawnPoint;
                ChunkSpawner(upperLeftSpawnPoint);
            }
        }
        //left + down
        else if (playerController.movementX < 0 && playerController.movementY < 0)
        {
            //only spawn a chunk if there is not a chunk in the spot our circle overlaps
            if (!Physics2D.OverlapCircle(lowerLeftSpawnPoint, RadiusToCheckForChunk, BackgroundMask))
            {
                chunkSpawnPoint = lowerLeftSpawnPoint;
                ChunkSpawner(lowerLeftSpawnPoint);
            }
        }
    }

    void ChunkSpawner(Vector3 chunkSpawnPoint)
    {
        int randomChunkSelector = Random.Range(0, terrainChunksList.Count);
        Instantiate(terrainChunksList[randomChunkSelector], chunkSpawnPoint, Quaternion.identity);
    }

    //checks if we're far enough away to disable the respective chunk
    void ChunkPerformanceOptimizer()
    {
        chunkOptimizerCooldown -= Time.deltaTime;

        if(chunkOptimizerCooldown <= 0)
        {
            chunkOptimizerCooldown = chunkOptimizerCooldownDuration;
        }
        else
        {
            return;
        }
        foreach (GameObject chunk in spawnedTerrainChunks)
        {
            distanceToChunk = Vector3.Distance(playerObject.transform.position, chunk.transform.position);
            if(distanceToChunk > distanceToDisableChunk)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
