using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public enum Order
    {
        First,
        second
    }
    private Transform m_transform;
    private float position_Y;
    private Vector2 movePos;
    public Order currentOder;
    private float height;

    public float scrollDis;
    public float Speed;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_transform = transform;
        movePos = Vector2.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Scroll());

        }

    }

    IEnumerator Scroll()
    {
        while (m_transform.position.y <= -scrollDis)
        {
            position_Y = transform.position.y;
            position_Y -= Speed * Time.deltaTime;
            movePos.y = position_Y;
            m_transform.position = movePos;
            yield return null;


        }

      //  movePos.y= height;

    }



}
