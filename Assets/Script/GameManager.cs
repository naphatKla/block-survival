using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeCountText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private List<GameObject> otherUI;
    [SerializeField] private TextMeshProUGUI currencyTextOnWin;
    [SerializeField] private TextMeshProUGUI currencyTextOnLose;
    [SerializeField] private TextMeshProUGUI enemyKilledTextOnWin;
    [FormerlySerializedAs("enemyLeftTextOnLose")] [SerializeField] private TextMeshProUGUI enemyKillTextOnLose;
    [SerializeField] private float currencyPerMin = 5f;
    [SerializeField] private float currencyOnWin = 100f;
    [SerializeField] private TMP_InputField nameInput;
    public bool saveInLeaderboard;
    private float currencySum;

    public int enemySpawned;
    public int enemyLeft;
    public float timeInGame;
    private Player _player;
    public bool isEnd;

    void Start()
    {
        _player = FindObjectOfType<Player>();
        enemyLeft = 0;
        enemySpawned = 0;
        resumeButton.onClick.AddListener(() =>
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        });
        
        howToPlayButton.onClick.AddListener(() =>
        {
            howToPlayMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
        });
        Invoke(nameof(HowToPlayPopUp),1.25f);
    }
    
    void Update()
    {
        SetTimeInGameText();
        PauseMenuHandle();

        if(isEnd) return;
        
        if ((int)timeInGame % 60 == 0 && (int)timeInGame <= 900 && (int)timeInGame > 0)
        {
            currencySum += currencyPerMin;
        }
        
        if(enemyLeft <= 0 && timeInGame >= 900)
        {
            isEnd = true;
            CareerManager.currency += currencyOnWin;
            currencyTextOnWin.text = "Currency: +" + currencyOnWin;
            enemyKilledTextOnWin.text = "Enemy Killed: " + Level.Instance.enemyKill; // enemy Kill on win
            CareerManager.Instance.SaveCareerData();
            StartCoroutine(EndScenePopUp(winMenu));
        }
        else if (_player.health <= 0)
        {
            isEnd = true;
            CareerManager.currency += currencySum;
            currencyTextOnLose.text = "Currency: +" + currencySum;
            enemyKillTextOnLose.text = "Enemy Killed: " + Level.Instance.enemyKill; // enemy Kill on lose
            CareerManager.Instance.SaveCareerData();
            StartCoroutine(EndScenePopUp(loseMenu));
        }   

        if (isEnd && saveInLeaderboard)
        {
            FirebaseRankingManager.Instance.currentPlayerDatas.playerName = nameInput.text;
            FirebaseRankingManager.Instance.currentPlayerDatas.playerKill = (int)Level.Instance.enemyKill;
            FirebaseRankingManager.Instance.AddDataWithSorting();
        }
    }
    
    private void SetTimeInGameText()
    {
        timeInGame += Time.deltaTime;
        timeCountText.text = "" + Mathf.FloorToInt(timeInGame / 60) + ":" + Mathf.FloorToInt(timeInGame % 60).ToString("00");
    }

    private void HowToPlayPopUp()
    {
        StartCoroutine(HowToPlayPop());
    }
    private void PauseMenuHandle()
    {
        if(timeInGame < 1.25f) return;
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        
    }

    IEnumerator HowToPlayPop()
    {
        Animator animator = howToPlayMenu.GetComponent<Animator>();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            foreach (var ui in otherUI)
            {
                ui.SetActive(false);
            }
            howToPlayMenu.SetActive(true);
            Time.timeScale = 1;
            yield return null;
        }
        
        while(howToPlayMenu.activeSelf && isEnd == false)
        {
            Time.timeScale = 0;
            yield return null;
        }
    }
    IEnumerator EndScenePopUp(GameObject endScene)
    {
        nameInput.gameObject.SetActive(true);
        while (endScene.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            foreach (var ui in otherUI)
            {
                ui.SetActive(false);
            }
            endScene.SetActive(true);
            Time.timeScale = 1;
            yield return null;
        }
        Time.timeScale = 0;
    }
}
