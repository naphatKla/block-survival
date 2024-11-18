using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public struct PlayerStats
{
    public float health;
    public float attackDamage;
    public float attackSpeed;
    public float movementSpeed;
    
    public void PrintStats()
    {
        Debug.Log($"Health: {health}");
        Debug.Log($"Attack Damage: {attackDamage}");
        Debug.Log($"Attack Speed: {attackSpeed}");
        Debug.Log($"Movement Speed: {movementSpeed}");
    }
}

public class Career : MonoBehaviour
{
    [SerializeField] protected string careerName;
    [SerializeField] protected string careerDescription;
    [SerializeField] protected PlayerStats statsImprovement;
    public float currencyCost;
    [SerializeField] protected List<Buff> buffs;
    [SerializeField] protected Career careerLeft;
    [SerializeField] protected Career careerRight;
    protected Career careerParent;
    public bool IsUnlocked { get; private set; }
    private bool _canUnlock;
    private Image _image;

    private void Awake()
    {
        if (careerLeft) careerLeft.SetParent(this);
        if (careerRight) careerRight.SetParent(this);
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        if (careerParent) return; 
        _canUnlock = true; // if it's root career
        CareerManager.Instance.SetRootCareer(this);
    }

    private void Update()
    {
        DebugState();
    }

    
    public void SetParent(Career parent)
    {
        careerParent = parent;
    }
    
    public void SetCanUnlock(bool canUnlock)
    {
        _canUnlock = canUnlock;
    }
    
    [PropertySpace(10)] [Button(ButtonSizes.Medium)]
    public void Unlock()
    {
        if (!_canUnlock) return;
        if (IsUnlocked) return;
        if (CareerManager.currency < currencyCost)
        {
            Debug.LogWarning("Not enough currency");
            return;
        }
        IsUnlocked = true;
        careerLeft?.SetCanUnlock(true);
        careerRight?.SetCanUnlock(true);

        // if it's not root career
        if (careerParent)
        {
            if (careerParent.careerLeft && this == careerParent.careerLeft)
            {
                careerParent.careerRight?.SetCanUnlock(false);
                CareerManager.savedUnlockPath.Enqueue(0); // 0 means left
            }
            else if (careerParent.careerRight && this == careerParent.careerRight)
            {
                careerParent.careerLeft?.SetCanUnlock(false);
                CareerManager.savedUnlockPath.Enqueue(1); // 1 means right
            }
        }
        
        CareerManager.currency -= currencyCost;
        CareerManager.Instance.SetCurrentCareer(this);
    }
    
    public void UnlockFromSave()
    {
        if (!_canUnlock) return;
        if (IsUnlocked) return;
        IsUnlocked = true;
        careerLeft?.SetCanUnlock(true);
        careerRight?.SetCanUnlock(true);

        // if it's not root career
        if (careerParent)
        {
            if (careerParent.careerLeft && this == careerParent.careerLeft)
            {
                careerParent.careerRight?.SetCanUnlock(false);
                CareerManager.savedUnlockPath.Enqueue(0); // 0 means left
            }
            else if (careerParent.careerRight && this == careerParent.careerRight)
            {
                careerParent.careerLeft?.SetCanUnlock(false);
                CareerManager.savedUnlockPath.Enqueue(1); // 1 means right
            }
        }

        CareerManager.Instance.SetCurrentCareer(this);
    }

    public void Reset()
    {
        IsUnlocked = false;
        _canUnlock = false;
        if (careerParent) return; 
        _canUnlock = true; // if it's root career
        CareerManager.Instance.SetRootCareer(this);
    }
    
    public Career GetParent()
    {
        return careerParent;
    }
    
    public Career GetLeftCareer()
    {
        return careerLeft;
    }
    
    public Career GetRightCareer()
    {
        return careerRight;
    }
    
    public PlayerStats GetStats()
    {
        return statsImprovement;
    }
    
    public List<Buff> GetBuffs()
    {
        return buffs;
    }
    
    // might be change later
    private void DebugState()
    {
        Color color = _image.color;
        color.a = IsUnlocked ? 1 : 0.25f;
        _image.color = color;
    }
    
    private void OnDrawGizmos()
    {
        if (careerLeft)
        {
            Gizmos.color = careerLeft.IsUnlocked ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, careerLeft.transform.position);
        }
        if (careerRight)
        {
            Gizmos.color = careerRight.IsUnlocked ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, careerRight.transform.position);
        }
    }
}
