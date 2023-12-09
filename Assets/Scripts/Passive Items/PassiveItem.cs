using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemScriptableObject passiveItemStats;

    protected virtual void ApplyMultiplier()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyMultiplier();
    }

}
