using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public static ObstacleController Instance;

    public enum ObstacleSize
    {
        Small=0,
        Medium=1,
        Large=2,
        Null=-1

    }

    //private CreateObstacle createObstacle;
    private PlayerJsonData stageData;

    private GameObject[] currentObstacle;
    private GameObject[] nextObstacle;

    private GameObject currentObstacleParent;
    private GameObject nextObstacleParent;

    private GameObject currentScore;
    private GameObject nextScore;

    private ScoreEffect[] scoreEffect;


    [SerializeField]
    private Vector3 nextObstaclePosition;

    [SerializeField]
    public LevelSetting levelSetting;


    [System.Serializable]
    public struct LevelSetting
    {
        [Header("關卡機率設定")]
        public int Level;
         
        [Header("圓環數比例")]
        public CircleAmountProportion[] circleAmountProportion;
        

        [Header("障礙物數值設定")]
        public PartOfObstacle Circle_S;
        public PartOfObstacle Circle_M;
        public PartOfObstacle Circle_L;
    }

    [System.Serializable]
    public struct CircleAmountProportion
    {
        public string Name;
        [Range(0, 10)]
        public int CircleProportion;
        
    }

    [System.Serializable]
    public struct PartOfObstacle
    {
        //[Header("圓形的尺寸 S:0 M:1 L:2")]
        public ObstacleSize Size;
        [Header("速度範圍")]
        public float MinSpeed;
        public float MaxSpeed;
        [Header("最大扇形數")]
        public int MaxSector;
        [Header("出現比例")]
        [Range(0,10)]
        public int AppearProportion;


    }
    
    public enum ObstacleState
    {
        obstacle_s,
        obstacle_m,
        obstacle_l,
        parent,
        obstacleall,
        ScoreEffect

    }

    public ObstaclePrefabInfo[] ObstaclePrefab;
    private GameObject[] obstacleCollection;

    [System.Serializable]
    public struct ObstaclePrefabInfo
    {
        [Header("Obstacle Info")]
        public ObstacleState Name;
        public int ID;
        public int Amount;
        public GameObject ObstaclePrefab;

    }

    PartOfObstacle nullPartOfObstacle;

    private GameManager gameManager;

    GameObject obstacleAll;
    GameObject currentCircle = null;
    GameObject obstacleParent;
    GameObject oneObstacle;

    Vector2 currentObstaclePos;
    Vector2 obstacleStartPos;
    Vector2 obstacleScrollDis;

    int circleSizeProportionSum = 0;
    int randomRange;
    int currentProportionRange = 0;

    int sizeAppearRange;
    int sizeAppearRangeSum = 0;
    int currentAppearRange = 0;
    int secterCount;


    int[] rotationAngle;
    int angle;
    float dis = 0;

    int currentBackgroundColor;

    #region for迴圈專用變數
    int i;
    int j;
    int k;
    int q;
    //以上製造障礙物專用變數
    int e;
    int r;
    #endregion

    #region 計分專用
    public int CircleCount;
    public int BigSecterCount;
    public int SmallSecterCount;
    public int MediumSecterCount;

    private int NextTotalScore;
    public int CurrentTotalScore;
    #endregion

    ObstacleBehaviour[] obstacleBehaviours;    

    PartOfObstacle[] appeared;

    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Instance = this;
        InitObstacleSetting();
        currentObstacle = new GameObject[3];
        nextObstacle = new GameObject[3];
        appeared = new PartOfObstacle[3];
        rotationAngle = new int[4];
        obstacleBehaviours = new ObstacleBehaviour[ObstaclePrefab[3].Amount];
        scoreEffect = new ScoreEffect[ObstaclePrefab[5].Amount];

        sizeAppearRangeSum = 0;
        currentAppearRange = 0;
        circleSizeProportionSum = 0;
        currentProportionRange = 0;
        angle = 0;
        obstacleStartPos = new Vector2(0, 1);
        obstacleScrollDis = new Vector2(0, 0);

    }

    private void InitObstacleSetting()
    {
        levelSetting.Circle_S.Size = ObstacleSize.Small;
        levelSetting.Circle_M.Size = ObstacleSize.Medium;
        levelSetting.Circle_L.Size = ObstacleSize.Large;
    }
    
    public void GetPrefab()
    {
        for (int i = 0; i < ObstaclePrefab.Length; i++) 
        {
            ObstaclePrefab[i].ObstaclePrefab = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, ObstaclePrefab[i].Name.ToString(), typeof(GameObject));
        }

        CreatObjectPool();
        StartCoroutine(GetComponent());
    }

    IEnumerator GetComponent()
    {
        yield return null;

        for (i = 0; i < ObstaclePrefab[3].Amount; i++)
        {
            obstacleBehaviours[i] = obstacleCollection[ObstaclePrefab[3].ID + i].GetComponent<ObstacleBehaviour>();
            obstacleBehaviours[i].enabled = false;

        }

        for (j = 0; j < ObstaclePrefab[5].Amount; j++) 
        {
            scoreEffect[j] = obstacleCollection[ObstaclePrefab[5].ID + j].GetComponent<ScoreEffect>();
        }
   
    }

    public void LoadLevelData(int stage)
    {
        stageData = StageDataController.Instance.PlayerJson;

        GetStageData(stage);
    }
    
    public void GetStageData(int stage)
    {
        switch (stage)
        {
            case 1:
                  /* levelSetting.circleAmountProportion[0].CircleProportion = stageData.Stage1_One_CircleProportion;
                   levelSetting.circleAmountProportion[1].CircleProportion = stageData.Stage1_Two_CircleProportion;
                   levelSetting.circleAmountProportion[2].CircleProportion = stageData.Stage1_Three_CircleProportion;

                   levelSetting.Circle_S.MinSpeed = stageData.Stage1_Small_MinSpeed;
                   levelSetting.Circle_S.MaxSpeed = stageData.Stage1_Small_MaxSpeed;
                   levelSetting.Circle_S.MaxSector = stageData.Stage1_Small_MaxSector;
                   levelSetting.Circle_S.AppearProportion = stageData.Stage1_Small_AppearProportion;

                   levelSetting.Circle_M.MinSpeed = stageData.Stage1_Medium_MinSpeed;
                   levelSetting.Circle_M.MaxSpeed = stageData.Stage1_Medium_MaxSpeed;
                   levelSetting.Circle_M.MaxSector = stageData.Stage1_Medium_MaxSector;
                   levelSetting.Circle_M.AppearProportion = stageData.Stage1_Medium_AppearProportion;

                   levelSetting.Circle_L.MinSpeed = stageData.Stage1_Big_MinSpeed;
                   levelSetting.Circle_L.MaxSpeed = stageData.Stage1_Big_MaxSpeed;
                   levelSetting.Circle_L.MaxSector = stageData.Stage1_Big_MaxSector;
                   levelSetting.Circle_L.AppearProportion = stageData.Stage1_Big_AppearProportion;*/

                levelSetting.circleAmountProportion[0].CircleProportion = 1;
                levelSetting.circleAmountProportion[1].CircleProportion = 0;
                levelSetting.circleAmountProportion[2].CircleProportion = 0;

                levelSetting.Circle_S.MinSpeed = 50;
                levelSetting.Circle_S.MaxSpeed =80;
                levelSetting.Circle_S.MaxSector = 2;
                levelSetting.Circle_S.AppearProportion = 0;

                levelSetting.Circle_M.MinSpeed = 150;
                levelSetting.Circle_M.MaxSpeed = 180;
                levelSetting.Circle_M.MaxSector = 2;
                levelSetting.Circle_M.AppearProportion = 1;

                levelSetting.Circle_L.MinSpeed = 200;
                levelSetting.Circle_L.MaxSpeed = 230;
                levelSetting.Circle_L.MaxSector = 2;
                levelSetting.Circle_L.AppearProportion = 1;

                break;

            case 2:
               /*  levelSetting.circleAmountProportion[0].CircleProportion = stageData.Stage2_One_CircleProportion;
                 levelSetting.circleAmountProportion[1].CircleProportion = stageData.Stage2_Two_CircleProportion;
                 levelSetting.circleAmountProportion[2].CircleProportion = stageData.Stage2_Three_CircleProportion;

                 levelSetting.Circle_S.MinSpeed = stageData.Stage2_Small_MinSpeed;
                 levelSetting.Circle_S.MaxSpeed = stageData.Stage2_Small_MaxSpeed;
                 levelSetting.Circle_S.MaxSector = stageData.Stage2_Small_MaxSector;
                 levelSetting.Circle_S.AppearProportion = stageData.Stage2_Small_AppearProportion;

                 levelSetting.Circle_M.MinSpeed = stageData.Stage2_Medium_MinSpeed;
                 levelSetting.Circle_M.MaxSpeed = stageData.Stage2_Medium_MaxSpeed;
                 levelSetting.Circle_M.MaxSector = stageData.Stage2_Medium_MaxSector;
                 levelSetting.Circle_M.AppearProportion = stageData.Stage2_Medium_AppearProportion;

                 levelSetting.Circle_L.MinSpeed = stageData.Stage2_Big_MinSpeed;
                 levelSetting.Circle_L.MaxSpeed = stageData.Stage2_Big_MaxSpeed;
                 levelSetting.Circle_L.MaxSector = stageData.Stage2_Big_MaxSector;
                 levelSetting.Circle_L.AppearProportion = stageData.Stage2_Big_AppearProportion;*/

                levelSetting.circleAmountProportion[0].CircleProportion = 1;
                levelSetting.circleAmountProportion[1].CircleProportion = 0;
                levelSetting.circleAmountProportion[2].CircleProportion = 0;

                levelSetting.Circle_S.MinSpeed = 50;
                levelSetting.Circle_S.MaxSpeed = 80;
                levelSetting.Circle_S.MaxSector = 2;
                levelSetting.Circle_S.AppearProportion = 0;

                levelSetting.Circle_M.MinSpeed = 150;
                levelSetting.Circle_M.MaxSpeed = 180;
                levelSetting.Circle_M.MaxSector = 2;
                levelSetting.Circle_M.AppearProportion = 1;

                levelSetting.Circle_L.MinSpeed = 200;
                levelSetting.Circle_L.MaxSpeed = 230;
                levelSetting.Circle_L.MaxSector = 2;
                levelSetting.Circle_L.AppearProportion = 1;

                break;

            case 3:
                /*    levelSetting.circleAmountProportion[0].CircleProportion = stageData.One_CircleProportion;
                    levelSetting.circleAmountProportion[1].CircleProportion = stageData.Two_CircleProportion;
                    levelSetting.circleAmountProportion[2].CircleProportion = stageData.Three_CircleProportion;

                    levelSetting.Circle_S.MinSpeed = stageData.Small_MinSpeed;
                    levelSetting.Circle_S.MaxSpeed = stageData.Small_MaxSpeed;
                    levelSetting.Circle_S.MaxSector = stageData.Small_MaxSector;
                    levelSetting.Circle_S.AppearProportion = stageData.Small_AppearProportion;

                    levelSetting.Circle_M.MinSpeed = stageData.Medium_MinSpeed;
                    levelSetting.Circle_M.MaxSpeed = stageData.Medium_MaxSpeed;
                    levelSetting.Circle_M.MaxSector = stageData.Medium_MaxSector;
                    levelSetting.Circle_M.AppearProportion = stageData.Medium_AppearProportion;

                    levelSetting.Circle_L.MinSpeed = stageData.Big_MinSpeed;
                    levelSetting.Circle_L.MaxSpeed = stageData.Big_MaxSpeed;
                    levelSetting.Circle_L.MaxSector = stageData.Big_MaxSector;
                    levelSetting.Circle_L.AppearProportion = stageData.Big_AppearProportion;*/

                levelSetting.circleAmountProportion[0].CircleProportion = 1;
                levelSetting.circleAmountProportion[1].CircleProportion = 0;
                levelSetting.circleAmountProportion[2].CircleProportion = 0;

                levelSetting.Circle_S.MinSpeed = 50;
                levelSetting.Circle_S.MaxSpeed = 80;
                levelSetting.Circle_S.MaxSector = 2;
                levelSetting.Circle_S.AppearProportion = 0;

                levelSetting.Circle_M.MinSpeed = 150;
                levelSetting.Circle_M.MaxSpeed = 180;
                levelSetting.Circle_M.MaxSector = 2;
                levelSetting.Circle_M.AppearProportion = 1;

                levelSetting.Circle_L.MinSpeed = 200;
                levelSetting.Circle_L.MaxSpeed = 230;
                levelSetting.Circle_L.MaxSector = 2;
                levelSetting.Circle_L.AppearProportion = 1;
                break;
        }
       

    }

    public void StopObstacleBehaviour()
    {

        for (e = 0; e < obstacleBehaviours.Length; e++)
        {
            obstacleBehaviours[e].enabled = false;

        }

    }

    public void StartObstacleBehiour()
    {
        for (e = 0; e < obstacleBehaviours.Length; e++)
        {
            obstacleBehaviours[e].enabled = true;

        }

    }

    public void ScrollObject(int scrollObject,float scrollDis,float speed)
    {
        switch (scrollObject)
        {
            case 0:
                ObstacleScrollDown(ref currentObstacleParent, scrollDis, speed);
                ObstacleScrollDown(ref currentScore, scrollDis, speed);

                break;
            case 1:
                ObstacleScrollDown(ref nextObstacleParent, scrollDis, speed);
                ObstacleScrollDown(ref nextScore, scrollDis, speed);

                break;
            default:
                throw new System.Exception("Scroll current obstacle input 0 , Scroll next obstacle intpu 1");
        }


    }

    public void StartGame()
    {
        currentBackgroundColor = GameManager.Instance.BackgroundColor;
        SetObstacle(ref currentObstacle, obstacleStartPos, currentBackgroundColor, ref currentObstacleParent,ref CurrentTotalScore);
        SetScorePoint(ref currentScore, obstacleStartPos, currentBackgroundColor);
    }

    public void UnLoadCurrentObstacle()
    {
        RecoverObstacle(currentObstacle);
        currentScore.SetActive(false);

        currentObstacleParent = null;
        currentScore = null;
    }

    public void UpdateCurrentObstacle()
    {
        if (currentObstacleParent != null)
        {
            throw new System.Exception("尚未卸載當前障礙物");
        }
        else
        {
            currentObstacleParent = nextObstacleParent;
            currentObstacle = nextObstacle;

            currentScore = nextScore;

            nextObstacleParent = null;
            nextObstacle = new GameObject[3];
            nextScore = null;

            CurrentTotalScore = NextTotalScore;//計分用
         
        }


    }

    public void ClearAllObstacle()
    {
        RecoverObstacle(currentObstacle);

        RecoverObstacle(nextObstacle);

        currentScore.SetActive(false);
        nextScore.SetActive(false);
     
        currentObstacleParent = null;
        nextObstacleParent = null;

        nextScore = null;
        currentScore = null;

        for (e = 0; e < currentObstacle.Length; e++)
        {
            currentObstacle[e] = null;
            nextObstacle[e]=null;
        }
    }


    public void LoadNextObstacle()
    {
        if (nextObstacleParent != null)
        {
            throw new System.Exception("尚未卸載障礙物");
        }
        else
        {
            currentBackgroundColor = GameManager.Instance.BackgroundColor;

            SetObstacle(ref nextObstacle, nextObstaclePosition, currentBackgroundColor + 1, ref nextObstacleParent,ref NextTotalScore);
            SetScorePoint(ref nextScore, nextObstaclePosition, currentBackgroundColor + 1);

            for (e = 0; e < scoreEffect.Length; e++)
            {
                scoreEffect[e].ResetState();
            }
        }
    }

    public int CountScore()
    {
        CircleCount = 0;
        if(SmallSecterCount!=0)
        {
            CircleCount += 1;
        }
        if (MediumSecterCount != 0)
        {
            CircleCount += 1;
        }
        if (BigSecterCount != 0)
        {
            CircleCount += 1;
        }
       // Debug.Log("圈數: " + CircleCount+" 小扇形數: " + SmallSecterCount + " 中扇形數: " + MediumSecterCount+ " 大扇形數: " + BigSecterCount+ " 分數: " + StageDataController.Instance.Score(CircleCount, SmallSecterCount, MediumSecterCount, BigSecterCount));       
        return StageDataController.Instance.Score(CircleCount, SmallSecterCount, MediumSecterCount, BigSecterCount);
    }

    private void SetObstacle(ref GameObject[] Obstacle,Vector3 resetPosition,int backgroundColor,ref GameObject gameObjectParent,ref int score)
    {
        CreatObstacle(ref Obstacle, backgroundColor,ref score);

        obstacleAll = GetObject(ObstaclePrefab[4].ID);
        
        for (e = 0; e < Obstacle.Length; e++)
        {
            if (Obstacle[e] != null)
            {

                Obstacle[e].transform.parent = obstacleAll.transform;
                gameObjectParent = obstacleAll;
                obstacleAll.transform.position = resetPosition;

                Obstacle[e].transform.localPosition = new Vector3(0,0,0);
                for (r = 0; r < Obstacle[e].transform.childCount; r++)
                {
                    Obstacle[e].transform.GetChild(r).transform.localPosition = Vector3.zero;
                }

            }
            

        }


    }

    private void SetScorePoint(ref GameObject scoreEffect, Vector2 position,int color)
    {
        scoreEffect = GetObject(ObstaclePrefab[5].ID);

        scoreEffect.transform.position = position;
        scoreEffect.GetComponent<ScoreEffect>().GetSprite(color);

    }

    private void CreatObstacle(ref GameObject[] Obstacle,int backgroundColor,ref int score)
    {
        circleSizeProportionSum = 0;       
        currentProportionRange = 0;

        for (i = 0; i < levelSetting.circleAmountProportion.Length; i++)
        {
            circleSizeProportionSum += levelSetting.circleAmountProportion[i].CircleProportion;
        }

        randomRange = Random.Range(1, circleSizeProportionSum);

        for (i = 0; i < levelSetting.circleAmountProportion.Length; i++)//決定製造的環數
        {
        //    Debug.Log("圓環個數機率 min : " + currentProportionRange + " Max : " + (levelSetting.circleAmountProportion[i].CircleProportion + currentProportionRange) + " 現在機率 : " + randomRange);

            if (randomRange > currentProportionRange && randomRange <= (levelSetting.circleAmountProportion[i].CircleProportion + currentProportionRange)) 
            {
                for (j = 0; j < appeared.Length; j++)//傳入關卡設定的障礙物資訊
                {
                    switch (j)
                    {
                        case 0:
                            appeared[0] = levelSetting.Circle_S;
                            break;
                        case 1:
                            appeared[1] = levelSetting.Circle_M;
                            break;
                        case 2:
                            appeared[2] = levelSetting.Circle_L;
                            break;

                    }
                }
            
                for (j = 0; j < appeared.Length; j++)
                {

                    if (appeared[j].AppearProportion == 0)//判斷那些尺寸可以出現(初始值的運算)
                    {
                        appeared[j].Size = ObstacleSize.Null;

                    }

                }

                SmallSecterCount = 0;//計分用
                MediumSecterCount = 0;//計分用
                BigSecterCount = 0;//計分用

                for (j = 0; j < (i + 1); j++) //
                {
                    Obstacle[j] = CreateCircle(ref appeared, backgroundColor);//製造一種尺寸的圓環,存入當前的障礙物
                }
                score = CountScore();//計分用
                break;
            }

            currentProportionRange += levelSetting.circleAmountProportion[i].CircleProportion;
        }     

    }
    
    private GameObject CreateCircle(ref PartOfObstacle[] appearedCircleList,int backgroundColor)
    {
        sizeAppearRangeSum = 0;
        currentAppearRange = 0;

        for (k = 0; k < appearedCircleList.Length; k++)
        {
            if (appearedCircleList[k].Size != ObstacleSize.Null) //判斷哪種尺寸可以出現(把出現過的篩掉)
            {
                sizeAppearRangeSum += appearedCircleList[k].AppearProportion;

            }

        }

        sizeAppearRange = Random.Range(1, sizeAppearRangeSum);//隨機算數

        for (k = 0; k < appearedCircleList.Length; k++)//判斷該製造哪些尺寸的圓
        {

            if (appearedCircleList[k].Size != ObstacleSize.Null)//有出現過的尺寸或出線機率為0的不能再出現
            {
           //     Debug.Log("機率 Min : " + currentAppearRange + "  MAX : "+ (currentAppearRange + appearedCircleList[j].AppearProportion) + "  現在機率" + sizeAppearRange);

                if (sizeAppearRange > currentAppearRange && sizeAppearRange <= (currentAppearRange + appearedCircleList[k].AppearProportion))
                {
                    //依造比例用隨機亂數去決定要製造哪種尺寸的圓環
                    currentCircle = CreateOneObstacle(backgroundColor, (int)appearedCircleList[k].Size, appearedCircleList[k].MaxSector, appearedCircleList[k].MinSpeed, appearedCircleList[k].MaxSpeed);
                    //  Debug.Log(appearedCircleList[k].Size);

                   
                    switch (appearedCircleList[k].Size)//計分用
                    {
                        case ObstacleSize.Small:
                            SmallSecterCount = currentCircle.transform.childCount;
                            break;

                        case ObstacleSize.Medium:
                            MediumSecterCount = currentCircle.transform.childCount;
                            break;

                        case ObstacleSize.Large:
                            BigSecterCount = currentCircle.transform.childCount;
                            break;
                    }
                    appearedCircleList[k].Size = ObstacleSize.Null;//製造過的尺寸要篩掉;
                }
                currentAppearRange += appearedCircleList[k].AppearProportion;

            }

        }
        return currentCircle;
    }

    public GameObject CreateOneObstacle(int color,int size, int sectorAmount, float minSpeed, float maxSpeed)
    {

        obstacleParent = GetObject(ObstaclePrefab[3].ID);

        for (q = 0; q < 4; q++)
        {
            rotationAngle[q] = 90 * q;

        }
        secterCount = Random.Range(0, sectorAmount);

        for (q = 0; q <= secterCount; q++)//隨機製造規定以下的扇數
        {
            oneObstacle = GetObject(ObstaclePrefab[size].ID);
            oneObstacle.transform.parent = obstacleParent.transform;
            oneObstacle.transform.position = Vector3.zero;
            
            angle = Random.Range(0, 4);

            while (rotationAngle[angle] == -1)//判斷那些角度的扇形已經存在,並重新亂數
            {
                angle = Random.Range(0, 4);
            }
            oneObstacle.transform.localEulerAngles = new Vector3(0, 0, rotationAngle[angle]);
            
            oneObstacle.GetComponent<SpriteController>().GetSprite("Obstacle_" + color + size + angle);
            
            rotationAngle[angle] = -1;

            
        }
        
        obstacleParent.GetComponent<ObstacleBehaviour>().RotateSpeed = Random.Range(minSpeed, maxSpeed);
        return obstacleParent;
    }


    private void CreatObjectPool()
    {
        int total = 0;
        int objectId = 0;

        for (i = 0; i < ObstaclePrefab.Length; i++)
        {
            total += ObstaclePrefab[i].Amount;
        }

        obstacleCollection = new GameObject[total];

        for (i = 0; i < ObstaclePrefab.Length; i++)
        {
            ObstaclePrefab[i].ID = objectId;
            for (j = 0; j < ObstaclePrefab[i].Amount; j++)
            {
                obstacleCollection[objectId] = Instantiate(ObstaclePrefab[i].ObstaclePrefab);
                objectId += 1;
            }

        }
        StartCoroutine(CloseObject());
    }

    IEnumerator CloseObject()
    {
        yield return null;
        for (e = 0; e < obstacleCollection.Length; e++)
        {
            obstacleCollection[e].SetActive(false);
        }
    }

    public GameObject GetObject(int id)
    {
        for (e = 0; e < ObstaclePrefab.Length; e++)
        {
            if (ObstaclePrefab[e].ID == id)
            {
                for (int r = 0; r < ObstaclePrefab[e].Amount; r++)
                {
                    if (!obstacleCollection[id].activeInHierarchy)
                    {
                        obstacleCollection[id].SetActive(true);
                        return obstacleCollection[id];
                    }
                    id += 1;
                }

            }

        }

        return null;
    }

    public void RecoverObstacle(GameObject[] obstacle)
    {
        for (e = 0; e < obstacle.Length; e++)
        {
            if (obstacle[e] == null)
            {
                break;
            }
            else
            {
                int childCount = obstacle[e].transform.childCount;
                obstacle[e].transform.parent.gameObject.SetActive(false);
                obstacle[e].transform.parent = null;
                for (int j = 0; j < childCount; j++)
                {
                    obstacle[e].transform.GetChild(0).gameObject.SetActive(false);
                    obstacle[e].transform.GetChild(0).gameObject.transform.parent.gameObject.SetActive(false);
                    obstacle[e].transform.GetChild(0).gameObject.transform.parent = null;
                }
                
            }

        }

    }
    
    public void ObstacleScrollDown(ref GameObject scrollObject, float scrollDis, float speed)
    {
        dis = 0;
        StartCoroutine(obstacleScrollDown(dis, scrollDis, speed, scrollObject));
    }

    IEnumerator obstacleScrollDown(float dis, float scrollDis, float speed, GameObject moveGameObject)
    {
        currentObstaclePos = moveGameObject.transform.position;
        while (dis < 1)
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.GameOver)
            {
                break;
            }
            else
            {
                dis += speed * Time.deltaTime;
                dis = Mathf.Clamp(dis, 0, 1);
                obstacleScrollDis.y = scrollDis;

                moveGameObject.transform.position = Vector3.Lerp(moveGameObject.transform.position, currentObstaclePos + obstacleScrollDis, dis);
                yield return null;
            }
        }
    }

}
