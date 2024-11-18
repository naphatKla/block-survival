using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletType bulletType;
    [SerializeField] private PlayerClassData playerClassData;
    [SerializeField] private DamageText damageText;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletDamage;
    [SerializeField] public float bulletOffSetScale;
    [SerializeField] public float destroyTime;
    
    private CombatSystem _combatSystem;

    public enum BulletType
    {
        Player,
        Enemy,
        PlayerHelper
    }
    
    protected Player player;
    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        _combatSystem = FindObjectOfType<CombatSystem>();
        if(bulletType.Equals(BulletType.Player))
            AssignBulletData();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        _rigidbody2D.velocity = transform.up * bulletSpeed;
        Destroy(gameObject, destroyTime);
    }
    void FixedUpdate()
    {
        //_rigidbody2D.velocity = transform.up * bulletSpeed;
    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
   
        
        if (bulletType.Equals(BulletType.Enemy))
        {
            if (col.gameObject.CompareTag("Player"))
            {
                Player player = col.gameObject.GetComponent<Player>();
            
                if(player == null) return;
                player.TakeDamage(bulletDamage);
                Instantiate(damageText, col.transform.position, Quaternion.identity).InitializeText(bulletDamage,Color.red,1f);
            }
            return;
        }
        
        float damage = bulletType.Equals(BulletType.PlayerHelper) ? bulletDamage : player.PlayerDamage;
        
        if (col.gameObject.CompareTag("Guard") && !bulletType.Equals(BulletType.PlayerHelper))
        {
            col.GetComponent<Guard>().TakeDamage(damage);
            Instantiate(damageText, col.transform.position, Quaternion.identity).InitializeText(damage,Color.gray,1f);
            Destroy(gameObject);
        }
        
        if (col.gameObject.CompareTag("Enemy"))
        {
            try
            {
                if(col == null) return;
                Enemy _enemy = col.gameObject.GetComponent<Enemy>();
                if(_enemy == null) return;

                if (_combatSystem.playerClass.Equals(CombatSystem.PlayerClass.Sword) &&
                    !bulletType.Equals(BulletType.PlayerHelper) && _enemy.maxHp < 500f)
                {
                    _enemy.TakeDamage(damage,true,5,0.15f);
                    Instantiate(damageText, col.transform.position, Quaternion.identity).InitializeText(damage,Color.white,1f);
                }
                else
                {
                    _enemy.TakeDamage(damage);
                    Instantiate(damageText, col.transform.position, Quaternion.identity).InitializeText(damage,Color.white,1f);
                }
            }
            catch (Exception e)
            {
                //Debug.Log(e);
            }
        }
    }

    private void AssignBulletData()
    {
        bulletSpeed = playerClassData.bulletSpeed;
        bulletOffSetScale = playerClassData.bulletOffSetScale;
        destroyTime = playerClassData.destroyTime;
    }
}
