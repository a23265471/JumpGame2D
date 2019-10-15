using System.Collections;
using UnityEngine;

public class StageDataController : MonoBehaviour
{
    //public PlayerStageData playerStageData;

    public static StageDataController Instance;
    public PlayerJsonData PlayerJson;

    public int TotalScore;
  //  public int CurrentScore;
    public int OneSmallSectorScore;
    public int OneMediumSectorScore;
    public int OneBigSectorScore;
    public float CircleCountMagnificationScore;
    private int score;


    private void Awake()
    {
        Instance = this;
      
    }
    
    public void GetData(string dataJson)
    {

         PlayerJson = new PlayerJsonData();
         PlayerJson = JsonUtility.FromJson<PlayerJsonData>(dataJson);
    
    }

    public void SetData()//*******************************************要改回來
    {
          PlayerBehaviour.Instance.LoadData();        
          GameManager.Instance.PlayTime = PlayerJson.Time;
          OneSmallSectorScore = PlayerJson.OneSmallSectorScore;
          OneMediumSectorScore = PlayerJson.OneMediumSectorScore;
          OneBigSectorScore = PlayerJson.OneBigSectorScore;
          CircleCountMagnificationScore = PlayerJson.CircleCountMagnificationScore;
       /* PlayerBehaviour.Instance.LoadData();
        GameManager.Instance.PlayTime = 5;
        OneSmallSectorScore = 5;
        OneMediumSectorScore = 3;
        OneBigSectorScore = 2;
        CircleCountMagnificationScore = 10;*/
    }

    
    
    public int Score(int circleCount,int smallSectorCount, int mediumSectorCount, int bigSectorCount)
    {
        score = (int)Mathf.Round((smallSectorCount * OneSmallSectorScore + mediumSectorCount * OneMediumSectorScore + bigSectorCount * OneBigSectorScore) * circleCount * CircleCountMagnificationScore);

        return score;
    }

    public int GetCurrentObstacleScore()
    {
        TotalScore += ObstacleController.Instance.CurrentTotalScore;
        return ObstacleController.Instance.CurrentTotalScore;
    }

    public void ResetTotalScore()
    {
        TotalScore = 0;
    }

}
