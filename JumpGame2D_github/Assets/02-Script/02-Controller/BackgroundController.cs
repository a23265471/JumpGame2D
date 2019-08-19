using System.Collections;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController Instance;

    RectTransform[] backgroundImageTransform;

    int currentBackground;
    private Vector2 backgroundStartPos;
    private Vector2 baclgroundNextPos;

    Vector3 backgroundCurrentPos;
    float dis;
    public int BackgroundStageColor; 


    private void Awake()
    {
        Init();
    }
    
    public void Init()
    {
        Instance = this;
        backgroundImageTransform = new RectTransform[2];

        for (int i = 0; i < transform.childCount; i++)
        {
            backgroundImageTransform[i] = transform.GetChild(i).gameObject.GetComponent<RectTransform>();
        }
        backgroundStartPos = backgroundImageTransform[1].position;
        baclgroundNextPos = backgroundImageTransform[0].position;
        currentBackground = 1;
        BackgroundStageColor = 0;

    }

    public void ScrollBackGround(float scrollDis, float scrollSpeed)
    {
        StartCoroutine(scrollBackdround(scrollDis, scrollSpeed));

    }

    IEnumerator scrollBackdround(float scrollDis, float scrollSpeed)
    {
        backgroundCurrentPos = backgroundImageTransform[currentBackground].position;
        dis = 0;

        while (dis < 1)
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.GameOver)
            {
                break;
            }
            else
            {                
                dis += scrollSpeed * Time.deltaTime;
                dis = Mathf.Clamp(dis, 0, 1);
                backgroundImageTransform[currentBackground].position = Vector3.Lerp(backgroundImageTransform[currentBackground].position, backgroundCurrentPos + new Vector3(0, scrollDis, 0), dis);

                if (backgroundImageTransform[currentBackground].position.y < -50)
                {
                    backgroundImageTransform[currentBackground].position = new Vector2(backgroundImageTransform[currentBackground].position.x, 50);
                    switch (currentBackground)
                    {
                        case 1:
                            currentBackground = 0;
                            break;

                        case 0:
                            currentBackground = 1;
                            break;
                    }
                    backgroundImageTransform[currentBackground].position = backgroundStartPos;
                    break;
                }



                yield return null;
            }


        }
    }

    public void ResetBackGroundPos()
    {
        backgroundImageTransform[0].position = new Vector2(backgroundImageTransform[currentBackground].position.x, 35);

        backgroundImageTransform[1].position = backgroundStartPos;
        currentBackground = 1;
        BackgroundStageColor = 0;
    }


}
