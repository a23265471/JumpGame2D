using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffect : MonoBehaviour
{
    public static ScoreEffect instance;

    public float FadeOutSpeed;

    SpriteController spriteController;
    CircleCollider2D circleCollider2D;
    SpriteRenderer spriteRenderer;
    Color alpha;
    

    private void Awake()
    {
        instance = this;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
        spriteController = gameObject.GetComponent<SpriteController>();
        alpha = new Color(0,0,0);
        alpha.a = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        spriteController.FadeOut(FadeOutSpeed);
        circleCollider2D.enabled = false;

        Application.ExternalCall("AudioPlay", "GetScore", 1,false);
        // AudioController.Instance.PlayAudio(2, 4, false);

        //  UIPanelController.instance.AddScore(StageDataController.Instance.GetCurrentObstacleScore());
        UIController.instance.AddScore(StageDataController.Instance.GetCurrentObstacleScore());
    }

    public void GetSprite(int color)
    {
        spriteController.GetSprite("ScorePoint_" + color);
    }

    public void ResetState()
    {
        circleCollider2D.enabled = true;

        spriteController.ResetAlpha(1);
    }


}
