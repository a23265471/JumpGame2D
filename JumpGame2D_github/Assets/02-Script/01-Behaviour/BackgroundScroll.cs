using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    //  public static BackgroundScroll Instance;
    [SerializeField]
    private float StartPos_Y;
    private RectTransform rectTransform;
    public float position_Y;
    private Vector2 movePos;
    private float height;

    public float scrollDis;
    public float Speed;
    private float nextPosY;

 //   public int BackgroundStageColor;
    

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
     //   Instance = this;
      //  startPos.y = rectTransform.anchoredPosition.y;
        rectTransform = GetComponent<RectTransform>();
        movePos = Vector2.zero;
        scrollDis = rectTransform.sizeDelta.y/4;
     //   BackgroundStageColor = 0;
    }

    public void ScrollBackground()
    {
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        nextPosY = rectTransform.anchoredPosition.y - scrollDis;
        
        while (rectTransform.anchoredPosition.y > nextPosY)
        {
            position_Y = rectTransform.anchoredPosition.y;
            position_Y -= Speed * Time.deltaTime;
            if (position_Y < nextPosY)
            {
                position_Y = nextPosY;
            }
            movePos.y = position_Y;
            rectTransform.anchoredPosition = movePos;
            yield return null;

           // Debug.Log(rectTransform.anchoredPosition);

        }



        //  BackgroundStageColor += 1;

        if (rectTransform.anchoredPosition.y <= -rectTransform.sizeDelta.y)
        {
            movePos.y = rectTransform.sizeDelta.y / 4;
            rectTransform.anchoredPosition = movePos;
            switch (GameManager.Instance.CurrentBackground)
            {
                case 0:
                    GameManager.Instance.CurrentBackground = 1;
                    break;
                case 1:
                    GameManager.Instance.CurrentBackground = 0;
                    break;


            }

       //     BackgroundStageColor = 0;

        }

    }

    public void ResetBackground()
    {
        StopCoroutine(Scroll());
        movePos.y = StartPos_Y;
        rectTransform.anchoredPosition = movePos;
        GameManager.Instance.CurrentBackground = 1;

        //     Debug.Log(rectTransform.anchoredPosition);

     //   BackgroundStageColor = 0;

    }

}
