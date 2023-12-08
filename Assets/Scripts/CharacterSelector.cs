using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    public CharacterScriptableObject characterStats;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogWarning("Extra " + this + " deleted");
            Destroy(this.gameObject);
        }
    }
    
    public static CharacterScriptableObject GetStats()
    {
        return instance.characterStats;
    }

    public void SelectCharacter(CharacterScriptableObject character)
    {
        characterStats = character;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(this.gameObject);
    }
}
