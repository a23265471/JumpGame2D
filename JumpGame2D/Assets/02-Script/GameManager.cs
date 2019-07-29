using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private ObstacleController obstacleController;

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
    private int Level;
    
    private void OnEnable()
    {
        Init();

    }

    private void Init()
    {
        //  GameFacade.GetInstance().stageDataController.
        Instance = this;
        obstacleController = ObstacleController.Instance;
        InstantiateObject();
        PlayerBehaviour.Instance.ResetPlayer();

    }

    private void Start()
    {
        StartGame();

    }
    
    public void StartGame()
    {
        Time.timeScale = 0;
        CurrentGameState = GameState.Start;
        obstacleController.LoadNextObstacle();
        obstacleController.StartGame();
        PlayerBehaviour.Instance.SwitchControlPlayer(false);
        StartCoroutine(ReadyGameCroutine());
    }

    IEnumerator ReadyGameCroutine()
    {
        yield return new WaitUntil(() => Input.anyKey);
        StartGamePanel.SetActive(false);
        Time.timeScale = 1;

        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

        PlayGame();
    }

    public void PlayGame()
    {
        CurrentGameState = GameState.Play;
        //    obstacleController.CreatObstacle();
        PlayerBehaviour.Instance.SwitchControlPlayer(true);
    }
  
    public void GameOver()
    {
        CurrentGameState = GameState.GameOver;
        GameOverPanel.SetActive(true);
        StopAllCoroutines();
        obstacleController.ClearAllObstacle();
        obstacleController.LoadNextObstacle();
        obstacleController.StartGame();
        BackgroundController.Instance.ResetBackGroundPos();
        Time.timeScale = 0;
        PlayerBehaviour.Instance.SwitchControlPlayer(false);
        StartCoroutine(RestartGame());

    }

    IEnumerator RestartGame()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

        Time.timeScale = 1;
        GameOverPanel.SetActive(false);
        PlayerBehaviour.Instance.ResetPlayer();
        //ResetPos();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

        PlayGame();

    }

    public void NextObstacle()
    {
        PlayerBehaviour.Instance.ScrollPlayer(-7.7f, 1.2f);
        BackgroundController.Instance.ScrollBackGround(-8, 1.2f);
        obstacleController.UnLoadCurrentObstacle();
        obstacleController.UpdateCurrentObstacle();
        obstacleController.ScrollObject(0,-8, 1.2f);
        obstacleController.LoadNextObstacle();

    }

    public void InstantiateObject()
    {
        Instantiate(Player);
        currentCanvas = Instantiate(OptionCanvas);
        
        //   player.transform.position = PlayerStartPos;
        // Player.transform.localPosition = new Vector3(0, 0, 0);
        StartCoroutine(GetPanel());
    }

    IEnumerator GetPanel()
    {
        yield return null;
        GameOverPanel = currentCanvas.transform.GetChild(0).gameObject;
        StartGamePanel = currentCanvas.transform.GetChild(1).gameObject;
     //   Debug.Log(StartGamePanel.name);
    }
}
