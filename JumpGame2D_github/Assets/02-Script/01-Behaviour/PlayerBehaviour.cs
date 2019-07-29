using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;
using UnityEngine.UI;


public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour Instance;
    private PlayerJsonData playerData;


    #region Component
    private Rigidbody2D rigidbody2;

    #endregion

    private void Awake()
    {
        Init();

    }

    #region Init
    private void Init()
    {
        Instance = this;
        rigidbody2 = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(LoadData());

    }
    #endregion
    
    IEnumerator LoadData()
    {
        yield return new WaitForEndOfFrame();
        playerData = StageDataController.Instance.PlayerJson;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Jump();

        }
    }

    public void ResetPlayer()
    {
        transform.parent.gameObject.transform.position = new Vector3(0, -4, 0);
        transform.localPosition = Vector2.zero;
    }
    
    public void SwitchControlPlayer(bool state)
    {
        SwitchRigidbody(state);

    }

    public void ScrollPlayer(float scrollDis, float speed)
    {
        Scroll(scrollDis, speed);


    }

    public void Jump()
    {
        rigidbody2.gravityScale = 0;
        rigidbody2.mass = 0;
        rigidbody2.velocity = Vector2.zero;
        rigidbody2.AddForce(new Vector2(0, playerData.JumpForce));//1500

        rigidbody2.gravityScale = playerData.Gravity;//2
        rigidbody2.mass = playerData.Weight;//5
        
    }

    public void SwitchRigidbody(bool State)
    {

        rigidbody2.simulated = State;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Obstacle":
                GameManager.Instance.GameOver();

                break;
            case "Scroll":
                GameManager.Instance.NextObstacle();
                
                break;
                
        }


    }

    public void Scroll(float scrollDis,float speed)
    {
        float dis = 0;
        StartCoroutine(ScrollPosition(dis, scrollDis, speed));

    }

    IEnumerator ScrollPosition(float dis,float ScrollPos,float speed)
    {
        Vector3 currPos = transform.parent.gameObject.transform.position;
        while (dis < 1) 
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.GameOver)
            {
                break;
            }
            else
            {
                transform.parent.gameObject.transform.position = Vector3.Lerp(transform.parent.gameObject.transform.position, currPos + new Vector3(0, ScrollPos, 0), dis);
                dis += speed * Time.deltaTime;
                yield return null;
            }
        }
    }
    
}
