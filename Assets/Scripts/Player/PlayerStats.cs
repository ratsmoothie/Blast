using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Referenced https://www.youtube.com/watch?v=qREiQ5vSAng

public class PlayerStats : MonoBehaviour
{

    public CharacterScriptableObject characterStats;

    //player's current stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentHealthRegen;
    [HideInInspector]
    public float currentMovementSpeed;
    [HideInInspector]
    public float currentAttackPower;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnetRange;

    public List<GameObject> playerWeapons;

    [Header("XP & Levelling")]
    public int xp = 0;
    public int level = 1;
    public int xpCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int xpCapIncrease;
    }

    //Invincibility frames to trigger after the player takes damage
    //stops damage from ticking every frame and helps with general fun factor
    [Header("Invincibility Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    void Awake()
    {
        characterStats = CharacterSelector.GetStats();
        CharacterSelector.instance.DestroySingleton();

        currentHealth = characterStats.MaxHealth;
        currentHealthRegen = characterStats.HealthRegen;
        currentMovementSpeed = characterStats.MovementSpeed;
        currentAttackPower = characterStats.AttackPower;
        currentProjectileSpeed = characterStats.ProjectileSpeed;
        currentMagnetRange = characterStats.MagnetRange;

        SpawnWeapon(characterStats.StartingWeapon);
    }

    // Start is called before the first frame update
    void Start()
    {
        xpCap = levelRanges[0].xpCapIncrease;
    }

    void Update()
    {
        //ensures the player can only take one instance of damage at a time
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible == true)
        {
            isInvincible = false;
        }

        HealthRegen();
    }

    public void IncreaseXP(int xpGain)
    {
        xp += xpGain;
        
        Debug.Log(xp + " experience gained!");
        if(xp >= xpCap)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        xp -= xpCap;

        int xpCapIncrease = 0;
        foreach (LevelRange range in levelRanges)
        {
            if(level >= range.startLevel && level <= range.endLevel)
            {
                xpCapIncrease = range.xpCapIncrease;
                break;
            }
        }

        xpCap += xpCapIncrease;
        Debug.Log("Level Up! Player is now level " + level);
    }

    public void TakeDamage(float damageTaken)
    {
        //if the player does not currently have i-frames, take damage as usual
        if(isInvincible == false)
        {
            currentHealth -= damageTaken;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if(currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("You died. ");
        //Destroy(this.gameObject);
    }

    public void Heal(float healthRestored)
    {
        //only heal if our current health is less than max, and dont go over max health either
        if(currentHealth < characterStats.MaxHealth)
        {
            currentHealth += healthRestored;

            if(currentHealth > characterStats.MaxHealth)
            {
                currentHealth = characterStats.MaxHealth;
            }
        }
    }

    void HealthRegen()
    {
        //only heal if our current health is less than max, and dont go over max health either
        if(currentHealth < characterStats.MaxHealth)
        {
            currentHealth += currentHealthRegen * Time.deltaTime;

            if(currentHealth > characterStats.MaxHealth)
            {
                currentHealth = characterStats.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        playerWeapons.Add(spawnedWeapon);
    }

}
