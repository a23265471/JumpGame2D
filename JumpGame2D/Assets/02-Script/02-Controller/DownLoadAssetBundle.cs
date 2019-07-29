using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AssetBundleState
{
    Images,
    Prefab,
    Audio
}

public class DownLoadAssetBundle : MonoBehaviour
{
    public static DownLoadAssetBundle Instance;

    private Dictionary<int, AssetBundle> AssetBundleDictionary;
    
    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        Instance = this;
        AssetBundleDictionary = new Dictionary<int, AssetBundle>();
        LoadAssetBundle(AssetBundleState.Images, "loadstage");
        LoadAssetBundle(AssetBundleState.Prefab, "prefab");
    }

    public void LoadAssetBundle(AssetBundleState assetBundleState, string assetBundleName)
    {
        StartCoroutine(LoadAsset(assetBundleState, assetBundleName));
    }

    IEnumerator LoadAsset(AssetBundleState assetBundleState, string assetBundleName)
    {
        string path1 = "file:D:/MoonMoonGames/00-TestGame/2D/UnityAssetBundle/"+ assetBundleName + ".unityassetbundle"; //本地资源包路径
        while (Caching.ready == false)yield return null;   //是否准备好
        WWW www1 = WWW.LoadFromCacheOrDownload(@path1, 1);                                                            //从本地加载
        AssetBundleDictionary[(int)assetBundleState] = www1.assetBundle;
    }

    public object GetAsset(AssetBundleState assetBundleState,string ObjectName,System.Type type)
    {
        if (AssetBundleDictionary[(int)assetBundleState] != null)
        {
            object asset = AssetBundleDictionary[(int)assetBundleState].LoadAsset(ObjectName, type);   //加载ab1包中的资源名为 Sphere-Head 文件的数据，返回Object对象 （这是一个预设物）

            if (asset != null)
            {
                return asset;

            }
            else
            {
                throw new System.Exception("沒找到該物件");
            }

        }
        else
        {
            throw new System.Exception("AssetBundle 沒有被加載");

        }

    }

}