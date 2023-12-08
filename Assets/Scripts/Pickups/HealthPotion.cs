using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Pickup, InterfacePickups
{
    public int healthRestored;

    public void Pickup()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.Heal(healthRestored);
    }
}
