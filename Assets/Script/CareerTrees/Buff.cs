using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "ScriptableObjects/Buff")]
public abstract class Buff : ScriptableObject
{
    [SerializeField] private string buffName;
    [SerializeField] private PlayerStats buffStats;
    [SerializeField] private float buffDuration;
    [SerializeField] private float buffCooldown;
    private float _buffTimer;
    
    private void Start()
    {
        _buffTimer = buffCooldown;
    }
    
    void Update()
    {
        CooldownHandler();
    }

    public abstract void ApplyBuff();

    private void CooldownHandler()
    {
        if (_buffTimer > 0)
        {
            _buffTimer -= Time.deltaTime;
        }
    }
    
    public string GetBuffName()
    {
        return buffName;
    }
}
