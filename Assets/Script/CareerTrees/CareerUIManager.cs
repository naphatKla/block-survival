using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CareerUIManager : MonoSingleton<CareerUIManager>
{
    [FoldoutGroup("CareerPath")] [SerializeField] private TextMeshProUGUI descriptionHeader;
    [FoldoutGroup("CareerPath")] [SerializeField] private TextMeshProUGUI descriptionDescription;
    [FoldoutGroup("CareerPath")] [SerializeField] private TextMeshProUGUI descriptionPrice;
    [FoldoutGroup("CareerPath")] [SerializeField] TextMeshProUGUI currencyText;
    [FoldoutGroup("CareerPath")] [SerializeField] private Button buyButton;
    
    [FoldoutGroup("MainButton")] [SerializeField] private Button homeButton;
    [FoldoutGroup("MainButton")] [SerializeField] private Button careerPathButton;
    [FoldoutGroup("MainButton")] [SerializeField] private Button statsButton;
    
    void Start()
    {
        CareerManager.Instance.LoadSaveCareer();
    }
    
    void Update()
    {
        currencyText.text = $"Currency: {CareerManager.currency}";
    }

    public void OnCareerSelect(Career career)
    {
        Debug.Log(career.currencyCost);
        buyButton.onClick.RemoveAllListeners();
        descriptionHeader.text = career.careerName;
        descriptionDescription.text = career.careerDescription;
        descriptionPrice.text = $"Price : {career.currencyCost.ToString()}";
        List<Buff> buffs = career.GetBuffs();
        if (buffs.Count != 0)
        {
            
        }

        buyButton.onClick.AddListener(career.Unlock);
    }
}
