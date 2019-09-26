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
    
    public void GetData(
       /* float Gravity, float JumpForce, float Weight, int Time,
        int One_CircleProportion, int Two_CircleProportion, int Three_CircleProportion,
        float Small_MinSpeed, float Small_MaxSpeed, int Small_MaxSector,int Small_AppearProportion,
        float Medium_MinSpeed, float Medium_MaxSpeed, int Medium_MaxSector, int Medium_AppearProportion,
        float Big_MinSpeed, float Big_MaxSpeed, int Big_MaxSector, int Big_AppearProportion*/string dataJson)
    {

         PlayerJson = new PlayerJsonData();
         PlayerJson = JsonUtility.FromJson<PlayerJsonData>(dataJson);
        
    }

    public void SetData()
    {
        PlayerBehaviour.Instance.LoadData();        
        GameManager.Instance.PlayTime = PlayerJson.Time;
        OneSmallSectorScore = PlayerJson.OneSmallSectorScore;
        OneMediumSectorScore = PlayerJson.OneMediumSectorScore;
        OneBigSectorScore = PlayerJson.OneBigSectorScore;
        CircleCountMagnificationScore = PlayerJson.CircleCountMagnificationScore;
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
