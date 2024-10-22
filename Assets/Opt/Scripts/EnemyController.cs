using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    private Vector2 movement;
    
    [Header("Enemy Status")]
    public float enemyMoveSpeed = 3f;
    public float enemyMaxHp = 100f;
    public float enemyCurrentHp = 100f;
    public float enemyDamage = 10f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private float attackRange;

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
        
        attackRange = Mathf.Max(boxCollider.size.x, boxCollider.size.y);
    }

    private void Update()
    {
        // Finding player
        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

        // Facing the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        
        // Check the distance and attack the player if within range.
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            DealDamageToPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        // Moving to player
        rb.velocity = new Vector2(movement.x, movement.y) * enemyMoveSpeed;
    }

    private void DealDamageToPlayer()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(enemyDamage);
            Debug.Log("Enemy dealt " + enemyDamage + " damage to Player.");
        }
    }
}
