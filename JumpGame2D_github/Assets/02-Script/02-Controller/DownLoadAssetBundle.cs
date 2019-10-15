using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
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

    //  public bool IsDownLoaded;
    string path1;
    //  public WWW www1;
    public UnityWebRequest request;
    object asset;
    AudioClip audioAsset;

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
    }

    public void SetIP(string url)
    {
        path1 = url;
    }

    public void LoadAssetBundle(AssetBundleState assetBundleState, string assetBundleName)
    {
        StartCoroutine(LoadAsset(assetBundleState, assetBundleName));
    }

    IEnumerator LoadAsset(AssetBundleState assetBundleState, string assetBundleName)
    {
        request = UnityWebRequestAssetBundle.GetAssetBundle("http://192.168.0.137:8080/games/games011/AssetBundle/" + assetBundleName + ".unityassetbundle");
        yield return request.SendWebRequest();
        // AssetBundle ab = DownloadHandlerAssetBundle.GetContent (request );

      //  Debug.Log((request.downloadHandler as DownloadHandlerAssetBundle).assetBundle.name);
        AssetBundleDictionary[(int)assetBundleState] = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
        Debug.Log(AssetBundleDictionary[(int)assetBundleState]);
            
    }

    public object GetAsset(AssetBundleState assetBundleState,string ObjectName,System.Type type)
    {

        /*  for (int i = 0; i < AssetBundleDictionary[(int)assetBundleState].GetAllAssetNames().Length; i++)
          {
              Debug.Log(AssetBundleDictionary[(int)assetBundleState].GetAllAssetNames()[i]);
          }*/

        if (AssetBundleDictionary[(int)assetBundleState] != null)
        {
            asset = AssetBundleDictionary[(int)assetBundleState].LoadAsset(ObjectName, type);   //加载ab1包中的资源名为 Sphere-Head 文件的数据，返回Object对象 （这是一个预设物）

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

    public void UnloadAllAssetBundle()
    {
        for (int i = 0; i < AssetBundleDictionary.Count; i++)
        {
            AssetBundleDictionary[i].Unload(true);
           // Resources.UnloadAsset(AssetBundleDictionary[i]);

        }

    }
    
}