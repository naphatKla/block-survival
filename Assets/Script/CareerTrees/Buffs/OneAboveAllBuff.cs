using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneAboveAllBuff : Buff
{
    public override void OnStartBuffFirstTime()
    {
        player.playerDamageBuff += 8f;
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
