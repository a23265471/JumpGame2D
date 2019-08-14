using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
  //  public static BackgroundScroll Instance;
    private Vector2 startPos;
    private Transform m_transform;
    private RectTransform rectTransform;
    private float position_Y;
    private Vector2 movePos;
    private float height;

    public float scrollDis;
    public float Speed;
    private float nextPosY;

    [SerializeField]
    private RectTransform otherTrans;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
     //   Instance = this;
        m_transform = transform;
        startPos = transform.position;
        rectTransform = GetComponent<RectTransform>();
        movePos = Vector2.zero;
    }

  /*  private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ScrollBackground();

        }
    }
    */

    public void ScrollBackground()
    {
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
       
        nextPosY = rectTransform.anchoredPosition.y - scrollDis;
        while (rectTransform.anchoredPosition.y >= nextPosY)
        {
            position_Y = rectTransform.anchoredPosition.y;
            position_Y -= Speed * Time.deltaTime;
            movePos.y = position_Y;
            rectTransform.anchoredPosition = movePos;
            yield return null;


        }

        if (rectTransform.anchoredPosition.y <= -1200)
        {
            movePos.y = otherTrans.anchoredPosition.y + 1200;
            rectTransform.anchoredPosition = movePos;

        }

    }

    public void ResetBackground()
    {
        m_transform.position = startPos;

    }

}
