using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkPassiveItem : PassiveItem
{
    protected override void ApplyMultiplier()
    {
        player.currentAttackPower *= (1 + (passiveItemStats.Multiplier / 100f));
    }
}
