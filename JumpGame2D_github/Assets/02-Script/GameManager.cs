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
    private GameObject OptionCanvas;
    private GameObject GameOverPanel;
    private GameObject StartGamePanel;

    private GameObject currentCanvas;
    #endregion
    [SerializeField]
    GameObject Player;

    [SerializeField]
    GameObject Background;
    [SerializeField]
    Canvas BackGroundCanvas;

    [SerializeField]
    private int Level;

    public int PlayTime;

    WaitUntil waitUntilInputKey;
    WaitUntil waitUntilLoaded;
    WaitForFixedUpdate WaitForFixedUpdate;
    WaitForSeconds timingSecond;
    WaitForSeconds waitForSecond;
    Action doAfterInputKey;
    Action doAfterInputKeyNextPer;
    Action doAfterSecondFun;
    BackgroundScroll[] backgroundScrolls; 

    bool TimerCanCount;
    
    [SerializeField]
    private Text TimerText;
    int time;
    int waitSec;

    private void Awake()
    {
        Init();
        StartCoroutine(DownloadAssetBundle());
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
        waitUntilLoaded = new WaitUntil(() => DownLoadAssetBundle.Instance.www1.isDone);
        timingSecond = new WaitForSeconds(1);
        waitForSecond = new WaitForSeconds(1);
        backgroundScrolls = new BackgroundScroll[2];
        time = PlayTime;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ResestAssetBundle();
        }
    }

    IEnumerator InstallImage()
    {
        ResestAssetBundle();

        DownLoadAssetBundle.Instance.LoadAssetBundle(AssetBundleState.Images, "atlas");
        //Debug.Log("a");

        yield return waitUntilLoaded;
        DownLoadAssetBundle.Instance.LoadAssetBundle(AssetBundleState.Prefab, "gameobjectbundle");
        //Debug.Log("b");

        yield return waitUntilLoaded;
        DownLoadAssetBundle.Instance.LoadAssetBundle(AssetBundleState.UIPrefab, "uibundle");

        yield return waitUntilLoaded;
        //Debug.Log("c");

        //Debug.Log("AssetBundle is DownLoade prefab");
        GetPrefab();
        InstantiateObject();

    }

    private void GetPrefab()
    {
        //Debug.Log("s");

        Player = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "Water", typeof(GameObject));
        Background = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.UIPrefab, "background", typeof(GameObject));
        OptionCanvas = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.UIPrefab, "UICanvas", typeof(GameObject));
        ObstacleController.Instance.GetPrefab();
    }

    public void InstantiateObject()
    {
        Instantiate(Player);
        Background = Instantiate(Background, BackGroundCanvas.transform.GetChild(0).transform);
        currentCanvas = Instantiate(OptionCanvas);

        StartCoroutine(SetScene());

    }

    IEnumerator SetScene()
    {
        yield return null;
        GameOverPanel = currentCanvas.transform.GetChild(0).gameObject;
        StartGamePanel = currentCanvas.transform.GetChild(1).gameObject;
        TimerText = currentCanvas.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
        backgroundScrolls[0] = Background.transform.GetChild(0).gameObject.GetComponent<BackgroundScroll>();
        backgroundScrolls[1] = Background.transform.GetChild(1).gameObject.GetComponent<BackgroundScroll>();

        ResetScene();

    }

    public void ResetScene()
    {
      //  Debug.Log("2.get player script");
        PlayerBehaviour.Instance.ResetPlayer();
        PlayerBehaviour.Instance.SwitchControlPlayer(false);
        PlayerBehaviour.Instance.Animator.enabled = false;

       // Time.timeScale = 0;
        CurrentGameState = GameState.Start;
        ObstacleController.Instance.LoadNextObstacle();
        ObstacleController.Instance.StartGame();
       // StartCoroutine(ReadyGameCroutine());
        ReadyGame();
    }
    
    private void ReadyGame()
    {    
        doAfterInputKey = StartGame;
        doAfterInputKeyNextPer = PlayGameBuffer;

        StartCoroutine(WaitUntilGetKey(KeyCode.Mouse0, doAfterInputKey, doAfterInputKeyNextPer));
        
    }

    private void RestartGame()
    {
      //  Debug.Log("6");

        ObstacleController.Instance.ClearAllObstacle();

        ObstacleController.Instance.LoadNextObstacle();

        ObstacleController.Instance.StartGame();
     //   BackgroundScroll.Instance.ResetBackground();
        backgroundScrolls[0].ResetBackground();
        backgroundScrolls[1].ResetBackground();

        doAfterInputKey = ResetGame;
        doAfterInputKeyNextPer = PlayGameBuffer;

        StartCoroutine(WaitUntilGetKey(KeyCode.Mouse0, doAfterInputKey, doAfterInputKeyNextPer));
    }

    IEnumerator WaitUntilGetKey(KeyCode keyCode, Action doAfterGetKeyDown,Action doAfterInputKeyNextPer)
    {
        while (!Input.GetKeyDown(keyCode))
        {
            yield return null;

        }
        doAfterGetKeyDown();

        yield return null;
        doAfterInputKeyNextPer?.Invoke();

    }
    
    private void StartGame()
    {
        ObstacleController.Instance.StartObstacleBehiour();
        StartGamePanel.SetActive(false);
    }

    public void PlayGameBuffer()
    {
        doAfterInputKey = PlayGame;
        StartCoroutine(WaitUntilGetKey(KeyCode.Mouse0, doAfterInputKey, null));

    }

    public void PlayGame()
    {
        CurrentGameState = GameState.Play;
        ReStartTimer();
        PlayerBehaviour.Instance.Animator.enabled = true;
        PlayerBehaviour.Instance.SwitchControlPlayer(true);
    }

    public void ResetGame()
    {
        PlayerBehaviour.Instance.ResetPlayer();
        ObstacleController.Instance.StartObstacleBehiour();

        GameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        if (CurrentGameState != GameState.GameOver)
        {
            CurrentGameState = GameState.GameOver;
            StopTime();
            PlayerBehaviour.Instance.SwitchControlPlayer(false);

            ObstacleController.Instance.StopObstacleBehaviour();
           // Debug.Log("4");

            doAfterSecondFun = OpenGameOverPanel;

            StartCoroutine(DoAfterSecond(doAfterSecondFun));


        }
       

    }

    IEnumerator DoAfterSecond(Action doAfterSecond)
    {

        yield return waitForSecond;

        doAfterSecond();
    }

    public void OpenGameOverPanel()
    {
        GameOverPanel.SetActive(true);
        doAfterSecondFun = RestartGame;
     //   Debug.Log("5");

        StartCoroutine(DoAfterSecond(doAfterSecondFun));


    }
    public void ReStartTimer()
    {
        //   time = PlayTime;
        StopCoroutine(Timer());
        TimerCanCount = true;
        StartCoroutine(Timer());

    }

    public void StopTime()
    {
        TimerCanCount = false;
        time = PlayTime;
        TimerText.text = "Timer : " + Mathf.Floor(time * 0.017f) + " : " + (time % 60).ToString("0#");
        StopCoroutine(Timer());

    }

    public void NextObstacle()
    {
        PlayerBehaviour.Instance.ScrollPlayer(-7.7f, 1.2f);
     //   BackgroundScroll.Instance.ScrollBackground();
        backgroundScrolls[0].ScrollBackground();
        backgroundScrolls[1].ScrollBackground();

        ObstacleController.Instance.UnLoadCurrentObstacle();
      //  Debug.Log("1.");

        ObstacleController.Instance.UpdateCurrentObstacle();
     //   Debug.Log("2.");

        ObstacleController.Instance.ScrollObject(0, -8, 1.2f);
        ObstacleController.Instance.LoadNextObstacle();
      //  Debug.Log("3.");

    }



    IEnumerator Timer()
    {
        while (time > 0 && TimerCanCount)
        {
            time -= 1;
            TimerText.text = "Timer : " + time/60 + " : " + (time - ((time / 60 )*60)).ToString("0#");
            waitSec = 1;

            yield return timingSecond;
           

            yield return null;
        }
     //   Debug.Log(time);
        GameOver();
    }

    public void ResestAssetBundle()
    {
        DownLoadAssetBundle.Instance.UnloadAllAssetBundle();

    }

}
