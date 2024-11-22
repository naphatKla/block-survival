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
    [FoldoutGroup("Panel")] [SerializeField] private GameObject homePanel;
    [FoldoutGroup("Panel")] [SerializeField] private GameObject careerPathPanel;
    [FoldoutGroup("Panel")] [SerializeField] private GameObject statsPanel;
    [FoldoutGroup("Panel")] [SerializeField] private TextMeshProUGUI HeaderPanel;
    
    [FoldoutGroup("MainButton")] [SerializeField] private Button homeButton;
    [FoldoutGroup("MainButton")] [SerializeField] private Button careerPathButton;
    [FoldoutGroup("MainButton")] [SerializeField] private Button statsButton;
    
    [FoldoutGroup("CareerPathPanel")] [SerializeField] private TextMeshProUGUI careerPathHeader;
    [FoldoutGroup("CareerPathPanel")] [SerializeField] private TextMeshProUGUI careerPathDescription;
    [FoldoutGroup("CareerPathPanel")] [SerializeField] private TextMeshProUGUI careerPathPrice;
    [FoldoutGroup("CareerPathPanel")] [SerializeField] TextMeshProUGUI currencyText;
    [FoldoutGroup("CareerPathPanel")] [SerializeField] private Button buyButton;
    [FoldoutGroup("CareerPathPanel")] [SerializeField] private Button resetButton;
    
    [FoldoutGroup("HomePanel")] [SerializeField] private Button playButton;
    
    
    
    void Start()
    {
        if (!careerPathPanel.activeSelf)
        {
            careerPathPanel.SetActive(true);
            CareerManager.Instance.OnLoadSaveDataDone.AddListener( () =>careerPathPanel.SetActive(false));
        }
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
        careerPathHeader.text = career.careerName;
        careerPathDescription.text = career.careerDescription;
        careerPathPrice.text = $"Price : {career.currencyCost.ToString()}";
        List<Buff> buffs = career.GetBuffs();
        if (buffs.Count != 0)
        {
            
        }

        buyButton.onClick.AddListener(career.Unlock);
    }

    public void ResetStats()
    {
        CareerManager.Instance.ResetAllCareer();
    }

    private void CloseAllPanel()
    {
        homePanel.SetActive(false);
        statsPanel.SetActive(false);
        careerPathPanel.SetActive(false);
    }
    public void HomeButtonSelected()
    {
        CloseAllPanel();
        homePanel.SetActive(true);
        HeaderPanel.text = "Home";
    }
    public void StatButtonSelected()
    {
        CloseAllPanel();
        statsPanel.SetActive(true);
        HeaderPanel.text = "Stats";
    }
    public void CareerPathButtonSelected()
    {
        CloseAllPanel();
        careerPathPanel.SetActive(true);
        HeaderPanel.text = "Career Path";
    }
}
