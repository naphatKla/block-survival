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
    private static bool isCareerTreeUnlocked = false;
    private static PlayerStats sumOfStats = new PlayerStats();
    private static List<Buff> sumOfBuffs = new List<Buff>();
    
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
        sumOfStats.health += _currentCareer.GetStats().health;
        sumOfStats.attackDamage += _currentCareer.GetStats().attackDamage;
        sumOfStats.attackSpeed += _currentCareer.GetStats().attackSpeed;
        sumOfStats.movementSpeed += _currentCareer.GetStats().movementSpeed;
        sumOfBuffs.AddRange(_currentCareer.GetBuffs());
        isCareerTreeUnlocked = true;
    }
    
    private int CountAllCareerEffect(Career career)
    {
        if (_rootCareer == null) return 0;
        if (career == null) return 0;
        return CountAllCareerEffect(career.GetLeftCareer()) + CountAllCareerEffect(career.GetRightCareer()) + 1;
    }
    
    private int CountActiveCareer(Career career)
    {
        if (_rootCareer == null) return 0;
        if (career == null || _currentCareer == null) return 0;
        return CountActiveCareer(career.GetParent()) + 1;
    }
    
    public void ApplyALlCareerEffect()
    {
       Player.Instance.AddBuffStats(sumOfStats);
       
       GameObject buffParent = new GameObject("BuffParent");
       foreach (Buff buff in sumOfBuffs)
       {
           Instantiate(buff).transform.SetParent(buffParent.transform);
       }
       buffParent.transform.SetParent(Player.Instance.transform);
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
        sumOfStats = new PlayerStats();
        sumOfBuffs = new List<Buff>();
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
    public void LogCountAllCareer()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This function only works in play mode");
            return;
        }
        Debug.Log("All Career Count: "+CountAllCareerEffect(_rootCareer));
        Debug.Log("====================================");
    }
    
    [Button(ButtonSizes.Medium),DisplayName("CountActiveCareer")]
    public void LogCountActiveCareer()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This function only works in play mode");
            return;
        }
        Debug.Log("Active Career Count: "+CountActiveCareer(_currentCareer));
        Debug.Log("====================================");
    }
    
    [Button(ButtonSizes.Medium),DisplayName("ApplyAllCareerEffect")]
    public void LogAllCareerEffect()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("This function only works in play mode");
            return;
        }
        
        sumOfStats.PrintStats();
        Debug.Log("Buffs : " + sumOfBuffs.Count);
        foreach (Buff buff in sumOfBuffs)
        {
            Debug.Log(buff.GetBuffName());
        }
        Debug.Log("====================================");
    }
    #endregion
}
