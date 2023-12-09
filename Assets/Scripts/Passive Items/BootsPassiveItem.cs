using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsPassiveItem : PassiveItem
{
    protected override void ApplyMultiplier()
    {
        player.currentMovementSpeed *= (1 + (passiveItemStats.Multiplier / 100f));
    }
}
