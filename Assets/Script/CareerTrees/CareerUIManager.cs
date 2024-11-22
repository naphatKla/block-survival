using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CareerUIManager : MonoSingleton<CareerUIManager>
{
    [SerializeField] private TextMeshProUGUI descriptionHeader;
    [SerializeField] private TextMeshProUGUI descriptionDescription;
    [SerializeField] private TextMeshProUGUI descriptionPrice;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button homeButton;
    
    void Start()
    {
        CareerManager.Instance.LoadSaveCareer();
    }
    
    void Update()
    {
        currencyText.text = $"Currency: {CareerManager.currency}";
    }

    public void OnSelect(Career career)
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
