using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Start,
        Play,
        GameOver
        
    }

    public GameState CurrentGameState;

    [SerializeField]
    private Vector3 PlayerStartPos;

    #region Panel
    [SerializeField]
    private GameObject[] UICanvas;

    [SerializeField]
    private CanvasGroup GuestMarkCanvas;

    [SerializeField]
    private CanvasGroup Logo;

    #endregion
    [SerializeField]
    GameObject Player;

    [SerializeField]
    GameObject Background;
    [SerializeField]
    Canvas BackGroundCanvas;

    [SerializeField]
    private int Level;

    //public GameObject audioController;

    public int PlayTime;

    WaitUntil waitUntilInputKey;
    WaitForFixedUpdate WaitForFixedUpdate;
    WaitForSeconds timingSecond;
    WaitForSeconds waitForSecond;
    Action doAfterInputKey;
    Action doAfterInputKeyNextPer;
    Action doAfterSecondFun;
    private BackgroundScroll[] backgroundScrolls;
    public int CurrentBackground;
    public int BackgroundColor;

    int timer_Minute;
    int timer_Second;

    string player_name;
    string game_sn;
    
    public int time;
    int waitSec;

//    public int currentStory;

    IEnumerator timerCoroutine;

    private void Awake()
    {
        Init();
        Application.ExternalCall("LoadData");
        StartCoroutine(DownloadAssetBundle());
    }

    private void Start()
    {
        LogoControll();
    }

    private IEnumerator DownloadAssetBundle()
    {
        yield return null;
        StartCoroutine(InstallImage());
    }

    public void Init()
    {
        Instance = this;

        WaitForFixedUpdate = new WaitForFixedUpdate();
        timingSecond = new WaitForSeconds(1);
        waitForSecond = new WaitForSeconds(2);
        backgroundScrolls = new BackgroundScroll[2];
        CurrentBackground = 1;
        timerCoroutine = Timer();

    }

    #region 初始化與載入資源
    IEnumerator InstallImage()
    {

        DownLoadAssetBundle.Instance.UnloadAllAssetBundle();
        DownLoadAssetBundle.Instance.LoadAssetBundle(AssetBundleState.Prefab, "gameobject");

        while (!DownLoadAssetBundle.Instance.request.isDone)
        {
            yield return null;
        }
        yield return null;

       /* DownLoadAssetBundle.Instance.LoadAssetBundle(AssetBundleState.Prefab, "gameobjectbundle");
        while (!DownLoadAssetBundle.Instance.request.isDone)
        {
            yield return null;
        }
        yield return null;*/

        GetPrefab();
        InstantiateObject();

    }

    private void GetPrefab()//******************************************************************************要改ㄉ地方
    {
        Player = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "Water", typeof(GameObject));
        Background = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "background2", typeof(GameObject));
        UICanvas[0]  = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "StaticCanvas", typeof(GameObject));
        UICanvas[1] = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "MovableCanvas", typeof(GameObject));
        UICanvas[2] = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "ScoreCanvas", typeof(GameObject));
        ObstacleController.Instance.GetPrefab();
    }

    public void InstantiateObject()
    {
        Instantiate(Player);
        Background = Instantiate(Background, BackGroundCanvas.transform);
        UICanvas[0] = Instantiate(UICanvas[0]);
        UICanvas[1] = Instantiate(UICanvas[1]);
        UICanvas[2] = Instantiate(UICanvas[2]);
        
        StartCoroutine(SetGameObject());

    }

    IEnumerator SetGameObject()
    {
        yield return null;

        backgroundScrolls[0] = Background.transform.GetChild(0).gameObject.GetComponent<BackgroundScroll>();
        backgroundScrolls[1] = Background.transform.GetChild(1).gameObject.GetComponent<BackgroundScroll>();

        UIController.instance.CreatDictionary(UICanvas);
        UIController.instance.CloseAllPanel();

        StageDataController.Instance.SetData();
        SetScene();
        
    }

    public void SetScene()//******************************************************************************要改ㄉ地方
    {
        /* SetGuestMark();//******************************************************************************要改ㄉ地方
         GetPlayerInfo_PlayerID("s1414042214@gms.nutc.edu.tw");//******************************************************************************要改ㄉ地方
         SetPlayerInfo_GameID("8+95+5dfg654654dfg");//******************************************************************************要改ㄉ地方
         Application.ExternalCall("GetPlayerInfo");*/

        SetPlayerInfo();
        time = PlayTime;

        PlayerBehaviour.Instance.ResetPlayer();    
        PlayerBehaviour.Instance.enabled = false;

        CurrentGameState = GameState.Start;
        ObstacleController.Instance.LoadLevelData(1);
        ObstacleController.Instance.StartGame();
        ObstacleController.Instance.LoadLevelData(2);
        ObstacleController.Instance.LoadNextObstacle();
        ObstacleController.Instance.LoadLevelData(3);

        UIController.instance.ResetText();

        ResetTimer();

        OpenPrecautions();
    }

    private void StartObstacle()
    {
        ObstacleController.Instance.StartObstacleBehiour();
        PlayerBehaviour.Instance.enabled = true;

    }

    #endregion

    
    public void SetGuestMark()
    {
        GuestMarkCanvas.alpha = 1;
    }

    public void SetPlayerInfo()
    {
        UIController.instance.SetPlayerInfo_PlayerInfo(player_name);
        UIController.instance.SetPlayerInfo_GameID(game_sn);
    }

    public void GetPlayerInfo_PlayerID(string playerID)//------------------被JS呼叫
    {
        player_name = playerID;
    }

    public void GetPlayerInfo_GameID(string gameID)//------------------被JS呼叫
    {
        game_sn = gameID;
    }

    public void LogoControll()
    {
        StartCoroutine(Logofade());


    }

    IEnumerator Logofade()
    {

        while (Logo.alpha < 1)
        {
            Logo.alpha += 2 * Time.deltaTime;
            yield return null;

        }

        yield return new WaitForSeconds(0.5f);

        while (Logo.alpha > 0)
        {

            Logo.alpha -= 2 * Time.deltaTime;
            yield return null;

        }

    }

    public void ResetGame()
    {
        Application.ExternalCall("AudioPause");

        BackgroundColor = 0;

        ObstacleController.Instance.ClearAllObstacle();

        ObstacleController.Instance.LoadLevelData(1);
        ObstacleController.Instance.StartGame();

        ObstacleController.Instance.LoadLevelData(2);
        ObstacleController.Instance.LoadNextObstacle();
        ObstacleController.Instance.LoadLevelData(3);

        PlayerBehaviour.Instance.ResetPlayer();
        PlayerBehaviour.Instance.SwitchControlPlayer(false);
        PlayerBehaviour.Instance.enabled = false;

        StageDataController.Instance.ResetTotalScore();
        
        UIController.instance.ResetText();

        ResetTimer();

        backgroundScrolls[0].ResetBackground();
        backgroundScrolls[1].ResetBackground();

    }

    public void StartGame()
    {
        Application.ExternalCall("AudioPlay", "BGM_Play", 1, true);
        StartObstacle();
        DoAfterInput(PlayGame, null);
    }

    public void PlayGame()
    {
        CurrentGameState = GameState.Play;
        ReStartTimer();

        PlayerBehaviour.Instance.Animator.enabled = true;
        PlayerBehaviour.Instance.SwitchControlPlayer(true);
    }

    
    public void GameOver()//******************************************************************************要改ㄉ地方
    {
        if (CurrentGameState != GameState.GameOver)
        {
            CurrentGameState = GameState.GameOver;

            StopTime();

            PlayerBehaviour.Instance.SwitchControlPlayer(false);
            PlayerBehaviour.Instance.enabled = false;

            ObstacleController.Instance.StopObstacleBehaviour();

            doAfterSecondFun = SendScoreToServer;

         //   doAfterSecondFun = OpenLosePanel;//******************************************************************************要改ㄉ地方

            StartCoroutine(DoAfterSecond(doAfterSecondFun));

        }

    }

    public void SendScoreToServer()
    {
        Application.ExternalCall("GetFinalScore", StageDataController.Instance.TotalScore);

    }

    public void NextObstacle()
    {
        PlayerBehaviour.Instance.ScrollPlayer(-7.7f, 1.2f);

        switch (backgroundScrolls[CurrentBackground].position_Y)
        {
            case -1290:
                backgroundScrolls[0].ScrollBackground();
                backgroundScrolls[1].ScrollBackground();

                BackgroundColor = -1;
                break;

            default:
                backgroundScrolls[CurrentBackground].ScrollBackground();

                break;
        }

        switch (BackgroundColor)
        {
            case 2:
                UIController.instance.SetPlayTextColor(3);
                BackgroundColor = -1;

                break;

            default:
                BackgroundColor += 1;
                UIController.instance.SetPlayTextColor(BackgroundColor);

                break;
        }

        ObstacleController.Instance.UnLoadCurrentObstacle();
        ObstacleController.Instance.UpdateCurrentObstacle();
        ObstacleController.Instance.ScrollObject(0, -7, 1.2f);
        ObstacleController.Instance.LoadNextObstacle();

    }

    IEnumerator WaitUntilGetKey(KeyCode keyCode, Action doAfterGetKeyDown, Action doAfterInputKeyNextPer)
    {
        while (!Input.GetKeyDown(keyCode))
        {
            yield return null;

        }
        doAfterGetKeyDown();

        yield return null;
        doAfterInputKeyNextPer?.Invoke();

    }

    public void DoAfterInput(Action doAfterInputKey, Action doAfterInputKeyNextPer)
    {
        StartCoroutine(WaitUntilGetKey(KeyCode.Mouse0, doAfterInputKey, doAfterInputKeyNextPer));
    }

    IEnumerator DoAfterSecond(Action doAfterSecond)
    {
        yield return waitForSecond;

        doAfterSecond();
    }

    #region UI們

    public void OpenPrecautions()
    {
        UIController.instance.OpenPrecautions();
    }

    #region 扣點欄
    public void OpenConsumPointPanel()//被javaScript呼叫,打開消耗點數的頁面
    {
        UIController.instance.OpenStartPanel(1);
    }

    public void SetConsumePoint(int consumeOncePoint)//被javaScript呼叫,設置需消耗的點數
    {
        UIController.instance.SetNumber(2, consumeOncePoint, 4, true);
        UIController.instance.SetNumber(0, consumeOncePoint, 4, true);
    }

    public void SetPlayerPoint(int playerPoint)//被javaScript呼叫,設置玩家的點數
    {
        UIController.instance.SetNumber(1, playerPoint, 4, true);
    }
    #endregion

    #region 免費欄
    public void OpenFreePanel()//被javaScript呼叫,打開免費遊玩的畫面
    {
        UIController.instance.OpenStartPanel(0);      
    }


    public void SetPresetFreeCount(int presetFreeCount)//被javaScript呼叫,設置預設的免費遊園次數
    {
        UIController.instance.SetNumber(2, presetFreeCount, 4, true);
    }

    public void SetPlayerFreeTimes(int playerFreeCount)//被javaScript呼叫,設置玩家的免費遊玩次數
    {
        UIController.instance.SetNumber(1, playerFreeCount, 4, true);
    }
    #endregion

    public void StartStory()//被javaScript呼叫,開始故事及新手教學
    {
        UIController.instance.UIStartStory();
    }

    public void OpenWinPanel()//被javaScript呼叫//******************************************************************************要改ㄉ地方
    {
        UIController.instance.OpenResultPanel(0);
        Application.ExternalCall("AudioPlay", "BGM_Story", 0.3f, true);
        Application.ExternalCall("AudioPlay","Win", 1f, false);
     //   OpenReceiveButton();//******************************************************************************要改ㄉ地方
    }

    public void OpenLosePanel()//被javaScript呼叫 //******************************************************************************要改ㄉ地方
    {
        UIController.instance.OpenResultPanel(1);
        Application.ExternalCall("AudioPlay", "BGM_Story", 0.3f, true);
        Application.ExternalCall("AudioPlay", "Lose", 0.8f, false);

     //   OpenAgainButton();//******************************************************************************要改ㄉ地方
    }

    public void OpenAgainButton()//被javaScript呼叫
    {
        UIController.instance.OpenAgainButton();
    }

    public void OpenReceiveButton()//被javaScript呼叫
    {
        UIController.instance.OpenReceiveButton();
    }

    #endregion


    #region 計時

    public void ResetTimer()
    {
        time = PlayTime;
        ShowTime();

    }

    public void ReStartTimer()
    {
        StopCoroutine(timerCoroutine);

        time = PlayTime;
        ShowTime();

        timerCoroutine = Timer();
        StartCoroutine(timerCoroutine);

    }

    public void StopTime()
    {
        StopCoroutine(timerCoroutine);
        
    }

    IEnumerator Timer()
    {
        while (time > 0)
        {
            time -= 1;
            ShowTime();

            waitSec = 1;

            yield return timingSecond;
           
            yield return null;
        }

        Application.ExternalCall("AudioPlay", "TimesUP", 1, false);
        UIController.instance.OpenTimesUP();
        GameOver();
    }

    public void ShowTime()
    {
        timer_Minute = time / 60;
        timer_Second = time - ((time / 60) * 60);
        UIController.instance.SetTime(timer_Minute, timer_Second);
    }

    public void ResestAssetBundle()
    {
        DownLoadAssetBundle.Instance.UnloadAllAssetBundle();
    }
    
    #endregion

  
}
