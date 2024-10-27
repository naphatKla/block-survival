using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        Spit spit = collision.GetComponent<Spit>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
