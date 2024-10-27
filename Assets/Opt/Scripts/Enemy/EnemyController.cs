using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class EnemyController : MonoBehaviour
{
    #region Declare Variable
    public Transform target;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    private Vector2 _movement;
    
    [Header("Enemy Status")]
    public float moveSpeed;
    public float maxHp;
    public float currentHp;
    public float damage;
    private float _attackCooldown = 1f;
    private float _lastAttackTime;
    private float _attackRange;
    #endregion
    
    #region Unity Method
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on Enemy Object!");
        }
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D not found on Enemy Object!");
        }

        currentHp = maxHp;
        
        // Attack Range = Enemy Box Collider
        _attackRange = Mathf.Max(boxCollider.size.x, boxCollider.size.y);
    }

    protected virtual void Update()
    {
        // If Player is alive then move to player
        PlayerController playerController = target.GetComponent<PlayerController>();
        if (playerController != null && playerController.isPlayerAlive)
        {
            // Finding player position
            Vector2 direction = target.position - transform.position;
            direction.Normalize();
            _movement = direction;

            // Facing to player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        
            // Check the distance and attack the player if within range.
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (distanceToPlayer <= _attackRange && Time.time >= _lastAttackTime + _attackCooldown)
            {
                DealDamage();
                _lastAttackTime = Time.time;
            }
        }
        else
        {
            _movement = Vector2.zero; // Stop moving when player die
        }
    }
    
    protected  virtual void FixedUpdate()
    {
        // Moving
        rb.velocity = new Vector2(_movement.x, _movement.y) * moveSpeed;
    }
    #endregion

    #region Method

    protected virtual void DealDamage()
    {
        PlayerController playerController = target.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage);
            Debug.Log("Enemy dealt " + damage + " damage to Player.");
        }
    }

     public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        Debug.Log("Enemy took " + damage + " damage. Current HP: " + currentHp);
        
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy has been destroyed.");
        }
    }
    #endregion
}
