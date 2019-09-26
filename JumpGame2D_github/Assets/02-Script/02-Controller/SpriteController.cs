using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public enum spriteAtlas
{//R 0,B 1,G 2,Y 3
    None,
    PlayBackground, NoviceTeaching, StoryBackground_B, StoryBackground_P, Story_5,
    Skip_Down, Skip_Up,
    Border_Up, Border_Down,
    Again_Up,Again_Down,
    Receive_Up, Receive_Down,
    OK_Up,OK_Down,
    Free_Up,Free_Down,
    StoryWater_0, StoryWater_1, StoryWater_2, StoryWater_3, StoryWater_4,
    StoryText_0, StoryText_1, StoryText_2, StoryText_3, StoryText_4,
    StartPanel,
    StartPanel_Free,
    Win,Lose,
    ResultPanel,
    Quit_Up, Quit_Down,
    ScorePoint_0, ScorePoint_1, ScorePoint_2, ScorePoint_3,

    //紅色 = 0,藍色 = 1,綠色 = 2,黃色 = 3
    Score_0, Score_1, Score_2,Score_3,//Score_顏色
    Time_0, Time_1, Time_2, Time_3,//Time_顏色

    //Number_顏色數字
    Number_00, Number_01, Number_02, Number_03, Number_04, Number_05, Number_06, Number_07, Number_08, Number_09,
    Number_10, Number_11, Number_12, Number_13, Number_14, Number_15, Number_16, Number_17, Number_18, Number_19,
    Number_20, Number_21, Number_22, Number_23, Number_24, Number_25, Number_26, Number_27, Number_28, Number_29,
    Number_30, Number_31, Number_32, Number_33, Number_34, Number_35, Number_36, Number_37, Number_38, Number_39,
    Number_40, Number_41, Number_42, Number_43, Number_44, Number_45, Number_46, Number_47, Number_48, Number_49,

    Slash, colon_0, colon_1, colon_2, colon_3,//Colon_顏色

    Plus_0, Plus_1, Plus_2, Plus_3,
    TimesUP,SpecialThank, SpecialThank_1, SpecialThank_2, SpecialThank_3,SpecialThanks_small,

}

enum SpriteComponent
{
    RawImage,
    SpriteRender,
    UIImage,

}

public class SpriteController : MonoBehaviour
{
    public spriteAtlas CurrentSprite;
    private SpriteComponent CurrentSpriteCompnet;

    public string AtlasName;
    public SpriteAtlas Atlas;
    private Color alpha;
    SpriteRenderer spriteRenderer;
    RawImage rawImage;
    Image UIimage;
    ParticleSystemRenderer particleSystemRenderer;

    IEnumerator fadeInCoroutine;
    IEnumerator fadeOutCoroutine;


    private void Awake()
    {       
        rawImage = gameObject.GetComponent<RawImage>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        UIimage = gameObject.GetComponent<Image>();
        alpha = new Color(0, 0, 0, 1);

        fadeInCoroutine = fadeIn(1);
        fadeOutCoroutine = fadeOut(1);

        if(AtlasName != string.Empty)
        {
            GetAtlas(AtlasName);

        }
      
        if (rawImage != null)
        {
            CurrentSpriteCompnet = SpriteComponent.RawImage;
            StartCoroutine(GetRawImage());
        }
            
    }

    public void GetAtlas(string atlasName)
    {
        StartCoroutine(GetAsset(atlasName, typeof(SpriteAtlas)));
    }

    IEnumerator GetAsset(string assetName, System.Type type)
    {
        yield return null;

        Atlas = (SpriteAtlas)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Images, assetName, type);
     
        if (spriteRenderer != null)
        {
            CurrentSpriteCompnet = SpriteComponent.SpriteRender;
            if (CurrentSprite != spriteAtlas.None)
            {
                GetSprite(CurrentSprite.ToString());
            }
        }
        else if (UIimage != null)
        {

            CurrentSpriteCompnet = SpriteComponent.UIImage;

            if (CurrentSprite != spriteAtlas.None)
            {
                // Debug.Log("j");

                GetSprite(CurrentSprite.ToString());
            }
        }

    }

    IEnumerator GetRawImage()
    {
        yield return null;
        rawImage.texture = (Texture)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Images, CurrentSprite.ToString(), typeof(Texture));
    }    

    public void GetSprite(string currentSprite)
    {
        switch (CurrentSpriteCompnet)
        {
            case SpriteComponent.SpriteRender:
                spriteRenderer.sprite = Atlas.GetSprite(currentSprite);

                break;

            case SpriteComponent.UIImage:

               // Debug.Log(Atlas.spriteCount);
                UIimage.sprite = Atlas.GetSprite(currentSprite);

                break;
                               
        }

    }

    public void FadeIn(float fadeInspeed)
    {
        StopCoroutine(fadeOutCoroutine);
        StopCoroutine(fadeInCoroutine);
        fadeInCoroutine = fadeIn(fadeInspeed);
        StartCoroutine(fadeInCoroutine);

    }

    IEnumerator fadeIn(float fadeInSpeed)
    {
        ResetAlpha(0);

        switch (CurrentSpriteCompnet)
        {
            case SpriteComponent.SpriteRender:
                while (alpha.a < 1)
                {

                    alpha.r = spriteRenderer.color.r;
                    alpha.g = spriteRenderer.color.g;
                    alpha.b = spriteRenderer.color.b;
                    alpha.a += fadeInSpeed * Time.deltaTime;

                    spriteRenderer.color = alpha;

                    yield return null;

                }
                break;

            case SpriteComponent.UIImage:
                while (alpha.a < 1)
                {
                    alpha.r = UIimage.color.r;
                    alpha.g = UIimage.color.g;
                    alpha.b = UIimage.color.b;
                    alpha.a += fadeInSpeed * Time.deltaTime;

                    UIimage.color = alpha;

                    yield return null;

                }
                break;

        }


    }

    public void FadeOut(float fadeOutSpeed)
    {
        StopCoroutine(fadeOutCoroutine);
        StopCoroutine(fadeInCoroutine);
        fadeOutCoroutine = fadeOut(fadeOutSpeed);
        StartCoroutine(fadeOutCoroutine);
    }

    IEnumerator fadeOut(float fadeOutSpeed)
    {
        ResetAlpha(1);
        switch (CurrentSpriteCompnet)
        {
            case SpriteComponent.SpriteRender:
                while (alpha.a > 0)
                {
                    
                    alpha.r = spriteRenderer.color.r;
                    alpha.g = spriteRenderer.color.g;
                    alpha.b = spriteRenderer.color.b;
                    alpha.a -= fadeOutSpeed * Time.deltaTime;

                    spriteRenderer.color = alpha;

                    yield return null;

                }

                break;

            case SpriteComponent.UIImage:
                while (alpha.a > 0)
                {
                    alpha.r = UIimage.color.r;
                    alpha.g = UIimage.color.g;
                    alpha.b = UIimage.color.b;
                    alpha.a -= fadeOutSpeed * Time.deltaTime;

                    UIimage.color = alpha;

                    yield return null;

                }

                break;
        }
    }

    public void ResetAlpha(int a)
    {
        switch (CurrentSpriteCompnet)
        {
            case SpriteComponent.SpriteRender:
                alpha.r = spriteRenderer.color.r;
                alpha.g = spriteRenderer.color.g;
                alpha.b = spriteRenderer.color.b;
                alpha.a = a;
                spriteRenderer.color = alpha;
                break;

            case SpriteComponent.UIImage:
                alpha.r = UIimage.color.r;
                alpha.g = UIimage.color.g;
                alpha.b = UIimage.color.b;
                alpha.a = a;
                UIimage.color = alpha;
                break;
        }
        
    }

    public void InvisibleUIImage()
    {
        gameObject.SetActive(false);
    }

    public void VisibleUIImage()
    {
        gameObject.SetActive(true);
    }

}
