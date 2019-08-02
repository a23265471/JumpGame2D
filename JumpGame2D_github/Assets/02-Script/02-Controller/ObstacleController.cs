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

    int circleSizeProportionSun = 0;
    int randomRange;
    int currentProportionRange = 0;

    int sizeAppearRange;
    int sizeAppearRangeSum = 0;
    int currentAppearRange = 0;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Instance = this;

     //   createObstacle = gameObject.GetComponent<CreateObstacle>();
        InitObstacleSetting();
        currentObstacle = new GameObject[3];
        nextObstacle = new GameObject[3];
        sizeAppearRangeSum = 0;
        currentAppearRange = 0;
        circleSizeProportionSun = 0;
        currentProportionRange = 0;
       // 
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

        ResetObstacle(ref currentObstacle,Vector3.zero,ref currentObstacleParent);

    }

    public void LoadNextObstacle()
    {
        if (nextObstacleParent != null)
        {
            throw new System.Exception("尚未卸載障礙物");
        }
        else
        {
            ResetObstacle(ref nextObstacle, nextObstaclePosition, ref nextObstacleParent);
          //  nextObstacleParent.transform.position = Vector3.zero;

        }

    }

    public void UnLoadCurrentObstacle()
    {
        RecoverObstacle(currentObstacle);
        currentObstacleParent = null; 
     // currentObstacle = null;
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
            //     Debug.Log(currentObstacleParent.transform.GetChild(0).gameObject.transform.GetChild(0).name);
            nextObstacleParent = null;
          
            nextObstacle = new GameObject[3];
        }


    }

    public void ClearAllObstacle()
    {
        RecoverObstacle(currentObstacle);
        RecoverObstacle(nextObstacle);

        currentObstacleParent = null;
        nextObstacleParent = null;
       // currentObstacle = new GameObject[3];

        for (int i = 0; i < currentObstacle.Length; i++)
        {
            currentObstacle[i] = null;
            nextObstacle[i]=null;
        }

       // nextObstacle = new GameObject[3];
    }

    private void ResetObstacle(ref GameObject[] Obstacle,Vector3 resetPosition,ref GameObject gameObjectParent)
    {
        CreatObstacle(ref Obstacle);

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

    private void CreatObstacle(ref GameObject[] Obstacle)
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
                PartOfObstacle[] appeared = new PartOfObstacle[] { levelSetting.Circle_S, levelSetting.Circle_M, levelSetting.Circle_L };

            /*    Debug.Log(levelSetting.Circle_S.AppearProportion);
                Debug.Log(levelSetting.Circle_M.AppearProportion);*/



                for (int e = 0; e < appeared.Length; e++)
                {

                    if (appeared[e].AppearProportion == 0)
                    {
                        appeared[e].Size = ObstacleSize.Null;

                    }

                }
                    
                for (int q = 0; q < (currentCircle + 1); q++) 
                {
                   
                    Obstacle[q] = CreateCircle(ref appeared);//存入當前的障礙物
               //     Debug.Log(Obstacle[q].name);
                }

                break;
            }

            currentProportionRange += levelSetting.circleAmountProportion[currentCircle].CircleProportion;
        }     

    }
    
    private GameObject CreateCircle(ref PartOfObstacle[] appearedCircleList/*,ref int currentRandomLength*/)
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
                    currentCircle = CreateOneObstacle((int)appearedCircleList[j].Size, appearedCircleList[j].MaxSector, appearedCircleList[j].MinSpeed, appearedCircleList[j].MaxSpeed);
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
        float dis = 0;
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

    public GameObject CreateOneObstacle(int size, int sectorAmount, float minSpeed, float maxSpeed)
    {
        obstacleParent = GetObject(ObstaclePrefab[3].ID);

        int[] rotationAngle;
        rotationAngle = new int[] { 0, 90, 180, 270 };
        for (int i = 0; i <= Random.Range(0, sectorAmount); i++)
        {
            oneObstacle = GetObject(ObstaclePrefab[size].ID);
            int angle;
            oneObstacle.transform.parent = obstacleParent.transform;
            oneObstacle.transform.position = Vector3.zero;


            angle = Random.Range(0, 4);

            while (rotationAngle[angle] == -1)
            {
                angle = Random.Range(0, 4);
            }
            oneObstacle.transform.localEulerAngles = new Vector3(0, 0, rotationAngle[angle]);
            rotationAngle[angle] = -1;

        }

        obstacleParent.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        obstacleParent.GetComponent<ObstacleBehaviour>().RotateSpeed = Random.Range(minSpeed, maxSpeed);

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
            //    Debug.Log(obstacle[i].transform.GetChild(0).name);
                obstacle[i].transform.parent.gameObject.SetActive(false);
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
