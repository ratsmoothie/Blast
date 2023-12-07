using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    public WeaponScriptableObject weaponStats;
    
    public float durationUntilDestroy;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, durationUntilDestroy);
    }

}
