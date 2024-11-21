using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private float guardMaxHp;
    [SerializeField] private float _guardHp;
    [SerializeField] private ParticleSystem guardHitEffect;
    [SerializeField] private ParticleSystem guardBreakEffect;
    [SerializeField] private DamageText damageText;
    private Vector3 _startScale;

    void Start()
    {
        _guardHp = guardMaxHp;
        _startScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void FixedUpdate()
    {
        if (_guardHp < guardMaxHp / 4)
        {
            transform.localScale = new Vector3(_startScale.x / 1.5f, _startScale.y / 3, _startScale.z);
        }
        else if (_guardHp < guardMaxHp / 2f)
        {
            transform.localScale = new Vector3(_startScale.x / 1.25f, _startScale.y / 2, _startScale.z);
        }
        else if (_guardHp < guardMaxHp / 1.5f)
        {
            transform.localScale = new Vector3(_startScale.x / 1.1f, _startScale.y / 1.5f , _startScale.z);
        }
    }
    
    public void TakeDamage(float damage, bool isCritical = false)
    {
        _guardHp -= damage;
        Color color = isCritical ? Color.yellow : Color.grey;
        float size = isCritical ? 1.5f : 1f;
        Instantiate(damageText, transform.position, Quaternion.identity).InitializeText(damage,color,size);
        ParticleEffectManager.Instance.PlayParticleEffect(guardHitEffect,transform.position);
        if (_guardHp <= 0)
        {
            Destroy(gameObject);
            ParticleEffectManager.Instance.PlayParticleEffect(guardBreakEffect,transform.position);
        }
      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Guard Hit Player");
            Vector2 pushDirection = other.transform.position - transform.position;
            Player player = other.gameObject.GetComponent<Player>();
            player.TakeDamage(0,true, pushDirection ,5f ,0.5f);
        }
    }
}
