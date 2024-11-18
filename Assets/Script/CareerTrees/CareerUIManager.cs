using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CareerUIManager : MonoBehaviour
{
    void Start()
    {
        CareerManager.Instance.LoadSaveCareer();
    }
}
