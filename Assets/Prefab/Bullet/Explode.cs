using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField] public float destroyTime;
    [SerializeField] private DamageText damageText;
    private Player _player;
    void Start()
    {
        _player = FindObjectOfType<Player>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            try
            {
                if(col == null) return;
                Enemy _enemy = col.gameObject.GetComponent<Enemy>();
                if(_enemy == null) return;
                _enemy.TakeDamage(_player.PlayerDamage/2);
                Instantiate(damageText, col.transform.position, Quaternion.identity).InitializeText((_player.PlayerDamage/2),Color.white,1f);
            }
            catch (Exception e)
            {
                //Debug.Log(e);
            }
        }
    }
    
    
}
