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

            // Build Object projectile set direction
            GameObject spit = Instantiate(spitPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = spit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // projectile moving to Enemy direction
                rb.velocity = direction * magicMoveSpeed;
            }
        }
    }
    
    private GameObject FindClosestEnemy()
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

