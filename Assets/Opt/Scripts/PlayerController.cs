using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private PlayerInputAction playerControls;
    
     private Vector2 moveDirection = Vector2.zero;
     private InputAction move;

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
    }

    private void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90); // หมุนตัว Player หันไปตามทิศทาง
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}


