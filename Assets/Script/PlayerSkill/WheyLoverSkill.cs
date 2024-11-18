using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WheyLoverSkill : SkillBase
{
    public float maxHpDmgPercentage;
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
            col.gameObject.GetComponent<Enemy>().TakeDamage(player.MaxHealth*(maxHpDmgPercentage/100), isKnockBack, knockBackForce, knockBackDuration);
        }
    }
}
