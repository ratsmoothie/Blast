using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceGem : Pickup, InterfacePickups
{

    public int xpGiven;

    public void Pickup()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        if (player != null)
        {
            player.IncreaseXP(xpGiven);
        }
        else
        {
            Debug.LogWarning("PlayerStats not found.");
        }
    }

}
