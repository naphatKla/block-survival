using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MagicPower : MonoBehaviour
{
    public float magicDamage;
    public float magicCooldown;
    public float magicDuration;
    public float magicMoveSpeed;
    public float magicMultipleShot;
    public float magicSize;
    
    private float _cooldownTimer;
    
    private void Update()
    {
        // Auto spell Magic
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer <= 0)
        {
            CastMagic();
            _cooldownTimer = magicCooldown; // Cooldown reset
        }
    }
    protected abstract void CastMagic();

}
