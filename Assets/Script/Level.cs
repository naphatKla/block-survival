using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class Level : Singleton<Level>
{
    #region Declare Variables
    
    public float playerLevel;
    public float playerExp;
    public float playerNextLevelUpExp;
    public float playerLevelUpPoint;
    public float lootChestPickPoint;
    public float enemyKill;
    
    [Header("Class Data")][Space]
    public PlayerClassData defaultData;
    public PlayerClassData assaultRifleData;
    public PlayerClassData shotgunData;
    public PlayerClassData sniperData;
    public PlayerClassData missileData;
    public PlayerClassData swordData;
    public LootChestData lootChestData;
    
    [Header("UI")][Space]
    [SerializeField] private Scrollbar playerLevelBar;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private LootChest lootChest;
    [SerializeField] private PlayerLevelUp playerLevelUp;
    [SerializeField] private ClassTypeSelect classTypeSelect;
    [SerializeField] private Block blockImage;
    [SerializeField] private AudioSource levelUpSoundEffect;
    public TextMeshProUGUI playerLevelText;
    
    [Header("Other")]
    private CombatSystem _combatSystem;
    private Player _player;
    private GameManager _gameManager;
    
    [Serializable] public struct PlayerStatus
    {
        public Image playerStatusUI;
        public Button statusButton;
        public TextMeshProUGUI statusText;
        public TextMeshProUGUI enemyKillText;
    }
    
    [Serializable] public struct LootChest
    {
        [Header("UI")]
        public Image lootChestUI;
        public Button healthButton;
        public Button damageButton;
        public Button attackSpeedButton;
        public Button speedButton;
        public Button dashButton;
        public Button skipButton;
        [Space]
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI damageText;
        public TextMeshProUGUI attackSpeedText;
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI dashText;
        public TextMeshProUGUI lootChestPointText;

        [HideInInspector] public Animator animator;
    }
    [Serializable] public struct ClassTypeSelect
    {
        [Header("UI")]
        public Image classSelectUI;
        public Button assaultRifleButton;
        public Button shotGunButton;
        public Button sniperButton;
        public Button missileButton;
        public Button swordButton;
        
        [HideInInspector] public Animator animator;
    }
    [Serializable] public struct PlayerLevelUp
    {
        [Header("UI")]
        public Image levelUpUI;
        public Button healthButton;
        public Button damageButton;
        public Button attackSpeedButton;
        public Button speedButton;
        public Button skipButton;
        [Space]
        public TextMeshProUGUI levelUpPointText;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI damageText;
        public TextMeshProUGUI attackSpeedText;
        public TextMeshProUGUI speedText;
        
        [HideInInspector] public Animator animator;
    }
    [Serializable] public struct Block
    {
        public List<Image> hpBlockImages;
        public List<Image> damageBlockImages;
        public List<Image> attackSpeedBlockImages;
        public List<Image> speedBlockImages;
        public List<Image> dashBlockImages;
    }
    #endregion

    #region Unity Method
    void Start()
    {
        _player = GetComponent<Player>();
        _gameManager = FindObjectOfType<GameManager>();
        _combatSystem = GetComponent<CombatSystem>();
        lootChest.animator = lootChest.lootChestUI.GetComponent<Animator>();
        classTypeSelect.animator = classTypeSelect.classSelectUI.GetComponent<Animator>();
        playerLevelUp.animator = playerLevelUp.levelUpUI.GetComponent<Animator>();

        playerStatus.statusButton.onClick.AddListener(() =>
        {
            GameObject status = playerStatus.playerStatusUI.gameObject;
            status.SetActive(!status.activeSelf);
            playerStatus.statusButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = status.activeSelf ? "Close" : "Open";
        });
        
        ClassSelectButtonAssign();
        PlayerLevelUpButtonAssign();
        LootChestButtonAssign();
    }

    private void Update()
    {
        PlayerStatusUpdate();
    }

    private IEnumerator LootChestUIPopUp()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => classTypeSelect.classSelectUI.IsActive() == false);
        yield return new WaitUntil(() => playerLevelUp.levelUpUI.IsActive() == false);
        CheckPlayerStats();
        lootChest.lootChestUI.gameObject.SetActive(true);
        yield return new WaitUntil(() => lootChest.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        while (lootChestPickPoint > 0)
        {
            PlayerStatusUpdate();
            _player.PlayerBarUpdate();
            CheckPlayerStats();
            lootChest.lootChestPointText.text = $"{lootChestPickPoint} Chest";
            Time.timeScale = 0;
            yield return null;
        }
        
        lootChest.lootChestUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    private IEnumerator ClassSelectUIPopUp()
    {
        yield return new WaitUntil(() => lootChest.lootChestUI.IsActive() == false);
        yield return new WaitUntil(() => playerLevelUp.levelUpUI.IsActive() == false);
        classTypeSelect.classSelectUI.gameObject.SetActive(true);
        yield return new WaitUntil(() => classTypeSelect.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        while (classTypeSelect.classSelectUI.IsActive())
        {
            Time.timeScale = 0;
            yield return null;
        }
        
        playerLevelUp.healthText.text = $"+ {GetClassData(_combatSystem.playerClass).upgradeHealth} \nMax HP";
        playerLevelUp.damageText.text = $"+ {GetClassData(_combatSystem.playerClass).upgradeDamage} ATK Damage";
        playerLevelUp.attackSpeedText.text = $"- {GetClassData(_combatSystem.playerClass).upgradeAttackSpeed} ATK Cooldown";
        playerLevelUp.speedText.text = $"+ {GetClassData(_combatSystem.playerClass).upgradeSprintSpeed} Speed\n" +
                                       $"- {GetClassData(_combatSystem.playerClass).upgradeSprintStaminaDrain} Sprint Stamina";
        Time.timeScale = 1;
    }
    
    private IEnumerator LevelUpUIPopUp()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => lootChest.lootChestUI.IsActive() == false);
        yield return new WaitUntil(() => classTypeSelect.classSelectUI.IsActive() == false);
        
        CheckPlayerStats();
        playerLevelUp.levelUpUI.gameObject.SetActive(true);
        
        yield return new WaitUntil(() => playerLevelUp.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        while (playerLevelUpPoint > 0  && playerLevelUp.levelUpUI.IsActive())
        {
            PlayerStatusUpdate();
            _player.PlayerBarUpdate();
            CheckPlayerStats();
            playerLevelUp.levelUpPointText.text = $"{playerLevelUpPoint} Point";
            Time.timeScale = 0;
            yield return null;
        }
        
        playerLevelUp.levelUpUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    #endregion

    public void SetPlayerClass(CombatSystem.PlayerClass playerClass)
    {
        _player.health = GetClassData(playerClass).health;
        _player.maxHealth = GetClassData(playerClass).health;
        _player.playerDamage = GetClassData(playerClass).damage;
        _player.walkSpeed = GetClassData(playerClass).walkSpeed;
        _player.sprintSpeed = GetClassData(playerClass).sprintSpeed;
        _player.dashSpeed = GetClassData(playerClass).dashSpeed;
        _player.playerAttackSpeed = GetClassData(playerClass).attackSpeed;
        _combatSystem.bulletSpeed = GetClassData(playerClass).bulletSpeed;
        _combatSystem.playerClass = playerClass;
    }
    public PlayerClassData GetClassData(CombatSystem.PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case CombatSystem.PlayerClass.AssaultRifle:
                return assaultRifleData;
            case CombatSystem.PlayerClass.Shotgun:
                return shotgunData;
            case CombatSystem.PlayerClass.Sniper:
                return sniperData;
            case CombatSystem.PlayerClass.Missile:
                return missileData;
            case CombatSystem.PlayerClass.Sword:
                return swordData;
            default:
                return defaultData;
        }
    }
    
    public void LevelGain(float exp)
    {
        playerExp += exp;

        if (playerExp >= playerNextLevelUpExp)
            LevelUp();
    }
    
    public void LevelUp()
    {
        if (playerExp >= playerNextLevelUpExp)
        {
            playerLevel += 1;
            levelUpSoundEffect.Play();
            if(playerLevel >= 5)
                playerLevelUpPoint += 1;
            playerExp = 0;
            playerLevelBar.size = 0;
            playerNextLevelUpExp += playerNextLevelUpExp / 2.5f ;
        }

        if (playerLevel.Equals(5))
            StartCoroutine(ClassSelectUIPopUp());
        
        
        if(playerLevelUpPoint > 0 && playerLevel >= 5)
            StartCoroutine(LevelUpUIPopUp());
    }
    
    private void CheckPlayerStats()
    {
        if (_player.health >= GetClassData(_combatSystem.playerClass).maxHpLimit)
        {
            foreach (var blockImage in blockImage.hpBlockImages)
                blockImage.gameObject.SetActive(true);
        }
        else if(blockImage.hpBlockImages[0].IsActive())
        {
            foreach (var blockImage in blockImage.hpBlockImages)
                blockImage.gameObject.SetActive(false);
        }
        
        if(_player.playerDamage >= GetClassData(_combatSystem.playerClass).damageLimit)
        {
            foreach (var blockImage in blockImage.damageBlockImages)
                blockImage.gameObject.SetActive(true);
        }
        else if (blockImage.damageBlockImages[0].IsActive())
        {
            foreach (var blockImage in blockImage.damageBlockImages)
                blockImage.gameObject.SetActive(false);
        }
        
        if(_player.playerAttackSpeed <= GetClassData(_combatSystem.playerClass).attackSpeedLimit)
        {
            foreach (var blockImage in blockImage.attackSpeedBlockImages)
                blockImage.gameObject.SetActive(true);
        }
        else if (blockImage.attackSpeedBlockImages[0].IsActive())
        {
            foreach (var blockImage in blockImage.attackSpeedBlockImages)
                blockImage.gameObject.SetActive(false);
        }
        
        if(_player.sprintSpeed >= GetClassData(_combatSystem.playerClass).sprintSpeedLimit)
        {
            foreach (var blockImage in blockImage.speedBlockImages)
                blockImage.gameObject.SetActive(true);
        }
        else if (blockImage.speedBlockImages[0].IsActive())
        {
            foreach (var blockImage in blockImage.speedBlockImages)
                blockImage.gameObject.SetActive(false);
        }
        
        if(_player.dashSpeed > GetClassData(_combatSystem.playerClass).dashSpeedLimit)
        {
            foreach (var blockImage in blockImage.dashBlockImages)
                blockImage.gameObject.SetActive(true);
        }
        else if (blockImage.dashBlockImages[0].IsActive())
        {
            foreach (var blockImage in blockImage.dashBlockImages)
                blockImage.gameObject.SetActive(false);
        }
    }

    private void PlayerStatusUpdate()
    {
        playerStatus.statusText.text =
            $"Class : {_combatSystem.playerClass} \n" +
            $"Level : {playerLevel} \n" +
            $"Damage : {_player.playerDamage:F2} \n" +
            $"Attack Speed : {_player.playerAttackSpeed:F2} \n" +
            $"Speed : {_player.sprintSpeed:F2} \n" +
            $"Dash Speed : {_player.dashSpeed:F2} \n";
        

            playerStatus.enemyKillText.text = $"Enemy Kill : {enemyKill}\n" +
            $"Enemy Left : {_gameManager.enemyLeft}";
        playerLevelBar.size = playerExp / playerNextLevelUpExp;
        playerLevelText.text = $"{playerLevel}";
        playerLevelBar.value = 0;
    }
    
    public void LootChestPick()
    {
        StartCoroutine(LootChestUIPopUp());
    }
    
    private void LootChestButtonAssign()
    {
        lootChest.healthText.text = $"+ {lootChestData.upgradeHealth} \nMax HP";
        lootChest.damageText.text = $"+ {lootChestData.upgradeDamage} ATK Damage";
        lootChest.attackSpeedText.text = $"- {lootChestData.upgradeAttackSpeed} ATK Cooldown";
        lootChest.speedText.text = $"+ {lootChestData.upgradeSprintSpeed} Speed\n" +
                                   $"- {lootChestData.upgradeSprintStaminaDrain} Sprint Stamina";
        lootChest.dashText.text = $"+ {lootChestData.upgradeSprintSpeed} Dash Speed\n" +
                                  $"- {lootChestData.upgradeSprintStaminaDrain} Dash Stamina";
        
        lootChest.healthButton.onClick.AddListener(() =>
        {
            lootChestPickPoint--;
            _player.maxHealth += lootChestData.upgradeHealth;
            _player.health += lootChestData.upgradeHealth;
            _player.maxHealth = Mathf.Clamp(_player.maxHealth, 0, GetClassData(_combatSystem.playerClass).maxHpLimit);
            _player.health = Mathf.Clamp(_player.health, 0, _player.maxHealth);
        });

        lootChest.damageButton.onClick.AddListener(() =>
        {
            lootChestPickPoint--;
            _player.playerDamage += lootChestData.upgradeDamage;
            _player.playerDamage = Mathf.Clamp(_player.playerDamage, 0, GetClassData(_combatSystem.playerClass).damageLimit);
        });

        lootChest.attackSpeedButton.onClick.AddListener(() =>
        {
            lootChestPickPoint--;
            _player.playerAttackSpeed -= lootChestData.upgradeAttackSpeed;
            _player.playerAttackSpeed = Mathf.Clamp(_player.playerAttackSpeed, GetClassData(_combatSystem.playerClass).attackSpeedLimit, 5);
        });

        lootChest.speedButton.onClick.AddListener(() =>
        {
            lootChestPickPoint--;
            _player.walkSpeed += lootChestData.upgradeWalkSpeed;
            _player.sprintSpeed += lootChestData.upgradeSprintSpeed;
            _player.sprintStaminaDrain -= lootChestData.upgradeSprintStaminaDrain;
            _player.walkSpeed = Mathf.Clamp(_player.walkSpeed, 0, GetClassData(_combatSystem.playerClass).walkSpeedLimit);
            _player.sprintSpeed = Mathf.Clamp(_player.sprintSpeed, 0, GetClassData(_combatSystem.playerClass).sprintSpeedLimit);
        });

        lootChest.dashButton.onClick.AddListener(() =>
        {
            lootChestPickPoint--;
            _player.dashSpeed += lootChestData.upgradeDashSpeed;
            _player.dashStaminaDrain -= lootChestData.upgradeDashStaminaDrain;
            _player.dashSpeed = Mathf.Clamp(_player.dashSpeed, 0, GetClassData(_combatSystem.playerClass).dashSpeedLimit);
        });
        
        lootChest.skipButton.onClick.AddListener(() =>
        {
            lootChest.lootChestUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            lootChestPickPoint = 0;
        });
    }

    private void PlayerLevelUpButtonAssign()
    {
        playerLevelUp.healthText.text = $"+ {GetClassData(_combatSystem.playerClass).upgradeHealth} \nMax HP";
        playerLevelUp.damageText.text = $"+ {GetClassData(_combatSystem.playerClass).upgradeDamage} ATK Damage";
        playerLevelUp.attackSpeedText.text = $"- {GetClassData(_combatSystem.playerClass).upgradeAttackSpeed} ATK Cooldown";
        playerLevelUp.speedText.text = $"+ {GetClassData(_combatSystem.playerClass).upgradeSprintSpeed} Speed\n" +
                                       $"- {GetClassData(_combatSystem.playerClass).upgradeSprintStaminaDrain} Sprint Stamina";
        
        playerLevelUp.healthButton.onClick.AddListener(() =>
        {
            _player.maxHealth += GetClassData(_combatSystem.playerClass).upgradeHealth;
            _player.health += GetClassData(_combatSystem.playerClass).upgradeHealth;
            _player.maxHealth = Mathf.Clamp(_player.maxHealth, 0, GetClassData(_combatSystem.playerClass).maxHpLimit);
            _player.health = Mathf.Clamp(_player.health, 0, _player.maxHealth);
            playerLevelUpPoint--;
        });

        playerLevelUp.damageButton.onClick.AddListener(() =>
        {
            _player.playerDamage += GetClassData(_combatSystem.playerClass).upgradeDamage;
            _player.playerDamage = Mathf.Clamp(_player.playerDamage, 0, GetClassData(_combatSystem.playerClass).damageLimit);
            playerLevelUpPoint--;
        });

        playerLevelUp.attackSpeedButton.onClick.AddListener(() =>
        {
            _player.playerAttackSpeed -= GetClassData(_combatSystem.playerClass).upgradeAttackSpeed;
            _player.playerAttackSpeed = Mathf.Clamp(_player.playerAttackSpeed, GetClassData(_combatSystem.playerClass).attackSpeedLimit, 5);
            playerLevelUpPoint--;
        });

        playerLevelUp.speedButton.onClick.AddListener(() =>
        {
            _player.walkSpeed += GetClassData(_combatSystem.playerClass).upgradeWalkSpeed;
            _player.sprintSpeed +=GetClassData(_combatSystem.playerClass).upgradeSprintSpeed;
            _player.sprintStaminaDrain -= GetClassData(_combatSystem.playerClass).upgradeSprintStaminaDrain;
            _player.walkSpeed = Mathf.Clamp(_player.walkSpeed, 0, GetClassData(_combatSystem.playerClass).walkSpeedLimit);
            _player.sprintSpeed = Mathf.Clamp(_player.sprintSpeed, 0, GetClassData(_combatSystem.playerClass).sprintSpeedLimit);
            playerLevelUpPoint--;
        });
        
        playerLevelUp.skipButton.onClick.AddListener(() =>
        {
            playerLevelUp.levelUpUI.gameObject.SetActive(false);
            Time.timeScale = 1;
        });
    }

    private void ClassSelectButtonAssign()
    {
        classTypeSelect.assaultRifleButton.onClick.AddListener(() =>
        {
            SetPlayerClass(CombatSystem.PlayerClass.AssaultRifle);
            classTypeSelect.classSelectUI.gameObject.SetActive(false);
        });

        classTypeSelect.shotGunButton.onClick.AddListener(() =>
        {
            SetPlayerClass(CombatSystem.PlayerClass.Shotgun);
            classTypeSelect.classSelectUI.gameObject.SetActive(false);
        });

        classTypeSelect.sniperButton.onClick.AddListener(() =>
        {
            SetPlayerClass(CombatSystem.PlayerClass.Sniper);
            classTypeSelect.classSelectUI.gameObject.SetActive(false);
        });

        classTypeSelect.missileButton.onClick.AddListener(() =>
        {
            SetPlayerClass(CombatSystem.PlayerClass.Missile);
            classTypeSelect.classSelectUI.gameObject.SetActive(false);
        });

        classTypeSelect.swordButton.onClick.AddListener(() =>
        {
            SetPlayerClass(CombatSystem.PlayerClass.Sword);
            classTypeSelect.classSelectUI.gameObject.SetActive(false);
        });
    }

}

