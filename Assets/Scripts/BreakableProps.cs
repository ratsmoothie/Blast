using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float propHealth;
    public AudioSource breakSound;

    public void TakeDamage(float damageTaken)
    {
        propHealth -= damageTaken;

        if (propHealth <= 0)
        {
            DestroyProp();
        }
    }

    public void DestroyProp()
    {
        Destroy(this.gameObject);
        breakSound.Play();
    }
}
