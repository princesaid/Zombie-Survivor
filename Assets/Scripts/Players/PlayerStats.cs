using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;

    //[HideInInspector]
    float currentHealth;

    float currentRecovery;

    float currentMoveSpeed;

    float currentMight;

    float currentProjectileSpeed;

    float currentMagnet;


    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {

            if (currentHealth != value)
            {
                currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }

    }
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {

            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
            }
        }

    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {

            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }

            }
        }

    }
    public float CurrentMight
    {
        get { return currentMight; }
        set
        {

            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Migh: " + currentMight;
                }
            }
        }

    }
    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {

            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }

    }
    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {

            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
            }
        }

    }

    #endregion



    // eperience and level of the player

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //i-frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    [Header("UI")]
    public Image healthBar;
    public Image experienceBar;
    public TextMeshProUGUI leveltext;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;


    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;

    }



    void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentMight = characterData.Might;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentRecovery = characterData.Recvery;
        CurrentMagnet = characterData.Magnet;

        SpawnWeapon(characterData.StartingWeapon);
    }

    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
        Recover();

    }
    void UpdateExperienceBar()
    {
        // update expereince bar fill amount
        experienceBar.fillAmount = (float)experience / experienceCap;

    }
    void UpdateLevelText()
    {
        leveltext.text = "Level" + level.ToString();

    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;

        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Migh: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
        UpdateHealthBar();
        UpdateExperienceBar();
        UpdateLevelText();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();

        UpdateExperienceBar();
    }
    void LevelUpChecker()
    {

        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelText();

            GameManager.instance.StartLevelUP();
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            CurrentHealth -= damage;
            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            if (CurrentHealth <= 0)
            {
                Kill();
            }
            UpdateHealthBar();
        }


    }
    void UpdateHealthBar()
    {
        // update health bar
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }
    
    public void Kill()
    {
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.GameOver();
        }

        //Destroy(gameObject);
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;
            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }

    }
    void Recover()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;
        }
        if (CurrentHealth > characterData.MaxHealth)
        {
            CurrentHealth = characterData.MaxHealth;
        }
    }
    public void SpawnWeapon(GameObject weapon)
    {
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("weapons Inventory slots already full");
            return;

        }
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);

        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());
        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("passive Item Inventory slots already full");
            return;

        }
        GameObject spawnedpassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedpassiveItem.transform.SetParent(transform);

        inventory.AddPassiveItem(passiveItemIndex, spawnedpassiveItem.GetComponent<PassiveItem>());
        passiveItemIndex++;
    }






}
