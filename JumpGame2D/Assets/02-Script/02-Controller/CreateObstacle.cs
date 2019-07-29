using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObstacle : MonoBehaviour
{
   /* public enum ObstacleState
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

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        CreatObjectPool();
    }

    public GameObject CreateOneObstacle(int size,int sectorAmount,float minSpeed,float maxSpeed)
    {
        GameObject ObstacleParent = GetObject(ObstaclePrefab[3].ID);

        int[] rotationAngle;
        rotationAngle = new int[] { 0, 90, 180, 270 };
        for (int i = 0; i <= Random.Range(0, sectorAmount); i++) 
        {
            GameObject obstacle = GetObject(ObstaclePrefab[size].ID);
            int angle;
            obstacle.transform.parent = ObstacleParent.transform;
            obstacle.transform.position = Vector3.zero;


            angle = Random.Range(0, 4);

            while (rotationAngle[angle] == -1)
            {
                angle = Random.Range(0, 4);
              //  Debug.Log(rotationAngle[angle]);

            }
            obstacle.transform.localEulerAngles = new Vector3(0, 0, rotationAngle[angle]);
            rotationAngle[angle] = -1;

        }
        
        ObstacleParent.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        //ObstacleParent.AddComponent<ObstacleBehaviour>();
        ObstacleParent.GetComponent<ObstacleBehaviour>().RotateSpeed = Random.Range(minSpeed, maxSpeed);
     
        return ObstacleParent;
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
            for (int i=0;i< ObstaclePrefab[j].Amount; i++)
            {
                GameObject gameObject = Instantiate(ObstaclePrefab[j].ObstaclePrefab);
                obstacleCollection[objectId] = gameObject;
                objectId += 1;
                gameObject.SetActive(false);

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
        // Debug.Log(obstacle[0].transform.childCount);

        for (int i = 0; i < obstacle.Length; i++)
        {
            if (obstacle[i] == null)
            {
               // Debug.Log("hh");

                break;

            }
            else
            {
                int childCount = obstacle[i].transform.childCount;
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
    */
}
