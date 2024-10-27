using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MagicPower : MonoBehaviour
{
    public float magicDamage;
    public float magicCooldown;
    public float magicDuration;
    public float magicMoveSpeed;
    public float magicMultipleShot;
    public float magicMultipleShotCooldown;
    public float magicSize;
    
    private float _cooldownTimer;
    
    private void Update()
    {
        // Auto cast Magic
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer <= 0)
        {
            CastMagic();
            _cooldownTimer = magicCooldown; // Cooldown reset
        }
    }
    protected abstract void CastMagic();
    
    protected virtual GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Find Tag "Enemy"
        GameObject closestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position); // Calculate position
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
