

public class b040174Buff : Buff
{
    public override void OnStartBuffFirstTime()
    {
        player.criticalRate += 15f;
        player.attackSpeedMultiplier -= 0.1f;
        player.maxHealthMultiplier -= 0.1f;
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
