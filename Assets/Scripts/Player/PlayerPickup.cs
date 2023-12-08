using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{

    PlayerStats player;
    CircleCollider2D playerPickup;
    public float pullForce;

    void Awake()
    {
        player = FindObjectOfType<PlayerStats>();
        playerPickup = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        playerPickup.radius = player.currentMagnetRange;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent(out InterfacePickups pickup))
        {
            //pull the item towards the player
            Rigidbody2D itemRb = collider.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = transform.position - collider.transform.position;
            itemRb.AddForce(forceDirection * pullForce);

            //grab the pickup
            pickup.Pickup();
        }
    }
}
