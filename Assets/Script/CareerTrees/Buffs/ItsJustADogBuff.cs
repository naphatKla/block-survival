

using System;
using UnityEngine;

public class ItsJustADogBuff : Buff
{
    private float _timeCount;
    private float _currentMultiplier;
    
    protected override void Start()
    {
        base.Start();
       
    }
    private void Update()
    {
        base.Update();
        _timeCount += Time.deltaTime;
    }
    public override void OnStartBuffFirstTime()
    {
        player.maxHealthMultiplier -= 0.25f;
    }

    public override void OnStartBuff()
    {
       
    }

    public override void OnUpdateBuff()
    {
        if (_timeCount > player.LastTakeDamageTime + 60f)
        {
            if (_currentMultiplier < 10f)
            {
                player.damageMultiplier += 8f;
                _currentMultiplier = 10f;
            }
        }
        else if (_timeCount > player.LastTakeDamageTime + 15f)
        {
            if (_currentMultiplier < 2f)
            {
                player.damageMultiplier += 0.75f;
                _currentMultiplier = 2f;
            }
        }
        if (_timeCount > player.LastTakeDamageTime + 5f)
        {
            if (_currentMultiplier < 1.25f)
            {
                player.damageMultiplier += 0.25f;
                _currentMultiplier = 1.25f;
            }
        }
    }

    public override void OnEndBuff()
    {
        player.damageMultiplier -= _currentMultiplier - 1f;
        _currentMultiplier = 0;
    }

    public override bool ApplyBuffCondition()
    {
        return _timeCount > player.LastTakeDamageTime + 5f;
    }
}
