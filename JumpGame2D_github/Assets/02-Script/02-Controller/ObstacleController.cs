using System.Collections;
using System.Collections.Generic;
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
        obstacleall

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

    Vector3 currentObstaclePos;
    Vector3 obstacleStartPos;

    int circleSizeProportionSun = 0;
    int randomRange;
    int currentProportionRange = 0;

    int sizeAppearRange;
    int sizeAppearRangeSum = 0;
    int currentAppearRange = 0;


    int[] rotationAngle;
    int angle;
    float dis = 0;

    int currentBackgroundColor;

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
        sizeAppearRangeSum = 0;
        currentAppearRange = 0;
        circleSizeProportionSun = 0;
        currentProportionRange = 0;
        angle = 0;
        obstacleStartPos = new Vector3(0, 1, 0);
    }

    private void InitObstacleSetting()
    {
        levelSetting.Circle_S.Size = ObstacleSize.Small;
        levelSetting.Circle_M.Size = ObstacleSize.Medium;
        levelSetting.Circle_L.Size = ObstacleSize.Large;
    }
    
    public void GetPrefab()
    {
      /*  for (int i = 0; i < ObstaclePrefab.Length; i++) 
        {
            ObstaclePrefab[i].ObstaclePrefab = (GameObject)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Prefab, ObstaclePrefab[i].Name.ToString(), typeof(GameObject));
        }*/


        CreatObjectPool();
        StartCoroutine(ObstacleParentGetObstacleBehaviour());
    }

    IEnumerator ObstacleParentGetObstacleBehaviour()
    {
        yield return null;
        for (int i = 0; i < ObstaclePrefab[3].Amount; i++)
        {
            obstacleBehaviours[i] = obstacleCollection[ObstaclePrefab[3].ID + i].GetComponent<ObstacleBehaviour>();
            obstacleBehaviours[i].enabled = false;

        }
    }

    public void GetStageData()
    {
        stageData = StageDataController.Instance.PlayerJson;

    }

    IEnumerator getStageData()
    {
        yield return null;

    }

    public void GetLevelData()
    {       
        levelSetting.circleAmountProportion[0].CircleProportion = stageData.Small_AppearProportion;
        levelSetting.circleAmountProportion[1].CircleProportion = stageData.Medium_AppearProportion;
        levelSetting.circleAmountProportion[2].CircleProportion = stageData.Big_AppearProportion;

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
        levelSetting.Circle_L.AppearProportion = stageData.Big_AppearProportion;



    }



    public void StopObstacleBehaviour()
    {
        for (int i = 0; i < obstacleBehaviours.Length; i++)
        {
            obstacleBehaviours[i].enabled = false;

        }

    }

    public void StartObstacleBehiour()
    {
        for (int i = 0; i < obstacleBehaviours.Length; i++)
        {
            obstacleBehaviours[i].enabled = true;

        }

    }

    public void ScrollObject(int scrollObject,float scrollDis,float speed)
    {
        switch (scrollObject)
        {
            case 0:
                ObstacleScrollDown(ref currentObstacleParent, scrollDis, speed);
                break;
            case 1:
                ObstacleScrollDown(ref nextObstacleParent, scrollDis, speed);
                break;
            default:
                throw new System.Exception("Scroll current obstacle input 0 , Scroll next obstacle intpu 1");
        }


    }

    public void StartGame()
    {
        currentBackgroundColor = GameManager.Instance.BackgroundColor;
        SetObstacle(ref currentObstacle, obstacleStartPos, currentBackgroundColor, ref currentObstacleParent);

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
            //Debug.Log(currentBackgroundColor);

            SetObstacle(ref nextObstacle, nextObstaclePosition, currentBackgroundColor + 1, ref nextObstacleParent);
        }

    }

    public void UnLoadCurrentObstacle()
    {
        RecoverObstacle(currentObstacle);
        currentObstacleParent = null;
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
            nextObstacleParent = null;

            nextObstacle = new GameObject[3];
            // StartCoroutine(next());
           /* for (int i = 0; i < nextObstacle.Length; i++)
            {
                nextObstacle[i] = null;
                Debug.Log("nextObstacle = null");
            }*/
        }


    }

    public void ClearAllObstacle()
    {
        RecoverObstacle(currentObstacle);

        RecoverObstacle(nextObstacle);
     //   Debug.Log("7");

        currentObstacleParent = null;
        nextObstacleParent = null;
        for (int i = 0; i < currentObstacle.Length; i++)
        {
            currentObstacle[i] = null;
            nextObstacle[i]=null;
        }
    }

    private void SetObstacle(ref GameObject[] Obstacle,Vector3 resetPosition,int backgroundColor,ref GameObject gameObjectParent)
    {
        CreatObstacle(ref Obstacle, backgroundColor);

        obstacleAll = GetObject(ObstaclePrefab[4].ID);
        
        for (int i = 0; i < Obstacle.Length; i++)
        {
            if (Obstacle[i] != null)
            {
                Obstacle[i].transform.parent = obstacleAll.transform;
                gameObjectParent = obstacleAll;
                obstacleAll.transform.position = resetPosition;

                Obstacle[i].transform.localPosition = new Vector3(0,0,0);
                for (int j = 0; j < Obstacle[i].transform.childCount; j++)
                {
                    Obstacle[i].transform.GetChild(j).transform.localPosition = new Vector3(0, 0, 0);
                }

            }
            

        }                     


    }

    private void CreatObstacle(ref GameObject[] Obstacle,int backgroundColor)
    {
        circleSizeProportionSun = 0;
        
        currentProportionRange = 0;

        for (int i = 0; i < levelSetting.circleAmountProportion.Length; i++)
        {
            circleSizeProportionSun += levelSetting.circleAmountProportion[i].CircleProportion;
        }

        randomRange = Random.Range(1, circleSizeProportionSun);

        for (int currentCircle = 0; currentCircle < levelSetting.circleAmountProportion.Length; currentCircle++)
        {
   //         Debug.Log("圓環個數機率 min : " + currentProportionRange + " Max : " + (levelSetting.circleAmountProportion[currentCircle].CircleProportion + currentProportionRange) + " 現在機率 : " + randomRange);

            if (randomRange > currentProportionRange && randomRange <= (levelSetting.circleAmountProportion[currentCircle].CircleProportion + currentProportionRange)) 
            {
                for (int y = 0; y < appeared.Length; y++)
                {
                    switch (y)
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

                //appeared = new PartOfObstacle[] { levelSetting.Circle_S, levelSetting.Circle_M, levelSetting.Circle_L };

            
                for (int e = 0; e < appeared.Length; e++)
                {

                    if (appeared[e].AppearProportion == 0)
                    {
                        appeared[e].Size = ObstacleSize.Null;

                    }

                }
                    
                for (int q = 0; q < (currentCircle + 1); q++) 
                {
                   
                    Obstacle[q] = CreateCircle(ref appeared, backgroundColor);//存入當前的障礙物
               //     Debug.Log(Obstacle[q].name);
                }

                break;
            }

            currentProportionRange += levelSetting.circleAmountProportion[currentCircle].CircleProportion;
        }     

    }
    
    private GameObject CreateCircle(ref PartOfObstacle[] appearedCircleList,int backgroundColor)
    {
        sizeAppearRangeSum = 0;
        currentAppearRange = 0;

        for (int i = 0; i < appearedCircleList.Length; i++)
        {
            if (appearedCircleList[i].Size != ObstacleSize.Null) 
            {
                sizeAppearRangeSum += appearedCircleList[i].AppearProportion;

            }

        }

        sizeAppearRange = Random.Range(1, sizeAppearRangeSum);

        for (int j = 0; j < appearedCircleList.Length; j++)//判斷該製造哪種尺寸的圓
        {

            if (appearedCircleList[j].Size != ObstacleSize.Null)
            {
           //     Debug.Log("機率 Min : " + currentAppearRange + "  MAX : "+ (currentAppearRange + appearedCircleList[j].AppearProportion) + "  現在機率" + sizeAppearRange);

                if (sizeAppearRange > currentAppearRange && sizeAppearRange <= (currentAppearRange + appearedCircleList[j].AppearProportion))
                {
                    currentCircle = CreateOneObstacle(backgroundColor, (int)appearedCircleList[j].Size, appearedCircleList[j].MaxSector, appearedCircleList[j].MinSpeed, appearedCircleList[j].MaxSpeed);
                    appearedCircleList[j].Size = ObstacleSize.Null;

                    //break;
                }
                currentAppearRange += appearedCircleList[j].AppearProportion;

            }

        }

        return currentCircle;
        
    }

    public void ObstacleScrollDown(ref GameObject scrollObject, float scrollDis, float speed)
    {
        dis = 0;
        StartCoroutine(obstacleScrollDown(dis, scrollDis, speed, scrollObject));
    }

    IEnumerator obstacleScrollDown(float dis,float scrollDis,float speed,GameObject moveGameObject)
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
                moveGameObject.transform.position = Vector3.Lerp(moveGameObject.transform.position, currentObstaclePos + new Vector3(0, scrollDis, 0), dis);
                yield return null;
            }
           

        }
    }

    public GameObject CreateOneObstacle(int color,int size, int sectorAmount, float minSpeed, float maxSpeed)
    {
        obstacleParent = GetObject(ObstaclePrefab[3].ID);

        rotationAngle = new int[] { 0, 90, 180, 270 };
        for (int i = 0; i <= Random.Range(0, sectorAmount); i++)
        {
            oneObstacle = GetObject(ObstaclePrefab[size].ID);
            oneObstacle.transform.parent = obstacleParent.transform;
            oneObstacle.transform.position = Vector3.zero;
            
            angle = Random.Range(0, 4);

            while (rotationAngle[angle] == -1)
            {
                angle = Random.Range(0, 4);
            }
            oneObstacle.transform.localEulerAngles = new Vector3(0, 0, rotationAngle[angle]);



            oneObstacle.GetComponent<SpriteController>().GetSpriteRendererAsset("ObjectAtlas", "Obstacle_" + color + size + angle);
            
            rotationAngle[angle] = -1;

        }

       // obstacleParent.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        obstacleParent.GetComponent<ObstacleBehaviour>().RotateSpeed = Random.Range(minSpeed, maxSpeed);
    //    obstacleParent.GetComponent<ObstacleBehaviour>().SetValue(minSpeed, maxSpeed);

        return obstacleParent;
    }


    private void CreatObjectPool()
    {
        int total = 0;
        int objectId = 0;

        for (int q = 0; q < ObstaclePrefab.Length; q++)
        {
            total += ObstaclePrefab[q].Amount;
        }

        obstacleCollection = new GameObject[total];

        for (int j = 0; j < ObstaclePrefab.Length; j++)
        {
            ObstaclePrefab[j].ID = objectId;
            for (int i = 0; i < ObstaclePrefab[j].Amount; i++)
            {
                
                obstacleCollection[objectId] = Instantiate(ObstaclePrefab[j].ObstaclePrefab);
                obstacleCollection[objectId].SetActive(false);
                objectId += 1;
                

            }

        }

    }

    public GameObject GetObject(int id)
    {
        for (int j = 0; j < ObstaclePrefab.Length; j++)
        {
            if (ObstaclePrefab[j].ID == id)
            {
                for (int i = 0; i < ObstaclePrefab[j].Amount; i++)
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
        for (int i = 0; i < obstacle.Length; i++)
        {
            if (obstacle[i] == null)
            {
                break;
            }
            else
            {
                int childCount = obstacle[i].transform.childCount;
                obstacle[i].transform.parent.gameObject.SetActive(false);
             //   Debug.Log(obstacle[i].transform.name);

                obstacle[i].transform.parent = null;
                for (int j = 0; j < childCount; j++)
                {
                    obstacle[i].transform.GetChild(0).gameObject.SetActive(false);
                    obstacle[i].transform.GetChild(0).gameObject.transform.parent.gameObject.SetActive(false);
                    obstacle[i].transform.GetChild(0).gameObject.transform.parent = null;
                }


            }

        }

    }


}
