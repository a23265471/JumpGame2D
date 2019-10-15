using System.Collections;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static PlayerBehaviour Instance;
    public PlayerJsonData playerData;

    #region Component
    private Rigidbody2D rigidbody2;
    private SpriteRenderer spriteRenderer;
    public Animator Animator;
    Sprite idleSprite;
    #endregion

    Vector3 currentPos;
    Vector3 startPos;
    Vector3 jumpVector;
    Transform m_transform;


    private void Awake()
    {
     //   Debug.Log("1.load player script");
        Init();

    }
   
    #region Init
    private void Init()
    {
        Instance = this;
        rigidbody2 = gameObject.GetComponent<Rigidbody2D>();
        Animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_transform = GetComponent<Transform>();
        idleSprite = spriteRenderer.sprite;
      //  Application.targetFrameRate = 60;
        startPos = new Vector3(0, -4, 0);
    }
    #endregion

    public void LoadData()
    {
        StartCoroutine(loadData());
    }

    IEnumerator loadData()//*******************************************要改回來
    {
        yield return null;
        playerData = StageDataController.Instance.PlayerJson;

        jumpVector = new Vector2(0, playerData.JumpForce);//1500
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            Jump();
          //  Debug.Log(playerData.JumpForce);
        }

    }

    public void ResetPlayer()
    {
        m_transform.parent.gameObject.transform.position = startPos;
        Animator.SetTrigger("Idle");
        spriteRenderer.sprite = idleSprite;
        m_transform.localPosition = Vector2.zero;
    }
    
    public void SwitchControlPlayer(bool state)
    {
        SwitchRigidbody(state);

    }

    public void ScrollPlayer(float scrollDis, float speed)
    {
        Scroll(scrollDis, speed);


    }

    public void Jump()//*******************************************要改回來
    {
        Animator.SetTrigger("UP");

        Application.ExternalCall("AudioPlay","Jump", 1, false);
       // AudioController.Instance.PlayAudio(1, 5, false);
        StopCoroutine(JumpAnimatoinControl());
        StartCoroutine(JumpAnimatoinControl());

        rigidbody2.gravityScale = 0;
        rigidbody2.mass = 0;
        rigidbody2.velocity = Vector2.zero;
        rigidbody2.AddForce(jumpVector);

        rigidbody2.gravityScale =playerData.Gravity;//2
        rigidbody2.mass = playerData.Weight;//5
        
    }

    IEnumerator JumpAnimatoinControl()
    {

        while (rigidbody2.velocity.y > 0.2f)
        {
            yield return null;
        }
        Animator.SetTrigger("Fall");
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
                Application.ExternalCall("AudioPlay","Dead", 1, false);
                //AudioController.Instance.PlayAudio(1, 3, false);

                Animator.SetTrigger("Dead");
                GameManager.Instance.GameOver();
             //   Debug.Log(other.name);

                break;
            case "Scroll":
                GameManager.Instance.NextObstacle();
                
                break;
          
        }


    }

    public void AnimatorEnable()
    {
        Animator.enabled = false;

    }


    public void Scroll(float scrollDis,float speed)
    {
        float dis = 0;
        StartCoroutine(ScrollPosition(dis, scrollDis, speed));

    }

    IEnumerator ScrollPosition(float dis,float ScrollPos,float speed)
    {
        currentPos = transform.parent.gameObject.transform.position;
        while (dis < 1) 
        {
            if (GameManager.Instance.CurrentGameState == GameManager.GameState.GameOver)
            {
                break;
            }
            else
            {
                m_transform.parent.gameObject.transform.position = Vector3.Lerp(m_transform.parent.gameObject.transform.position, currentPos + new Vector3(0, ScrollPos, 0), dis);
                dis += speed * Time.deltaTime;
                yield return null;
            }
        }
    }
    
    public void CloseBehaviour()
    {
        Instance.enabled = false;

    }

    public void OpenBehaviour()
    {
        Instance.enabled = true;

    }
}
