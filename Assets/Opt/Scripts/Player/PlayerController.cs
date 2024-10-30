using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    #region Declare Variable
    private Rigidbody2D rb;
    private PlayerInputAction _playerControls;
    private Vector2 _moveDirection = Vector2.zero;
    private InputAction _move;
    
    [Header("Player Status")]
    public float baseMoveSpeed = 5f;
    public float addMoveSpeed; // Increases movement speed by a % of base movement speed
    public float maxHp = 1000f;
    public float currentHp = 1000f;
    public float hpRegen = 0.05f;
    public bool isPlayerAlive = true;
    public float playerScore;
    #endregion
    
    #region Unity Method
    private void Awake()
    {
        _playerControls = new PlayerInputAction();
    }

    private void OnEnable()
    {
        _move = _playerControls.Game.Move;
        _move.Enable();
    }

    private void OnDisable()
    {
        _move.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on Player Object!");
        }
        
        currentHp = maxHp;
        playerScore = 0f;
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            _moveDirection = _move.ReadValue<Vector2>();

            // Player facing
            if (_moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }

            HpRegenerated();
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerAlive)
        {
            // Player Movement
            float totalSpeed = baseMoveSpeed + (baseMoveSpeed * (addMoveSpeed / 100f));
            rb.velocity = new Vector2(_moveDirection.x * totalSpeed, _moveDirection.y * totalSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop moving if player die
        }
    }
    #endregion
    
    #region method
    public void TakeDamage(float damage)
    {
        if (isPlayerAlive)
        {
            currentHp -= damage;

            if (currentHp <= 0)
            {
                isPlayerAlive = false;
                Debug.Log("Player has died.");
                Debug.Log($"Your Score: {playerScore}");
            }
        }
    }

    private void HpRegenerated()
    {
        if (isPlayerAlive && currentHp < maxHp)
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
            TakeDamage(enemy.damage);
        }
    }
    #endregion
}


