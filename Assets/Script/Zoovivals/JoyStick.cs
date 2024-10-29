using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class JoyStick : MonoBehaviour
{
    [Header("Movement Setting")] 
    [SerializeField] private float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool facingRight = true;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        FlipSprite();
    }
    
    private void MovePlayer()
    {
        Vector2 movement = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        rb.velocity = movement;
    }
    
    private void FlipSprite()
    {
        if (moveInput.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && facingRight)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
