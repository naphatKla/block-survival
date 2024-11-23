using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using SimpleJSON;
using System.Linq;
using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerDatas
{
    public string playerName;
    public int playerRank;
    public string playerMode;
    public int playerKill;

    public PlayerDatas(int rankNumber, string playernName, int playerKill, string playerMode)
    {
        this.playerRank = rankNumber;
        this.playerName = playernName;
        this.playerKill = playerKill;
        this.playerMode = playerMode;
    }
}
[System.Serializable]
public class Ranking
{
    public List<PlayerDatas> playerDatas = new List<PlayerDatas>();
}
public class FirebaseRankingManager : MonoSingleton<FirebaseRankingManager>
{
    public const string url = "https://blocksurvival-c49e5-default-rtdb.asia-southeast1.firebasedatabase.app";
    public const string secret = "NjYFQUTGtcNhl1aira3J8riIg8W28UqzVIGpJ3by";

    public LeaderboardUI LeaderboardUIManager;
    [SerializeField] public Ranking rankPlayers;
    

    public PlayerDatas currentPlayerDatas;
    
    public void ReloadSortingData()
    {
        string urlData = $"{url}/ranking/playerDatas.json?auth={secret}";
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);

            rankPlayers = new Ranking();
            rankPlayers.playerDatas = new List<PlayerDatas>();
            
            for (int i = 0; i < jsonNode.Count; i++)
            {
                rankPlayers.playerDatas.Add(new PlayerDatas(
                    jsonNode[i]["rankNumber"],
                    jsonNode[i]["playerName"],
                    jsonNode[i]["playerKill"],
                    jsonNode[i]["playerMode"]));
            }

            CalculateRankFromScore();
            SetLocalToDataBase();
            if (LeaderboardUIManager != null)
            {
                Debug.Log(LeaderboardUIManager.gameObject.name);
                LeaderboardUIManager.playerDatas = rankPlayers.playerDatas;
                LeaderboardUIManager.ReloadRankData();
            }
            
        }).Catch(error =>
        {
            Debug.Log(error.Message);
        });
    }

    public void SetLocalToDataBase()
    {
        string urlData = $"{url}/ranking.json?auth={secret}";

        // แปลงเป็น JSON
        Debug.Log(rankPlayers.playerDatas[0].playerName);
        string jsonString = JsonUtility.ToJson(rankPlayers);
        Debug.Log($"Uploading JSON: {jsonString}");

        RestClient.Put(urlData, jsonString).Then(response =>
        {
            Debug.Log("Upload Complete");
        }).Catch(error =>
        {
            Debug.LogError($"Upload Error: {error.Message}");
        });
    }

    private void CalculateRankFromScore()
    {
        List<PlayerDatas> sortRankPlayer = new List<PlayerDatas>();
        sortRankPlayer = rankPlayers.playerDatas.OrderByDescending(PlayerDatas => PlayerDatas.playerKill).ToList();
        
        for (int i = 0; i < sortRankPlayer.Count; i++)
        {
            PlayerDatas changedRankNum = sortRankPlayer[i];
            changedRankNum.playerRank = i + 1;
        
            sortRankPlayer[i] = changedRankNum;
        }
        
        rankPlayers.playerDatas = sortRankPlayer;
    }

    public void AddDataWithSorting()
    {
        string urlData = $"{url}/ranking/playerDatas.json?auth={secret}";
        
        RestClient.Get(urlData).Then(response =>
        {
            Debug.Log(response.Text);
            JSONNode jsonNode = JSONNode.Parse(response.Text);
        
            rankPlayers = new Ranking();
            rankPlayers.playerDatas = new List<PlayerDatas>();
            for (int i = 0; i < jsonNode.Count; i++)
            {
                rankPlayers.playerDatas.Add(new PlayerDatas(
                    jsonNode[i]["rankNumber"],
                    jsonNode[i]["playerName"],
                    jsonNode[i]["playerKill"],
                    jsonNode[i]["playerMode"]));
            }
        
            PlayerDatas checkPlayerData =
                rankPlayers.playerDatas.FirstOrDefault(datas => datas.playerName == currentPlayerDatas.playerName);
            int IndexOfPlayer = rankPlayers.playerDatas.IndexOf(checkPlayerData);
        
            if (checkPlayerData.playerName != null)
            {
                checkPlayerData.playerKill = currentPlayerDatas.playerKill;
                rankPlayers.playerDatas[IndexOfPlayer] = checkPlayerData;
            }
        
            else
            {
                rankPlayers.playerDatas.Add(currentPlayerDatas);
            }
        
            CalculateRankFromScore();
            SetLocalToDataBase();
            
        }).Catch(error =>
        {
            Debug.Log("error");
        });
    }
    
    
    void Start()
    {
        ReloadSortingData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeName(string name)
    {
        currentPlayerDatas.playerName = name;
    }

}
