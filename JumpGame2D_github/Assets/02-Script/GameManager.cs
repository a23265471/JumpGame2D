using System.Collections;
using UnityEngine;
using System;


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
    private Canvas OptionCanvas;
    private GameObject GameOverPanel;
    private GameObject StartGamePanel;

    private Canvas currentCanvas;
    #endregion
    [SerializeField]
    GameObject Player;

    [SerializeField]
    GameObject Background;
    [SerializeField]
    Canvas BackGroundCanvas;

    [SerializeField]
    private int Level;

    WaitUntil waitUntilInputKey;
    WaitUntil waitUntilLoaded;
    WaitForFixedUpdate WaitForFixedUpdate;
    Action doAfterInputKey;
    Action doAfterInputKeyNextPer;



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
    }

    IEnumerator InstallImage()
    {
        DownLoadAssetBundle.Instance.LoadAssetBundle(AssetBundleState.Images, "loadstage");

        yield return waitUntilLoaded;
        Debug.Log("AssetBundle is DownLoade Image");
        DownLoadAssetBundle.Instance.LoadAssetBundle(AssetBundleState.Prefab, "prefab");
        yield return waitUntilLoaded;
        Debug.Log("AssetBundle is DownLoade prefab");
        GetPrefab();
        InstantiateObject();

    }

    private void GetPrefab()
    {
        Player = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "Player", typeof(GameObject));
        Background = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "BackGround", typeof(GameObject));
        //OptionCanvas = (Canvas)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, "UICanvas", typeof(Canvas));
    }
    
    public void InstantiateObject()
    {
        Instantiate(Player);
        Instantiate(Background,BackGroundCanvas.transform);
        currentCanvas = Instantiate(OptionCanvas);

        //   player.transform.position = PlayerStartPos;
        // Player.transform.localPosition = new Vector3(0, 0, 0);
        StartCoroutine(SetScene());
    }

    IEnumerator SetScene()
    {
        yield return null;
        GameOverPanel = currentCanvas.transform.GetChild(0).gameObject;
        StartGamePanel = currentCanvas.transform.GetChild(1).gameObject;
        ResetScene();

        //   Debug.Log(StartGamePanel.name);
    }

    public void ResetScene()
    {
        Debug.Log("2.get player script");
        PlayerBehaviour.Instance.ResetPlayer();
        PlayerBehaviour.Instance.SwitchControlPlayer(false);
        Time.timeScale = 0;
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
        
     /*   doAfterInputKey = PlayGame;
        StartCoroutine(WaitUntilGetKey(KeyCode.Mouse0, doAfterInputKey)); */    
       
    }

    private void RestartGame()
    {
        //  WaitUntilJudgment(Input.GetKeyDown(KeyCode.Mouse0));

        doAfterInputKey = ResetGame;
        doAfterInputKeyNextPer = PlayGameBuffer;

        StartCoroutine(WaitUntilGetKey(KeyCode.Mouse0, doAfterInputKey, doAfterInputKeyNextPer));
        
    /*    yield return WaitForFixedUpdate;

        doAfterInputKey = PlayGame;
        StartCoroutine(WaitUntilGetKey(KeyCode.Mouse0, doAfterInputKey));*/

    }

    IEnumerator WaitUntilGetKey(KeyCode keyCode, Action doAfterGetKeyDown,Action doAfterInputKeyNextPer)
    {
        while (!Input.GetKeyDown(keyCode))
        {
           // Debug.Log("jjjjj");
            yield return null;

        }
        doAfterGetKeyDown();
        yield return WaitForFixedUpdate;
        if (doAfterInputKeyNextPer != null) 
        {
            doAfterInputKeyNextPer();

        }
       
    }

    private void StartGame()
    {
        Time.timeScale = 1;
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
        PlayerBehaviour.Instance.SwitchControlPlayer(true);

    }

    public void ResetGame()
    {
        Time.timeScale = 1;
        GameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        CurrentGameState = GameState.GameOver;
        GameOverPanel.SetActive(true);
        StopAllCoroutines();
        ObstacleController.Instance.ClearAllObstacle();
        ObstacleController.Instance.LoadNextObstacle();
        ObstacleController.Instance.StartGame();
        BackgroundController.Instance.ResetBackGroundPos();
        Time.timeScale = 0;
        PlayerBehaviour.Instance.SwitchControlPlayer(false);
        PlayerBehaviour.Instance.ResetPlayer();

        // StartCoroutine(RestartGame());
        RestartGame();
    }


    public void NextObstacle()
    {
        PlayerBehaviour.Instance.ScrollPlayer(-7.7f, 1.2f);
        BackgroundController.Instance.ScrollBackGround(-8, 1.2f);
        ObstacleController.Instance.UnLoadCurrentObstacle();
        ObstacleController.Instance.UpdateCurrentObstacle();
        ObstacleController.Instance.ScrollObject(0, -8, 1.2f);
        ObstacleController.Instance.LoadNextObstacle();

    }

}
