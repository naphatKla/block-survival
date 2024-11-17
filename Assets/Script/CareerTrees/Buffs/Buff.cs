using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    [SerializeField] private string buffName;
    [SerializeField] private PlayerStats buffStats;
    [SerializeField] private bool hasLifetime;
    [SerializeField] private bool hasCooldown;
    [ShowIf(nameof(hasLifetime))] [SerializeField] private float buffDuration;
    [ShowIf(nameof(hasCooldown))] [SerializeField] private float buffCooldown;
    public bool IsCooldown => _buffCooldownTimer > 0;

    protected Player player;
    protected CombatSystem combatSystem = CombatSystem.Instance;
    protected SkillSystem skillSystem = SkillSystem.Instance;
    protected Level level = Level.Instance;
    private float _buffCooldownTimer;
    private float _buffDurationTimer;
    private bool _isEnter;
    
    private void Start()
    {
        player = Player.Instance;
        combatSystem = CombatSystem.Instance;
        skillSystem = SkillSystem.Instance;
        level = Level.Instance;
        _buffCooldownTimer = buffCooldown;
        if (hasCooldown) _buffCooldownTimer = 0;
    }
    
    void Update()
    {
        CooldownHandler();
        if (hasCooldown && IsCooldown) return;
        if (!ApplyBuffCondition())
        {
            if (_isEnter)
            {
                OnEndBuff();
                _isEnter = false;
            }
            return;
        }
        
        if (hasLifetime && _buffDurationTimer > 0)
        {
            if (!_isEnter)
            {
                OnStartBuff();
                _isEnter = true;
            }
            OnUpdateBuff();
            _buffDurationTimer -= Time.deltaTime;
            if (_buffDurationTimer <= 0)
            {
                OnEndBuff();
                _isEnter = false;
            }
            return;
        }
        
        if (!_isEnter)
        {
            OnStartBuff();
            _isEnter = true;
        }
        
        OnUpdateBuff();
    }

    public abstract void OnStartBuff();
    public abstract void OnUpdateBuff();
    public abstract void OnEndBuff();
    public abstract bool ApplyBuffCondition();

    private void CooldownHandler()
    {
        if (_buffCooldownTimer > 0)
        {
            _buffCooldownTimer -= Time.deltaTime;
            return;
        }
        _buffCooldownTimer = buffCooldown;
    }
    
    public string GetBuffName()
    {
        return buffName;
    }
}
