using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Referenced this video for world generation and prop spawning (ie: treasure chests)
//https://www.youtube.com/watch?v=QN8dm0RD3mY&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=2
public class PropRandomizer : MonoBehaviour
{
    public List<GameObject> propSpawnPoints;
    public List<GameObject> propPrefabs;
    // Start is called before the first frame update
    void Start()
    {
        PropSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PropSpawner()
    {
  
        foreach(GameObject spawnPoint in propSpawnPoints)
        {
            int randomPropSelector = Random.Range(0, propPrefabs.Count);
            GameObject prop = Instantiate(propPrefabs[randomPropSelector], spawnPoint.transform.position, Quaternion.identity);
            prop.transform.parent = spawnPoint.transform;
        }

    }
}
