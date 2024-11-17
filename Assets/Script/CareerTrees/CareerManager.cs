using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CareerManager : Singleton<CareerManager>
{
    private Career _currentCareer;
    private Career _rootCareer;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetRootCareer(Career rootCareer)
    {
        _rootCareer = rootCareer;
    }
    
    public void SetCurrentCareer(Career currentCareer)
    {
        _currentCareer = currentCareer;
    }
    
    public int CountAllCareerEffect(Career career)
    {
        if (_rootCareer == null) return 0;
        if (career == null) return 0;
        return CountAllCareerEffect(career.GetLeftCareer()) + CountAllCareerEffect(career.GetRightCareer()) + 1;
    }
    
    public int CountActiveCareer(Career career)
    {
        if (_rootCareer == null) return 0;
        if (career == null || _currentCareer == null) return 0;
        return CountActiveCareer(career.GetParent()) + 1;
    }
    
    public void GetAllActiveCareer(ref List<Career> careers , Career career)
    {
        if (_rootCareer == null) return;
        if (career == null || _currentCareer == null) return;
        careers.Add(career);
        GetAllActiveCareer(ref careers, career.GetParent());
    }
    
    public void ApplyALlCareerEffect()
    {
       List<Career> activeCareers = new List<Career>();
       GetAllActiveCareer(ref activeCareers, _currentCareer);
       PlayerStats sumStats = new PlayerStats();
       List<Buff> sumBuffs = new List<Buff>();
       
       foreach (Career career in activeCareers)
       {
           sumStats.health += career.GetStats().health;
           sumStats.attackDamage += career.GetStats().attackDamage;
           sumStats.attackSpeed += career.GetStats().attackSpeed;
           sumStats.movementSpeed += career.GetStats().movementSpeed;
           sumBuffs.AddRange(career.GetBuffs());
       }
       
       // Apply to player after this
    }
    
    // DebugFunction ======================================================================================================
    #region DebugFunction
    [Button(ButtonSizes.Medium), DisplayName("CountAllCareer")]
    public void DebugCountAllCareer()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This function only works in play mode");
            return;
        }
        Debug.Log(CountAllCareerEffect(_rootCareer));
    }
    
    [Button(ButtonSizes.Medium),DisplayName("CountActiveCareer")]
    public void DebugCountActiveCareer()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This function only works in play mode");
            return;
        }
        Debug.Log(CountActiveCareer(_currentCareer));
    }
    
    [Button(ButtonSizes.Medium),DisplayName("GetAllCareerEffect")]
    public void DebugApplyALlCareerEffect()
    {
        List<Career> activeCareers = new List<Career>();
        GetAllActiveCareer(ref activeCareers, _currentCareer);
        PlayerStats sumStats = new PlayerStats();
        List<Buff> sumBuffs = new List<Buff>();
       
        foreach (Career career in activeCareers)
        {
            sumStats.health += career.GetStats().health;
            sumStats.attackDamage += career.GetStats().attackDamage;
            sumStats.attackSpeed += career.GetStats().attackSpeed;
            sumStats.movementSpeed += career.GetStats().movementSpeed;
            sumBuffs.AddRange(career.GetBuffs());
        }
        
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This function only works in play mode");
            return;
        }
        sumStats.PrintStats();
        Debug.Log("Buffs : " + sumBuffs.Count);
        foreach (Buff buff in sumBuffs)
        {
            Debug.Log(buff.GetBuffName());
        }
    }
    #endregion
}
