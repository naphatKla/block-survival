using System;
using UnityEngine;

public class WeGoFullBody : Buff
{
    private float playerMaxHealthTemp;
    private float playerDamageTemp;
    private float playerMaxHealthBuffTemp;
    private float playerDamageBuffTemp;
    public override void OnStartBuff()
    {
        playerMaxHealthTemp = player.baseMaxHealth;
        player.maxHealthBuff += -(player.baseMaxHealth * 0.35f); // reduce 35% of max health
        playerMaxHealthBuffTemp = -(player.baseMaxHealth * 0.35f);

        playerDamageTemp = player.basePlayerDamage;
        player.playerDamageBuff += player.basePlayerDamage * 0.25f; // increase 25% of damage
        playerDamageBuffTemp = player.basePlayerDamage * 0.25f;
        Debug.Log("Enter Buff: We Go Full Body");
        Debug.Log($"Base HP: {player.baseMaxHealth} : Buff HP: {player.maxHealthBuff} : Result : {player.MaxHealth}");
        Debug.Log($"Base DMG: {player.basePlayerDamage} : Buff DMG: {player.playerDamageBuff} : Result : {player.PlayerDamage}");
        Debug.Log("=====================================");
    }

    public override void OnUpdateBuff()
    {
        if (Math.Abs(playerMaxHealthTemp - player.baseMaxHealth) > 0.1f)
        {
            playerMaxHealthTemp = player.baseMaxHealth;
            player.maxHealthBuff -= playerMaxHealthBuffTemp;
            player.maxHealthBuff += -(player.baseMaxHealth * 0.35f); // reduce 35% of max health
            playerMaxHealthBuffTemp = -(player.baseMaxHealth * 0.35f);
            
            Debug.Log("Update Buff: We Go Full Body");
            Debug.Log($"Base HP: {player.baseMaxHealth} : Buff HP: {player.maxHealthBuff} : Result : {player.MaxHealth}");
            Debug.Log($"Base DMG: {player.basePlayerDamage} : Buff DMG: {player.playerDamageBuff} : Result : {player.PlayerDamage}");
            Debug.Log("=====================================");
        }
        
        if (Math.Abs(playerDamageTemp - player.basePlayerDamage) > 0.1f)
        {
            playerDamageTemp = player.basePlayerDamage;
            player.playerDamageBuff -= playerDamageBuffTemp;
            player.playerDamageBuff += player.basePlayerDamage * 0.25f; // increase 25% of damage
            playerDamageBuffTemp = player.basePlayerDamage * 0.25f;
            
            Debug.Log("Update Buff: We Go Full Body");
            Debug.Log($"Base HP: {player.baseMaxHealth} : Buff HP: {player.maxHealthBuff} : Result : {player.MaxHealth}");
            Debug.Log($"Base DMG: {player.basePlayerDamage} : Buff DMG: {player.playerDamageBuff} : Result : {player.PlayerDamage}");
            Debug.Log("=====================================");
        }
    }

    public override void OnEndBuff()
    {
        
    }

    public override bool ApplyBuffCondition()
    {
        return true;
    }
}
