using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    [SerializeField] private Explode explode;

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Guard"))
        {
            col.GetComponent<Guard>().TakeDamage(player.PlayerDamage);
            Instantiate(explode.gameObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Boom");
            try
            {
                Debug.Log("Boom");
                if(col == null) return;
                Enemy _enemy = col.gameObject.GetComponent<Enemy>();
                if(_enemy == null) return;
                _enemy.TakeDamage(player.PlayerDamage);
                Instantiate(explode.gameObject, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                //Debug.Log(e);
            }
        }
    }
}
