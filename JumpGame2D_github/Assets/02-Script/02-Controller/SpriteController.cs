using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public enum spriteAtlas {Player, Obstacle_S, Obstacle_M, Obstacle_L_DR, Obstacle_L_DL, Obstacle_L_UR, Obstacle_L_UL, background_1, background_2 }

public class SpriteController : MonoBehaviour
{
    public spriteAtlas CurrentSprite;
    
    public SpriteAtlas Atlas;
    SpriteRenderer spriteRenderer;
    RawImage rawImage;
    ParticleSystemRenderer particleSystemRenderer;

    private void Awake()
    {

        // Init();
        //  Debug.Log("ggg");
        // GetAsset();


    }

    public void GetSpriteRendererAsset(string assetName,string spriteName)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(GetAsset(assetName, typeof(SpriteAtlas), spriteName));
    }

    public void GetRawImageAsset()
    {
        rawImage = gameObject.GetComponent<RawImage>();
        
        StartCoroutine(GetRawImage());
            
        
    }
    
    IEnumerator GetAsset(string assetName, System.Type type,string getSprite)
    {
        yield return null;
        
        Atlas = (SpriteAtlas)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Images, assetName, type);
        GetAtlasSprite(getSprite);
    }

    IEnumerator GetRawImage()
    {
        yield return null;
        rawImage.texture = (Texture)DownLoadAssetBundle.Instance.GetAsset(AssetBundleState.Images, CurrentSprite.ToString(), typeof(Texture));
    }    

    public void GetAtlasSprite(string currentSprite)
    {
        spriteRenderer.sprite = Atlas.GetSprite(currentSprite);        
    }
    
}
