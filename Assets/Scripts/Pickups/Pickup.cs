using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //once the gem touches the player, it disappears
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            //play pickup sound here
           Destroy(gameObject); 
        }
    }
}
