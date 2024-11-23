using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    [SerializeField] private string buffName;
    [SerializeField] [TextArea(15,15)] private string buffDescription;
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
    protected bool isEnter;
    private bool _isEnterFirstTime;
    
    protected virtual void Start()
    {
        player = Player.Instance;
        combatSystem = CombatSystem.Instance;
        skillSystem = SkillSystem.Instance;
        level = Level.Instance;
        _buffCooldownTimer = buffCooldown;
        _isEnterFirstTime = true;
        if (hasCooldown) _buffCooldownTimer = 0;
    }
    
    protected virtual void Update()
    {
        CooldownHandler();
        if (hasCooldown && IsCooldown) return;
        if (!ApplyBuffCondition())
        {
            if (isEnter)
            {
                OnEndBuff();
                isEnter = false;
            }
            return;
        }
        
        if (hasLifetime && _buffDurationTimer > 0)
        {
            if (!isEnter)
            {
                OnStartBuff();
                if (_isEnterFirstTime)
                {
                    Debug.LogWarning("Enter First Time");
                    _isEnterFirstTime = false;
                    OnStartBuffFirstTime();
                }
                isEnter = true;
            }
            OnUpdateBuff();
            _buffDurationTimer -= Time.deltaTime;
            if (_buffDurationTimer <= 0)
            {
                OnEndBuff();
                isEnter = false;
            }
            return;
        }
        
        if (!isEnter)
        {
            OnStartBuff();
            if (_isEnterFirstTime)
            {
                Debug.LogWarning("Enter First Time");
                _isEnterFirstTime = false;
                OnStartBuffFirstTime();
            }
            isEnter = true;
        }
        
        OnUpdateBuff();
    }

    public abstract void OnStartBuffFirstTime();
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
    public string GetBuffDescription()
    {
        return buffDescription;
    }
}
