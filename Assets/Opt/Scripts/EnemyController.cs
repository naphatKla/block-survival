using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; // ความเร็วของศัตรู
    public Transform player; // อ้างอิงตำแหน่งของผู้เล่น
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // ตรวจสอบว่า Rigidbody2D ถูกอ้างอิงหรือไม่
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on enemy object!");
        }
    }

    private void Update()
    {
        // ตรวจหาตำแหน่งของผู้เล่น
        Vector2 direction = player.position - transform.position;
        direction.Normalize(); // ปรับทิศทางให้เป็นหน่วยเวคเตอร์
        movement = direction;

        // หมุนศัตรูให้หันไปหาผู้เล่น
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // หมุนให้หันไปยังทิศทางของผู้เล่น
    }

    private void FixedUpdate()
    {
        // เคลื่อนที่ตามผู้เล่น
        rb.velocity = new Vector2(movement.x, movement.y) * moveSpeed;
    }
}
