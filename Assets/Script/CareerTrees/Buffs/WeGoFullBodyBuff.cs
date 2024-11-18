using System;
using UnityEngine;

public class WeGoFullBodyBuff : Buff
{
    private float playerMaxHealthTemp;
    private float playerDamageTemp;
    private float playerMaxHealthBuffTemp;
    private float playerDamageBuffTemp;
    public override void OnStartBuffFirstTime()
    {
        player.damageMultiplier += 0.25f;
        player.maxHealthMultiplier -= 0.35f;
    }

    public override void OnStartBuff()
    {

    }

    public override void OnUpdateBuff()
    {

    }

    public override void OnEndBuff()
    {
        
    }

    public override bool ApplyBuffCondition()
    {
        return true;
    }
}
