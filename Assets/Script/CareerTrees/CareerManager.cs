using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CareerManager : PersistentSingleton<CareerManager>
{
    public static Queue<int> savedUnlockPath = new Queue<int>();
    public static bool isCareerTreeUnlocked = false;
    private Career _currentCareer;
    private Career _rootCareer;
    
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    
    [Button]
    public void TestChangeSceneToGameplay()
    {
        SceneManager.LoadScene("GamePlayShaoKuyToMobile");
    }

    [Button]
    public void ReloadScene()
    {
        SceneManager.LoadScene("Scenes/Career/TestCareer");
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
    
    public void LoadSaveCareer()
    {
        StartCoroutine(LoadSave());
    }

    private IEnumerator LoadSave()
    {
        yield return new WaitForNextFrameUnit();
        if (!isCareerTreeUnlocked) yield break;
        if (!_rootCareer) yield break;
        Queue<int> unlockPath = new Queue<int>(savedUnlockPath);
        Career currentCareer = _rootCareer;
        _rootCareer.Unlock();

        savedUnlockPath.Clear();
        while (unlockPath.Count > 0)
        {
            int path = unlockPath.Dequeue();
            switch (path)
            {
                case 0:
                    currentCareer = currentCareer.GetLeftCareer();
                    break;
                case 1:
                    currentCareer = currentCareer.GetRightCareer();
                    break;
            }
            if (currentCareer != null)
                currentCareer.Unlock();
        }
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
