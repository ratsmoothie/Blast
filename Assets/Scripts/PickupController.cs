using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public GameObject itemPrefab;
        public string itemName;       
        public float dropRate;
    }

    public List<Drops> drops;

    void OnDestroy()
    {
        //prevents living enemies from creating an error when stopping play
        if(!gameObject.scene.isLoaded)
        {
            return;
        }

        float roll = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach(Drops itemToDrop in drops)
        {
            if(roll <= itemToDrop.dropRate)
            {
                possibleDrops.Add(itemToDrop);
            }
        }
        //check possible drops, drop only 1
        if(possibleDrops.Count > 0)
        {
            Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }
       
    }

}
