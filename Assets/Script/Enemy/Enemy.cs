using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mono.Cecil.Cil;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region Declare Variable
    [SerializeField] protected string enemyName;
    [SerializeField] protected EnemyType enemyType;
    // For range enemy
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float bulletOffset;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float attackRange;

    [Space] [Header("Enemy Stats")]
    [SerializeField] public float maxHp;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected DamageText damageText;
    [SerializeField] protected float expDrop;
    
    [Header("Movement")]
    [SerializeField] protected float maxSpeed;
    [SerializeField] protected float minSpeed;
    [SerializeField] protected float turnDirectionDamp;
    protected bool _canMove;
    protected float _smoothDampVelocity;
    // The condition of distance for change enemy speed depend on target distance
    [SerializeField] protected float distanceThreshold;
    
    [Header("Other")]
    [HideInInspector] public Rigidbody2D rigidbody2D;
    protected Player _player;
    protected float _currentSpeed;
    protected float _currentHp;
    protected Level _level;
    protected GameManager _gameManager;

    [Header("Particle Effect")]
    [SerializeField] protected ParticleSystem deadParticleSystem;
    
    [Header("Bar")]
    protected Scrollbar _hpBar;
    
    [Header("Loot Chest")] 
    [SerializeField] protected GameObject lootChest;
    
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    
    public enum EnemyType
    {
        Melee,
        Range
    }
    #endregion

    #region Unity Method
    protected virtual void Start()
    {
        _currentHp = maxHp;
        _canMove = true;
        _gameManager = FindObjectOfType<GameManager>();
        _player = FindObjectOfType<Player>();
        _level = FindObjectOfType<Level>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hpBar = GetComponentInChildren<Canvas>().GetComponentInChildren<Scrollbar>();
        _hpBar.gameObject.SetActive(false);
        attackRange = Random.Range(attackRange - 2f, attackRange + 2f);
        attackRange = Mathf.Round(attackRange);
        _originalColor = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, _spriteRenderer.color.a);
        
        if (enemyType != EnemyType.Range) return;
        Invoke(nameof(ShootBullet), fireRate);
    }
    
    protected virtual void Update()
    {
        FollowTargetHandle(_player);
        EnemyBarUpdate();
    }
    
    private IEnumerator KnockBack(float knockBackForce = 5, float knockBackDuration = 0.1f)
    {
        float timeCount = 0;

        while (timeCount < knockBackDuration)
        {
            _canMove = false;
            rigidbody2D.velocity = -transform.up * knockBackForce;
            timeCount += Time.deltaTime;
            yield return null;
        }
        rigidbody2D.velocity = Vector2.zero;

        _canMove = true;
    }

    private IEnumerator HitColorChange()
    {
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        _spriteRenderer.color = _originalColor;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        Player player = col.gameObject.GetComponent<Player>();    
        Vector2 direction = (player.transform.position - transform.position).normalized;
        player.TakeDamage(attackDamage,true,direction,10f);
    }

    #endregion

    #region Method

    private Vector2 _directionDamp = Vector2.zero;
    private void FollowTargetHandle(Player target)
    {
        if (!_canMove) return;
        
        Vector2 targetPosition = target.transform.position;
        Vector2 direction = Vector2.SmoothDamp(transform.up,targetPosition - (Vector2)transform.position, ref _directionDamp, turnDirectionDamp) ;
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        transform.up = direction;

        // Melee Enemy
        if (enemyType == EnemyType.Melee)
        {
            if (distanceToTarget <= 0.5f)
            {
                rigidbody2D.velocity = Vector2.zero;
                return;
            }

            rigidbody2D.velocity = transform.up.normalized * _currentSpeed;

            _currentSpeed = distanceToTarget > distanceThreshold
                ? Mathf.SmoothDamp(_currentSpeed, maxSpeed, ref _smoothDampVelocity, 0.3f)
                : Mathf.SmoothDamp(_currentSpeed, minSpeed, ref _smoothDampVelocity, 0.3f);
            return;
        }

        // Range Enemy
        if (Math.Round(distanceToTarget).Equals(attackRange))
        {
            rigidbody2D.velocity = Vector2.zero;
            return;
        }

        rigidbody2D.velocity = distanceToTarget > attackRange
            ? transform.up.normalized * _currentSpeed
            : -transform.up.normalized * _currentSpeed;

        _currentSpeed = Math.Round(distanceToTarget) < distanceThreshold
            ? Mathf.SmoothDamp(_currentSpeed, maxSpeed, ref _smoothDampVelocity, 0.3f)
            : Mathf.SmoothDamp(_currentSpeed, minSpeed, ref _smoothDampVelocity, 0.3f);
    }
    
    private void EnemyBarUpdate()
    {
        _hpBar.transform.parent.rotation = Quaternion.identity;
        _hpBar.size = _currentHp / maxHp;
        
    }
    
    public void TakeDamage(float damage, bool isKnockBack = false, float knockBackForce = 5, float knockBackDuration = 0.1f, bool isCritical = false)
    {
        if(!_hpBar.gameObject.activeSelf) _hpBar.gameObject.SetActive(true);
        if (damage <= 0) return;
        _currentHp -= damage;
        Color color = isCritical ? Color.yellow : Color.white;
        float size = isCritical ? 1.5f : 1f;
        Instantiate(damageText, transform.position, Quaternion.identity).InitializeText(damage,color,size);
        StartCoroutine(HitColorChange());
        
        if (isKnockBack)
            StartCoroutine(KnockBack(knockBackForce,knockBackDuration));
        
        if (_currentHp <= 0)
        {
            ParticleEffectManager.Instance.PlayParticleEffect(deadParticleSystem,transform.position);
            Destroy(gameObject);
        }
    }

    public void TakeDamagePercentage(float percentage, bool isKnockBack = false, float knockBackForce = 5,
        float knockBackDuration = 0.1f)
    {
        if(!_hpBar.gameObject.activeSelf) _hpBar.gameObject.SetActive(true);
        if (maxHp * (percentage/100) <= 0) return;
        _currentHp -= maxHp * (percentage/100);
        Instantiate(damageText, transform.position, Quaternion.identity).InitializeText(maxHp * (percentage/100),Color.white,1f);
        StartCoroutine(HitColorChange());
        
        if (isKnockBack)
            StartCoroutine(KnockBack(knockBackForce,knockBackDuration));
        
        if (_currentHp <= 0)
        {
            ParticleEffectManager.Instance.PlayParticleEffect(deadParticleSystem,transform.position);
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        try
        {
            if(Random.Range(0,5001) >= 4999f && _level.playerLevel >= 5)
                LootChestSpawn();
            _level.enemyKill++;
            _level.LevelGain(expDrop);
            _gameManager.enemyLeft--;
        }
        catch (Exception e)
        {
            //Debug.Log(e);
        }
    }

    private void ShootBullet()
    {
        Instantiate(bulletPrefab, transform.position + (transform.up * bulletOffset), transform.rotation);
        Invoke(nameof(ShootBullet), fireRate);
    }
    
    protected void LootChestSpawn()
    {
        GameObject lootChestSpawn = Instantiate(lootChest, transform.position, quaternion.identity);
        Destroy(lootChestSpawn, 15f);
    }
    #endregion
}
