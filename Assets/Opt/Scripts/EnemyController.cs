using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    #region Declare Variable
    public Transform player;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    private Vector2 _movement;
    
    [Header("Enemy Status")]
    public float enemyMoveSpeed = 3f;
    public float enemyMaxHp = 100f;
    public float enemyCurrentHp = 100f;
    public float enemyDamage = 10f;
    public float attackCooldown = 1f;
    private float _lastAttackTime;
    private float _attackRange;
    #endregion
    
    #region Unity Method
    private void Start()
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

        enemyCurrentHp = enemyMaxHp;
        
        _attackRange = Mathf.Max(boxCollider.size.x, boxCollider.size.y);
    }

    private void Update()
    {
        // If Player is alive then move to player
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && playerController.isPlayerAlive)
        {
            // Finding player position
            Vector2 direction = player.position - transform.position;
            direction.Normalize();
            _movement = direction;

            // Facing to player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        
            // Check the distance and attack the player if within range.
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= _attackRange && Time.time >= _lastAttackTime + attackCooldown)
            {
                DealDamageToPlayer();
                _lastAttackTime = Time.time;
            }
        }
        else
        {
            _movement = Vector2.zero; // Stop moving when player die
        }
    }

    private void FixedUpdate()
    {
        // Moving
        rb.velocity = new Vector2(_movement.x, _movement.y) * enemyMoveSpeed;
    }
    #endregion

    #region Method
    private void DealDamageToPlayer()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(enemyDamage);
            Debug.Log("Enemy dealt " + enemyDamage + " damage to Player.");
        }
    }
    #endregion
}
