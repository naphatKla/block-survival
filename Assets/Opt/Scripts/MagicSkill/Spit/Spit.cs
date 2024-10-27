using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MagicPower
{
    public GameObject spitPrefab;

    private void Start()
    {
        magicDamage = 20f;
        magicCooldown = 2f;
        magicMoveSpeed = 40;
        magicMultipleShot = 1f;
    }

    protected override void CastMagic()
    {
        // Find the nearest  Enemy
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            // Calculate the direction to the nearest enemy
            Vector2 direction = closestEnemy.transform.position - transform.position;
            direction.Normalize();
            
            
            GameObject spit = Instantiate(spitPrefab, transform.position, Quaternion.identity);
            SpitBullet spitBullet = spit.GetComponent<SpitBullet>();
            
            if (spitBullet != null)
            {
                spitBullet.damage = magicDamage;
                Rigidbody2D rb = spit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Spit Bullet moving to Enemy direction
                    rb.velocity = direction * magicMoveSpeed;
                }
            }
        }
    }
    
}

