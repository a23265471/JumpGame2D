using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum AssetBundleState
{
    Images,
    Prefab,
    UIPrefab,
    Audio
}

public class DownLoadAssetBundle : MonoBehaviour
{
    public static DownLoadAssetBundle Instance;

    private Dictionary<int, AssetBundle> AssetBundleDictionary;

  //  public bool IsDownLoaded;

    public WWW www1;

  //  WaitUntil waitUntil;

    private void Awake()
    {
        Init();
       // GameManager.Instance.Init();
    }

    public void Init()
    {
        Instance = this;
        AssetBundleDictionary = new Dictionary<int, AssetBundle>();
   //     waitUntil = new WaitUntil(()=>www1.isDone);
       // LoadAssetBundle(AssetBundleState.Images, "loadstage");
        //LoadAssetBundle(AssetBundleState.Prefab, "prefab");

    }

    public void LoadAssetBundle(AssetBundleState assetBundleState, string assetBundleName)
    {
        StartCoroutine(LoadAsset(assetBundleState, assetBundleName));
    }

    IEnumerator LoadAsset(AssetBundleState assetBundleState, string assetBundleName)
    {
        //  IsDownLoaded = false;
        //  string path1 = "http://192.168.0.137/public/UnityAssetBundle/" + assetBundleName + ".unityassetbundle"; //本地资源包路径
        string path1 = "http://localhost/public/UnityAssetBundle/" + assetBundleName + ".unityassetbundle"; //本地资源包路径

        while (Caching.ready == false)yield return null;   //是否准备好
        www1 = WWW.LoadFromCacheOrDownload(@path1, 1);
        //   StartCoroutine(DownLoadProgress());

        yield return www1;


        AssetBundleDictionary[(int)assetBundleState] = www1.assetBundle;
    }

  /*  IEnumerator DownLoadProgress()
    {
      //  Debug.Log("DownLoad");

        yield return waitUntil;
        IsDownLoaded = true;
     //   Debug.Log(www1.progress);


    }
    */

    public object GetAsset(AssetBundleState assetBundleState,string ObjectName,System.Type type)
    {

         /* for (int i = 0; i < AssetBundleDictionary[(int)assetBundleState].GetAllAssetNames().Length; i++)
          {
              Debug.Log(AssetBundleDictionary[(int)assetBundleState].GetAllAssetNames()[i]);
          }*/
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