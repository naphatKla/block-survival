using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBooGeymanBuff : Buff
{
    public override void OnStartBuffFirstTime()
    {
        player.onKillEnemy += () => player.playerDamageBuff += 0.0025f;
        player.maxHealthMultiplier -= 0.25f;
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
