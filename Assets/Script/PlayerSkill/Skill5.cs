using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill5 : SkillBase
{
    private float _firstBulletSpeedAttack;
    
    
    protected override void SkillAction()
    {
        _firstBulletSpeedAttack = player.basePlayerAttackSpeed;
        player.basePlayerAttackSpeed /= 2f;
    }

    private void OnDestroy()
    {
        player.basePlayerAttackSpeed = _firstBulletSpeedAttack;
    }
}
