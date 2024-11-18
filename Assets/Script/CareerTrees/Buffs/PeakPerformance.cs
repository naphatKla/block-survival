using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakPerformance : Buff
{
    public override void OnStartBuffFirstTime()
    {
        player.maxHealthMultiplier += 0.25f;
        player.health = player.MaxHealth;
        Debug.Log("Peak Performance Buff Activated");
    }

    public override void OnStartBuff()
    {
        player.damageReductionPercentage += 45f;
        Debug.Log("Peak Performance Buff Activated");
    }

    public override void OnUpdateBuff()
    {
      
    }

    public override void OnEndBuff()
    {
        player.damageReductionPercentage -= 45f;
        Debug.Log("Peak Performance Buff END!!!!");
    }

    public override bool ApplyBuffCondition()
    {
        return player.CurrentHPPercentage >= 0.8f;
    }
}
