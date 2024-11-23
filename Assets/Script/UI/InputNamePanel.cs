using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputNamePanel : MonoBehaviour
{
    public TMP_InputField inputName;
    void Start()
    {
        inputName = GetComponent<TMP_InputField>();
        inputName.onValueChanged.AddListener((string text) =>
        {
            FirebaseRankingManager.Instance.ChangeName(text);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerInput()
    {
        
    }
    
}
