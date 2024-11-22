using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using DG.Tweening;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CareerSaveData
{
    public List<int> savedUnlockPath = new List<int>();
    public bool isCareerTreeUnlocked = false;
    public PlayerStats sumOfStats = new PlayerStats();
    public List<Buff> sumOfBuffs = new List<Buff>();
    public float currency;
}

public class CareerManager : PersistentSingleton<CareerManager>
{
    public static float currency = 1000f;
    public static Queue<int> savedUnlockPath = new Queue<int>();
    private static bool isCareerTreeUnlocked = false;
    private static PlayerStats sumOfStats = new PlayerStats();
    private static List<Buff> sumOfBuffs = new List<Buff>();
    private string savePath => Path.Combine(Application.persistentDataPath, "CareerSaveData.json");
    
    private Career _currentCareer;
    private Career _rootCareer;
    
    void Awake()
    {
        LoadSaveData();
    }
    
    public void SetRootCareer(Career rootCareer)
    {
        _rootCareer = rootCareer;
    }
    
    public void SetCurrentCareer(Career currentCareer)
    {
        _currentCareer = currentCareer;
        // save data
        sumOfStats.health += _currentCareer.GetStats().health;
        sumOfStats.attackDamage += _currentCareer.GetStats().attackDamage;
        sumOfStats.attackSpeed += _currentCareer.GetStats().attackSpeed;
        sumOfStats.movementSpeed += _currentCareer.GetStats().movementSpeed;
        sumOfBuffs.AddRange(_currentCareer.GetBuffs());
        isCareerTreeUnlocked = true;
        SaveCareerData();
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
    
    public void GetAllActiveCareer(ref List<Career> careers , Career career)
    {
        if (_rootCareer == null) return;
        if (career == null || _currentCareer == null) return;
        careers.Add(career);
        GetAllActiveCareer(ref careers, career.GetParent());
    }
    
    public void ApplyALlCareerEffect()
    {
       Player.Instance.AddBuffStats(sumOfStats);
       
       GameObject buffParent = new GameObject("BuffParent");
       buffParent.transform.SetParent(Player.Instance.transform);
       foreach (Buff buff in sumOfBuffs)
       {
           Instantiate(buff).gameObject.transform.SetParent(buffParent.transform);
       }
    }

    public PlayerStats GetSumOfStats()
    {
        return sumOfStats;
    }

    public List<Buff> GetSumOfBuffs()
    {
        return sumOfBuffs;
    }
    
    public void LoadSaveCareer()
    {
        StartCoroutine(LoadSave());
    }

    private void SaveCareerData()
    {
        CareerSaveData saveData = new CareerSaveData();
        saveData.savedUnlockPath = new List<int>(savedUnlockPath);
        saveData.isCareerTreeUnlocked = isCareerTreeUnlocked;
        saveData.sumOfStats = sumOfStats;
        saveData.sumOfBuffs = sumOfBuffs;
        saveData.currency = currency;
        
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json); 
        Debug.Log("Game Data Saved to: " + savePath);
    }

    private void LoadSaveData()
    {
        if (!File.Exists(savePath)) return;
        string json = File.ReadAllText(savePath);
        CareerSaveData saveData = JsonUtility.FromJson<CareerSaveData>(json);
        savedUnlockPath = new Queue<int>(saveData.savedUnlockPath);
        isCareerTreeUnlocked = saveData.isCareerTreeUnlocked;
        sumOfStats = saveData.sumOfStats;
        sumOfBuffs = saveData.sumOfBuffs;
        currency = saveData.currency;
    }
    
    private IEnumerator LoadSave()
    {
        yield return new WaitForNextFrameUnit();
        if (!isCareerTreeUnlocked) yield break;
        if (!_rootCareer) yield break;
        Queue<int> unlockPath = new Queue<int>(savedUnlockPath);
        Career currentCareer = _rootCareer;
        _rootCareer.UnlockFromSave();

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
                currentCareer.UnlockFromSave();
        }
    }
    
    // DebugFunction ======================================================================================================
    #region DebugFunction
    [Button(ButtonSizes.Medium), DisplayName("CountAllCareer")]
    private void LogCountAllCareer()
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
    private void LogCountActiveCareer()
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
    private void LogAllCareerEffect()
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

    [FoldoutGroup("DangerZone")] [Button(ButtonSizes.Medium), GUIColor("red")]
    public void ResetAllCareer()
    {
        List<Career> activeCareers = new List<Career>();
        GetAllActiveCareer(ref activeCareers, _currentCareer);
        float refundCurrency = 0;
        foreach (Career career in activeCareers)
        {
            career.Reset();
            refundCurrency += career.currencyCost;
        }
        _currentCareer = null;
        sumOfStats = new PlayerStats();
        sumOfBuffs = new List<Buff>();
        isCareerTreeUnlocked = false;
        savedUnlockPath.Clear();
        currency += refundCurrency / 2f;
        
        SaveCareerData();
    }
    
    [FoldoutGroup("DangerZone")] [Button(ButtonSizes.Medium), GUIColor("red")]
    private void DeleteAllSaveData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Delete Save Data");
        }
    }
    
    [PropertySpace(SpaceBefore = 15f)] [Button]
    private void LoadSceneToGameplay()
    {
        SceneManager.LoadScene("GamePlayShaoKuyToMobile");
    }

    [Button]
    private void LoadSceneToCareerTree()
    {
        SceneManager.LoadScene("Scenes/Career/TestCareer");
    }
    #endregion
}
