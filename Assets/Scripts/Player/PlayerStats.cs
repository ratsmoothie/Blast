using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Referenced https://www.youtube.com/watch?v=qREiQ5vSAng

public class PlayerStats : MonoBehaviour
{

    public CharacterScriptableObject characterStats;

    //player's current stats
    //[HideInInspector]
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

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    [Header("UI")]
    public Image healthBar;
    public Image xpBar;
    public Text levelText;


    public GameObject secondWeaponTest;
    public GameObject firstPassiveItemTest, secondPassiveItemTest;

    void Awake()
    {
        characterStats = CharacterSelector.GetStats();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        currentHealth = characterStats.MaxHealth;
        currentHealthRegen = characterStats.HealthRegen;
        currentMovementSpeed = characterStats.MovementSpeed;
        currentAttackPower = characterStats.AttackPower;
        currentProjectileSpeed = characterStats.ProjectileSpeed;
        currentMagnetRange = characterStats.MagnetRange;

        SpawnWeapon(characterStats.StartingWeapon);

        //Test code for inventory
        //SpawnWeapon(secondWeaponTest);
        //SpawnPassiveItem(firstPassiveItemTest);
        //SpawnPassiveItem(secondPassiveItemTest);
    }

    // Start is called before the first frame update
    void Start()
    {
        xpCap = levelRanges[0].xpCapIncrease;

        GameManager.instance.ShowCharacterInResults(characterStats);

        UpdateHealthBar();
        UpdateXPBar();
        UpdateLevelText();
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
        UpdateHealthBar();
        UpdateXPBar();
        UpdateLevelText();
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

        GameManager.instance.StartLevelUp();
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
        if(!GameManager.instance.isGameOver)
        {
            GameManager.instance.ShowLevelInResults(level);
            GameManager.instance.ShowWeaponsAndItemsInResults(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();
            Debug.Log("You died. ");
        }
        
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
        if(weaponIndex >= (inventory.weaponSlots.Count - 1))
        {
            Debug.LogError("Inventory full");
            return;
        }
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if(passiveItemIndex >= (inventory.passiveItemSlots.Count - 1))
        {
            Debug.LogError("Inventory full");
            return;
        }
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = (currentHealth / characterStats.MaxHealth);
    }

    void UpdateXPBar()
    {
        xpBar.fillAmount = (float)xp / xpCap;
    }

    void UpdateLevelText()
    {
        levelText.text = "LVL " + level.ToString();
    }

}
