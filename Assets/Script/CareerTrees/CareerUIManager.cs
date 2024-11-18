using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CareerUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currencyText;
    
    void Start()
    {
        CareerManager.Instance.LoadSaveCareer();
    }
    
    void Update()
    {
        currencyText.text = $"Currency: {CareerManager.currency}";
    }
}
