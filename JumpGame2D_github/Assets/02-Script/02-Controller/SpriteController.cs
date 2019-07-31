using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


enum spriteAtlas {Player, Obstacle_S, Obstacle_M, Obstacle_L, background_1, background_2 }

public class SpriteController : MonoBehaviour
{
    [SerializeField]
    private spriteAtlas CurrentSprite;

   
    private SpriteAtlas Atlas;
    SpriteRenderer spriteRenderer;
    RawImage rawImage;

    private void Awake()
    {

        Init();
      //  Debug.Log("ggg");
    }

    private void Init()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rawImage = gameObject.GetComponent<RawImage>();

        if (spriteRenderer != null)
        {

            StartCoroutine(GetAsset(AssetBundleState.Images, typeof(SpriteAtlas)));

        }
        else if (rawImage != null)
        {

            StartCoroutine(GetRawImage(AssetBundleState.Images, typeof(RawImage)));
        }
        else
        {
            throw new System.Exception("沒有圖形元件");
        }
       // Debug.Log("fff");

    }

    IEnumerator GetAsset(AssetBundleState assetBundleState,System.Type type)
    {
        yield return null;
        Atlas = (SpriteAtlas)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Images, "Atlas", type);
        GetSprite();
    }

    IEnumerator GetRawImage(AssetBundleState assetBundleState,System.Type type)
    {
        yield return null;
        rawImage.texture = (Texture)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Images, CurrentSprite.ToString(), typeof(Texture));
    }

    private void GetSprite()
    {
        spriteRenderer.sprite = Atlas.GetSprite(CurrentSprite.ToString());
        
    }

  


}
