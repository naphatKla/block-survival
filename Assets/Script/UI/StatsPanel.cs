using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatsPanel : MonoSingleton<StatsPanel>
{
    [FoldoutGroup("StatPanel")] [SerializeField] private TextMeshProUGUI health;
    [FoldoutGroup("StatPanel")] [SerializeField] private TextMeshProUGUI movementSpeed;
    [FoldoutGroup("StatPanel")] [SerializeField] private TextMeshProUGUI attackDamage;
    [FoldoutGroup("StatPanel")] [SerializeField] private TextMeshProUGUI attackSpeed;
    [FoldoutGroup("StatPanel")] [SerializeField] private TextMeshProUGUI statDescription;
    
    void Start()
    {
        UpdateStat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStat()
    {
        health.text = "+ " + CareerManager.Instance.GetSumOfStats().health.ToString();
        movementSpeed.text = "+ " +CareerManager.Instance.GetSumOfStats().movementSpeed.ToString();
        attackDamage.text = "+ " + CareerManager.Instance.GetSumOfStats().attackDamage.ToString();
        attackSpeed.text = "+ " + CareerManager.Instance.GetSumOfStats().attackSpeed.ToString();
        string temp = "";
        if (CareerManager.Instance.GetSumOfBuffs().Count <= 0)
            temp = "there is no buff available!";
        
        foreach (Buff a in CareerManager.Instance.GetSumOfBuffs())
        {
            temp += $"<b><size=120%>{a.GetBuffName()}</size></b>\n{a.GetBuffDescription()}\n\n-------------------------------------------\n";
        }

        statDescription.text = temp;
    }

}
