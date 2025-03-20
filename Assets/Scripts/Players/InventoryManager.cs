using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public List<Image> passiveItemUISlots = new List<Image>(6);
    public int[] passiveItemLevels = new int[6];

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;


    }
    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialWeapon;
        public PassiveItemScriptableObject passiveItemData;

    }
    [System.Serializable]
    public class UpgradeUI
    {
        public TextMeshProUGUI upgradeNameDisplay;
        public TextMeshProUGUI upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;



    }
    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }



    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null && GameManager.instance.canUpgrade)
        {
            GameManager.instance.EndLevelUP();
        }
    }


    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;
        if (GameManager.instance != null && GameManager.instance.canUpgrade)
        {
            GameManager.instance.EndLevelUP();
        }
    }
    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("No Next level for" + weapon.name);
                return;

            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

            if (GameManager.instance != null && GameManager.instance.canUpgrade)
            {
                GameManager.instance.EndLevelUP();
            }
        }

    }
    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.LogError("No Next level for" + passiveItem.name);
                return;

            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;
            passiveItemUpgradeOptions[upgradeIndex].passiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData;

            if (GameManager.instance != null && GameManager.instance.canUpgrade)
            {
                GameManager.instance.EndLevelUP();
            }
        }

    }

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgade = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);


        foreach (var upgradeOption in upgradeUIOptions)
        {


            if (availablePassiveItemUpgade.Count == 0 && availableWeaponUpgrades.Count == 0)
            {
                //DisableUpgradeUI(upgradeOption);
                return;
            }
            int upgradeType;
            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;

            }
            else if (availablePassiveItemUpgade.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = Random.Range(1, 3);
            }

            // = Random.Range(1, 3);
            if (upgradeType == 1)
            {
                WeaponUpgrade chooseWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chooseWeaponUpgrade);

                if (chooseWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chooseWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!chooseWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;

                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chooseWeaponUpgrade.weaponUpgradeIndex));
                                upgradeOption.upgradeDescriptionDisplay.text = chooseWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chooseWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chooseWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chooseWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chooseWeaponUpgrade.weaponData.name;

                    }

                    upgradeOption.upgradeIcon.sprite = chooseWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chossenPassiveItemUpgrade = availablePassiveItemUpgade[Random.Range(0, availablePassiveItemUpgade.Count)];
                availablePassiveItemUpgade.Remove(chossenPassiveItemUpgrade);
                if (chossenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chossenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false;
                            if (!newPassiveItem)
                            {
                                if (!chossenPassiveItemUpgrade.passiveItemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;

                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chossenPassiveItemUpgrade.passiveItemUpgradeIndex));
                                upgradeOption.upgradeDescriptionDisplay.text = chossenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chossenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                            }
                            break;

                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chossenPassiveItemUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chossenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chossenPassiveItemUpgrade.passiveItemData.name;

                    }
                    upgradeOption.upgradeIcon.sprite = chossenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }



        }

    }
    void RemoveUpgradeOption()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }

    }
    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOption();
        ApplyUpgradeOptions();

    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);

    }
    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);

    }

}
