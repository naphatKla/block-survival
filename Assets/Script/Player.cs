using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Declare Variable
    [Header("Component")] 
    [SerializeField] public Rigidbody2D playerRigidbody2D;
    [SerializeField] private TrailRenderer trailEffect;
    private SpriteRenderer _spriteRenderer;
    private Color _defaultSpriteColor;

    [Header("Player Stats")] 
    public float maxHealth;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegen;
    public float playerAttackSpeed;
    public float playerDamage;
    public float health;
    private float _stamina;
    public bool isImmune;
    
    [Header("UI Bar")] 
    [SerializeField] private Scrollbar healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Scrollbar staminaBar;
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private AudioSource getHitSoundEffect;
    [SerializeField] private AudioSource dashSoundEffect;

    [Header("Camera")] 
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float cameraSmoothDamp;
    private Vector3 _velocity = Vector3.zero;

    [Header("Player Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float dashStaminaDrain;
    public float sprintStaminaDrain;
    public float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float staminaRecoveryCooldown;
    [HideInInspector] public Transform playerTransform;
    private float _currentSpeed;
    public PlayerMovementStatus playerMovementStatus;

    public enum PlayerMovementStatus
    {
        Clear,
        Idle,
        Walk,
        Sprint,
        Dash,
        Stun,
        StaminaRecoveryCooldown
    }
    #endregion
    
    #region Unity Method
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _defaultSpriteColor = _spriteRenderer.color;
        playerTransform = transform.GetChild(0);
        health = maxHealth;
        _stamina = maxStamina;
        _currentSpeed = walkSpeed;
    }
    void Update()
    {
        if(Time.timeScale.Equals(0)) return;
        RotatePlayerFollowMouseDirection();
        CameraFollowPlayer();
        PlayerMovementHandle();
        PlayerBarUpdate();
        health = Mathf.Clamp(health, 0, maxHealth);
    }
    
    private IEnumerator Dash()
    {
        _stamina -= dashStaminaDrain;
        trailEffect.emitting = true;
        dashSoundEffect.Play();
        _spriteRenderer.color = new Color( _spriteRenderer.color.r,  _spriteRenderer.color.g,  _spriteRenderer.color.b, 0.5f);
        
        float dashTimeCount = 0;
        while (dashTimeCount < dashDuration)
        {
            SetPlayerStatus(PlayerMovementStatus.Dash);
            isImmune = true;
            dashTimeCount += Time.deltaTime;
            _currentSpeed = dashSpeed;

            if (playerRigidbody2D.velocity.magnitude < dashSpeed)
                playerRigidbody2D.velocity = playerRigidbody2D.velocity.normalized * dashSpeed;
            
            playerRigidbody2D.velocity = Vector2.ClampMagnitude(playerRigidbody2D.velocity, dashSpeed);
            yield return null;
        }
        
        trailEffect.emitting = false;
        isImmune = false;
        ResetSpriteColor();
    }
    private IEnumerator StaminaRecoveryCooldown()
    {
        float timeCount = 0;

        while(timeCount < staminaRecoveryCooldown)
        {
            SetPlayerStatus(PlayerMovementStatus.StaminaRecoveryCooldown);
            timeCount += Time.deltaTime;
            _currentSpeed = walkSpeed / 2;
            yield return null;
        }
        SetPlayerStatus(PlayerMovementStatus.Clear);
    }
    
    private IEnumerator KnockBack(Vector2 knockDirection, float knockBackForce = 5, float knockBackDuration = 0.1f)
    {
        float timeCount = 0;

        while (timeCount < knockBackDuration)
        {
            //SetPlayerStatus(PlayerStatus.Stun);
            playerRigidbody2D.velocity = knockDirection.normalized * knockBackForce;
            timeCount += Time.deltaTime;
            yield return null;
        }
        playerRigidbody2D.velocity = Vector2.zero;

        SetPlayerStatus(PlayerMovementStatus.Clear);
    }
    #endregion

    #region method
    private void CameraFollowPlayer()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, 0, -10);
        double cameraToTargetDistance = Math.Round(Vector3.Distance(targetPosition, playerCamera.transform.position),3);
        if(cameraToTargetDistance <= 0) return;
   
        Vector3 newCameraPosition = Vector3.SmoothDamp(playerCamera.transform.position, targetPosition,
            ref _velocity, cameraSmoothDamp);

        playerCamera.transform.position = newCameraPosition;
    }
    private void DashHandle()
    {
        if (!MovementConditionCheck(PlayerMovementStatus.Dash)) return;
        StartCoroutine(Dash());
    }
    private void StaminaRegenHandle()
    {
        _stamina = Mathf.Clamp(_stamina, 0, maxStamina);
        bool staminaRegenConditionCheck =
            (!playerMovementStatus.Equals(PlayerMovementStatus.Sprint) && !playerMovementStatus.Equals(PlayerMovementStatus.Dash)) && _stamina < maxStamina;
            
        if (!staminaRegenConditionCheck) return;
        _stamina += Time.deltaTime * staminaRegen;
    }
    private void SprintHandle()
    {
        if (!MovementConditionCheck(PlayerMovementStatus.Sprint)) return;
        
        SetPlayerStatus(PlayerMovementStatus.Sprint);
        _stamina -= Time.deltaTime * sprintStaminaDrain;
        _currentSpeed = sprintSpeed;
    }
    private void PlayerMovementHandle()
    {
        if(playerMovementStatus.Equals(PlayerMovementStatus.Stun)) return;
        StaminaRegenHandle();
        
        Vector2 playerVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * _currentSpeed;
        playerRigidbody2D.velocity = playerVelocity;
        
        if(playerMovementStatus.Equals(PlayerMovementStatus.StaminaRecoveryCooldown)) return;
        
        if (Input.GetAxisRaw("Horizontal").Equals(0) && Input.GetAxisRaw("Vertical").Equals(0))
        {
            playerMovementStatus = PlayerMovementStatus.Idle;
            return;
        }

        if (_stamina <= 0) StartCoroutine(StaminaRecoveryCooldown());
        
        SetPlayerStatus(PlayerMovementStatus.Walk);
        _currentSpeed = walkSpeed;
        
        SprintHandle();
        DashHandle();
    }
    public void PlayerBarUpdate()
    {
        staminaBar.value = 0;
        healthBar.value = 0;
        staminaBar.size = _stamina / maxStamina;
        staminaText.text = $"{_stamina:F0} / {maxStamina}";
        healthBar.size = health / maxHealth;
        healthText.text = $"{health:F0} / {maxHealth}";
    }
    
    private bool MovementConditionCheck(PlayerMovementStatus movementStatus)
    {
        switch (movementStatus)
        {
            case PlayerMovementStatus.Sprint:
                return Input.GetKey(KeyCode.LeftShift) && _stamina > 0;
            case PlayerMovementStatus.Dash:
                return Input.GetKeyDown(KeyCode.LeftControl) && !playerMovementStatus.Equals( PlayerMovementStatus.Dash) && _stamina >= dashStaminaDrain;
            default: return false;
        }
    }

    public void SetPlayerStatus(PlayerMovementStatus movementStatus)
    {
        if(movementStatus.Equals(playerMovementStatus)) return;
        playerMovementStatus = movementStatus;
    }
    
    private void RotatePlayerFollowMouseDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        //mousePosition = playerCamera.ScreenToWorldPoint(mousePosition);
        float xAngle = mousePosition.x - playerTransform.position.x;
        float yAngle = mousePosition.y - playerTransform.position.y;
        Vector2 direction = new Vector2(xAngle, yAngle);
        playerTransform.up = direction;
    }
    
    public void TakeDamage(float damage)
    {
        if(isImmune) return;
        health -= damage;
        getHitSoundEffect.Play();
        _spriteRenderer.color = Color.red - new Color(0,0,0,0.5f);
        Invoke(nameof(ResetSpriteColor),0.1f);
        
        if (health <= 0)
        {
            // Die
        }
    }
    
    public void TakeDamage(float damage, bool isKnockBack,Vector2 knockDirection ,  float knockBackForce = 5, float knockBackDuration = 0.1f)
    {
        if(isImmune) return;
        
        if (damage > 0)
        {
            health -= damage;
            getHitSoundEffect.Play();
            _spriteRenderer.color = new Color(1, 0.16f, 0, 0.5f);
            Invoke(nameof(ResetSpriteColor),0.1f);
        }
        
        if (isKnockBack)
        {
            StartCoroutine(KnockBack(knockDirection, knockBackForce, knockBackDuration));
        }

        if (health <= 0)
        {
            // Die
        }
    }

    private void ResetSpriteColor()
    {
        _spriteRenderer.color = _defaultSpriteColor;
    }
    #endregion
}
