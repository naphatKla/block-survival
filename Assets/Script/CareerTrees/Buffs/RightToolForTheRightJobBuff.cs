

using System.Collections;

public class RightToolForTheRightJobBuff : Buff
{
    public override void OnStartBuffFirstTime()
    {
        combatSystem.canSelectClass = false;
        player.criticalRate += 25f;
        player.maxHealthMultiplier -= 0.2f;
        player.walkSpeedBuff -= 1f;
        StartCoroutine(WaitAndSetPlayerClass());
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
    
    IEnumerator WaitAndSetPlayerClass()
    {
        yield return null;
        yield return null;
        yield return null;
        // wait for 3 frames
        Level.Instance.SetPlayerClass(CombatSystem.PlayerClass.AllForOne);
    }
}
