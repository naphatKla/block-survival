using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheyLoverSkill : SkillBase
{
    public float skillDamagePercentage;
    protected override void Update()
    {
        base.Update();
    }
    protected override void SkillAction()
    {
        transform.SetParent(player.playerTransform);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().TakeDamagePercentage(skillDamagePercentage, isKnockBack, knockBackForce, knockBackDuration);
        }
    }
}
