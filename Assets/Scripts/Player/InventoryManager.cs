using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Referenced https://www.youtube.com/watch?v=lDSPgvu9IQc&list=PLgXA5L5ma2Bveih0btJV58REE2mzfQLOQ&index=8

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(5);
    public int[] weaponLevels = new int[5];
    public List<Image> weaponUISlots = new List <Image>(5);

    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(5);
    public int[] passiveItemLevels = new int[5];
    public List<Image> passiveItemUISlots = new List <Image>(5);

    [System.Serializable]
    public class UpgradeWeapon
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponStats;
    }

    [System.Serializable]
    public class UpgradePassiveItem
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemStats;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<UpgradeWeapon> upgradeWeaponOptions = new List<UpgradeWeapon>();
    public List<UpgradePassiveItem> upgradePassiveItemOptions = new List<UpgradePassiveItem>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponStats.WeaponLevel;
        weaponUISlots[slotIndex].enabled = true; //keeps ui clean
        weaponUISlots[slotIndex].sprite = weapon.weaponStats.WeaponSprite;

        //After the player chooses an item end the levelling up state
        if (GameManager.instance != null && GameManager.instance.choosingItem)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemStats.PassiveItemLevel;
        passiveItemUISlots[slotIndex].enabled = true; //keeps ui clean
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemStats.PassiveItemSprite;

        //After the player chooses an item end the levelling up state
        if (GameManager.instance != null && GameManager.instance.choosingItem)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if(weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if(!weapon.weaponStats.NextLevelPrefab)
            {
                Debug.LogError(weapon.name + " is already max level. ");
                return;
            }

            GameObject upgradedWeapon = Instantiate(weapon.weaponStats.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponStats.WeaponLevel;

            upgradeWeaponOptions[upgradeIndex].weaponStats = upgradedWeapon.GetComponent<WeaponController>().weaponStats;

            //After the player chooses an item end the levelling up state
            if (GameManager.instance != null && GameManager.instance.choosingItem)
            {
                GameManager.instance.EndLevelUp();
            }
            
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if(passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if(!passiveItem.passiveItemStats.NextLevelPrefab)
            {
                Debug.LogError(passiveItem.name + " is already max level. ");
                return;
            }

            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemStats.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemStats.PassiveItemLevel;

            upgradePassiveItemOptions[upgradeIndex].passiveItemStats = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemStats;

            //After the player chooses an item end the levelling up state
            if (GameManager.instance != null && GameManager.instance.choosingItem)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        List<UpgradeWeapon> availableWeaponUpgradeOptions = new List<UpgradeWeapon>(upgradeWeaponOptions);
        List<UpgradePassiveItem> availablePassiveItemUpgradeOptions = new List<UpgradePassiveItem>(upgradePassiveItemOptions);

        foreach(var upgradeOption in upgradeUIOptions)
        {
            if(availableWeaponUpgradeOptions.Count == 0 && availableWeaponUpgradeOptions.Count == 0)
            {
                return;
            }

            int upgradeType;

            if(availableWeaponUpgradeOptions.Count == 0)
            {
                upgradeType = 2;
            }
            else if(availablePassiveItemUpgradeOptions.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = Random.Range(1, 3);
            }

            if(upgradeType == 1)
            {
                UpgradeWeapon weaponUpgradeChosen = availableWeaponUpgradeOptions[Random.Range(0, availableWeaponUpgradeOptions.Count)];

                availableWeaponUpgradeOptions.Remove(weaponUpgradeChosen);

                if(weaponUpgradeChosen != null)
                {
                    //only the necessary upgrade options will show in the ui
                    EnableUpgradeUI(upgradeOption);

                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if(weaponSlots[i] != null && weaponSlots[i].weaponStats == weaponUpgradeChosen.weaponStats)
                        {
                            newWeapon = false;
                            if(!newWeapon)
                            {
                                if(!weaponUpgradeChosen.weaponStats.NextLevelPrefab)
                                {
                                    //if a weapon is at max level dont try to upgrade it
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, weaponUpgradeChosen.weaponUpgradeIndex));

                                //Set the name and the description to the weapon's next level's description and name
                                upgradeOption.upgradeDescriptionDisplay.text = weaponUpgradeChosen.weaponStats.NextLevelPrefab.GetComponent<WeaponController>().weaponStats.Description;
                                upgradeOption.upgradeNameDisplay.text = weaponUpgradeChosen.weaponStats.NextLevelPrefab.GetComponent<WeaponController>().weaponStats.Name;
                            }

                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }

                    if(newWeapon) //Spawn a weapon the player does not yet have
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(weaponUpgradeChosen.initialWeapon));

                        //Set initial name and description for a weapon
                        upgradeOption.upgradeDescriptionDisplay.text = weaponUpgradeChosen.weaponStats.Description;
                        upgradeOption.upgradeNameDisplay.text = weaponUpgradeChosen.weaponStats.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = weaponUpgradeChosen.weaponStats.WeaponSprite;
                }
            }
            else if(upgradeType == 2)
            {
                UpgradePassiveItem passiveItemUpgradeChosen = availablePassiveItemUpgradeOptions[Random.Range(0, availablePassiveItemUpgradeOptions.Count)];

                availablePassiveItemUpgradeOptions.Remove(passiveItemUpgradeChosen);

                if(passiveItemUpgradeChosen != null)
                {
                    //only the necessary upgrade options will show in the ui
                    EnableUpgradeUI(upgradeOption);

                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if(passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemStats == passiveItemUpgradeChosen.passiveItemStats)
                        {
                            newPassiveItem = false;

                            if(!newPassiveItem)
                            {
                                if(!passiveItemUpgradeChosen.passiveItemStats.NextLevelPrefab)
                                {
                                    //if there is no next level dont try to upgrade it
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, passiveItemUpgradeChosen.passiveItemUpgradeIndex));
                                
                                //Show the description and name for upgrading a passive item
                                upgradeOption.upgradeDescriptionDisplay.text = passiveItemUpgradeChosen.passiveItemStats.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemStats.Description;
                                upgradeOption.upgradeNameDisplay.text = passiveItemUpgradeChosen.passiveItemStats.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemStats.Name;
                            }
                            break;               
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }

                    if(newPassiveItem)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(passiveItemUpgradeChosen.initialPassiveItem));

                        //Set initial name and description for passive item
                        upgradeOption.upgradeDescriptionDisplay.text = passiveItemUpgradeChosen.passiveItemStats.Description;
                        upgradeOption.upgradeNameDisplay.text = passiveItemUpgradeChosen.passiveItemStats.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = passiveItemUpgradeChosen.passiveItemStats.PassiveItemSprite;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach(var upgradeOptions in upgradeUIOptions)
        {
            upgradeOptions.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOptions);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    public void DisableUpgradeUI(UpgradeUI ui)
    {
        //go all the way to the top baby
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    public void EnableUpgradeUI(UpgradeUI ui)
    {
        //go all the way to the top baby
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }

    void Awake()
    {
        player = GetComponent<PlayerStats>();
    }
}
