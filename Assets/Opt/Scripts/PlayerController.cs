using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private PlayerInputAction playerControls;
    
    private Vector2 moveDirection = Vector2.zero;
    private InputAction move;
    
    [Header("Player Status")]
    public float moveSpeed = 5f;
    public float maxHp = 100f;
    public float currentHp = 100f;
    public float hpRegen;
    private float currentLevel = 1f;
    
    
    private void Awake()
    {
        playerControls = new PlayerInputAction();
    }

    private void OnEnable()
    {
        move = playerControls.Game.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on Player Object!");
        }
        
        currentHp = maxHp;
    }

    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    
        // Player facing
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        
        HpRegenerated();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void HpRegenerated()
    {
        if (currentHp < maxHp)
        {
            float regenAmount = maxHp * (hpRegen / 100f) * Time.deltaTime;
            currentHp += regenAmount;
            
            currentHp = Mathf.Min(currentHp, maxHp);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();
        if (enemy != null)
        {
            TakeDamage(enemy.enemyDamage);
        }
    }
    
}


