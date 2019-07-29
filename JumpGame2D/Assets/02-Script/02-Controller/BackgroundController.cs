using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public static BackgroundController Instance;

    RectTransform[] backgroundImageTransform;

    int currentBackground;
    private Vector2 backgroundStartPos;

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
        backgroundStartPos = backgroundImageTransform[0].position;
        currentBackground = 0;


    }

    public void ScrollBackGround(float scrollDis, float scrollSpeed)
    {
        StartCoroutine(scrollBackdround(scrollDis, scrollSpeed));

    }

    IEnumerator scrollBackdround(float scrollDis, float scrollSpeed)
    {
        Vector3 backgroundCurrentPos = backgroundImageTransform[currentBackground].position;
        float dis = 0;

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

                if (backgroundImageTransform[currentBackground].position.y < -20)
                {
                    backgroundImageTransform[currentBackground].position = new Vector2(backgroundImageTransform[currentBackground].position.x, 35);
                    switch (currentBackground)
                    {
                        case 1:
                            currentBackground = 0;
                            break;

                        case 0:
                            currentBackground = 1;
                            break;
                    }
                    backgroundImageTransform[currentBackground].position = new Vector2(backgroundImageTransform[currentBackground].position.x, 21);
                    break;
                }



                yield return null;
            }


        }
    }

    public void ResetBackGroundPos()
    {
        backgroundImageTransform[1].position = new Vector2(backgroundImageTransform[currentBackground].position.x, 35);

        backgroundImageTransform[0].position = backgroundStartPos;
        currentBackground = 0;

    }


}
