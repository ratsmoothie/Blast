using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Referenced https://www.youtube.com/watch?v=QN8dm0RD3mY&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=2
public class ChunkSpawnTrigger : MonoBehaviour
{
    MapGenerator mapGenerator;
    public GameObject targetChunk; 

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            //Debug.Log("Player entered trigger zone.");
            mapGenerator.currentChunk = targetChunk;
        }
    }

    private void OnTriggerStay2D(Collider2D playerCollider)
    {
        if(playerCollider.CompareTag("Player"))
        {
            mapGenerator.currentChunk = targetChunk;
        }
    }

    private void OnTriggerExit2D(Collider2D playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            //Debug.Log("Player exited trigger zone");
            if (mapGenerator.currentChunk == targetChunk)
            {
                mapGenerator.currentChunk = null;
            }
        }
    }   
}
