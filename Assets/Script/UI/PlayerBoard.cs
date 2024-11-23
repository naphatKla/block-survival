using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBoard : MonoBehaviour
{
    public PlayerDatas playerDatas_;
    public TextMeshProUGUI playerRank;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI kills;

    public void UpdateData()
    {
        playerRank.text = playerDatas_.playerRank.ToString();
        playerName.text = playerDatas_.playerName;
        kills.text = playerDatas_.playerKill.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
