public class WheyLoverBuff : Buff
{
    public override void OnStartBuffFirstTime()
    {
        player.maxHealthMultiplier += 0.15f;
        player.health = player.MaxHealth;
        player.playerDamageBuff -= 2f;
        player.damageReductionPercentage += 5f;
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
