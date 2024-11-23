using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    
    public GameObject rankDataPrefab;
    public Transform rankPanel;

    public List<PlayerDatas> playerDatas = new List<PlayerDatas>();
    public List<GameObject> createdPlayerDatas = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        ReloadRankData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRankData()
    {
        for (int i = 0; i < playerDatas.Count; i++)
        {
            GameObject rankObj = Instantiate(rankDataPrefab,rankPanel) as GameObject;
            PlayerBoard rankData = rankObj.GetComponent<PlayerBoard>();
            rankData.playerDatas_ = new PlayerDatas(playerDatas[i].playerRank
                , playerDatas[i].playerName, playerDatas[i].playerKill, null);

            rankData.UpdateData();
            createdPlayerDatas.Add(rankObj);
        }
    }

    private void SortRankData()
    {
        
        List<PlayerDatas> sortRankPlayers = new List<PlayerDatas>();
        sortRankPlayers = playerDatas.OrderByDescending(data => data.playerKill).ToList();

        for (int i = 0;i < sortRankPlayers.Count; i++)
        {
            PlayerDatas changedRankNum = sortRankPlayers[i];
            changedRankNum.playerRank = i + 1;

            sortRankPlayers[i] = changedRankNum;
        }

        playerDatas = sortRankPlayers;
       

    }

    private void ClearRankData()
    {
        foreach (GameObject createdData in createdPlayerDatas)
        {
            Destroy(createdData);
        }
        createdPlayerDatas.Clear();
    }

    [ContextMenu("Reload")]
    public void ReloadRankData()
    {
        ClearRankData();
        SortRankData();
        CreateRankData();
    }
}
