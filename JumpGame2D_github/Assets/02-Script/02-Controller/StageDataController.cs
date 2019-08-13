using System.Collections;
using UnityEngine;

public class StageDataController : MonoBehaviour
{
    //public PlayerStageData playerStageData;

    public static StageDataController Instance;
    public PlayerJsonData PlayerJson;
    public LevelSettingJsonData LevelSettingJson;

    private void Awake()
    {
        Instance = this;
        GetData();
    }
    
    public void GetData()
    {
        StartCoroutine("GetJson");

    }

    IEnumerator GetJson()
    {
        WWWForm form = new WWWForm();
        LevelSettingJsonData levelSetting = new LevelSettingJsonData();

      //  WWW www = new WWW("http://localhost/PHP.php");
        WWW www = new WWW("http://192.168.0.137/PHP.php");


        yield return www;
        //   string test2 = JsonUtility.ToJson(playerDatas);
        if (www != null)
        {

            PlayerJson = new PlayerJsonData();
            PlayerJson = JsonUtility.FromJson<PlayerJsonData>(www.text.Trim("[]".ToCharArray()));

        }
        else
        {

        }

  

    }


}
