using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public PanelObject[] Menu;
    public int CurrentStory;

    [System.Serializable]
    public struct PanelObject
    {
        [SerializeField]
        string name;
        public int ID;
        public int[] ActiveObjectID;
        public ImageInfo[] Image;
        public NumberInfo[] Number;
    }

    [System.Serializable]
    public struct NumberInfo
    {
        public int ID;
        public int Value;
        public bool DeletZore;
        public delegate int GetValue(int value);
    }

    [System.Serializable]
    public struct ImageInfo
    {
        public spriteAtlas ImageName;
        public int ID;

    }

    private UIBehaviour[] UICanvas;

    private Dictionary<int, CanvasGroup> PanelCollection;
    private Dictionary<int, SpriteController> ImageCollection;
    private Dictionary<int, NumberController> NumberCollection;
    private Dictionary<int, PanelObject> MenuCollection;
    private Dictionary<int, ButtonTriggerController> ButtonCollection;

    private WaitForSeconds waitSecond;
    private RectTransform ScorePanelTransform;
    private float ScoreNextVector_Y;
    private float scoreStartPos_Y;
    private Vector2 ScoreMoveVector;
    private StringBuilder playerInfo;

    int temporaryInt;
    int temporaryInt2;

    int panelCollectionLenght;
    int ImageCollectionLenght;
    int NumberCollectionLenght;
    int ButtonLenght;
    bool openThanks;

    private void Awake()
    {
        instance = this;
        waitSecond = new WaitForSeconds(0.3f);
        CurrentStory = 0;
        openThanks = false;
        playerInfo = new StringBuilder();

    }

    public void CreatDictionary(GameObject[] uIBehaviour)
    {

        UICanvas = new UIBehaviour[uIBehaviour.Length];

        for (temporaryInt = 0; temporaryInt < UICanvas.Length; temporaryInt++)
        {
            UICanvas[temporaryInt] = uIBehaviour[temporaryInt].GetComponent<UIBehaviour>();
            panelCollectionLenght += UICanvas[temporaryInt].panel.Length;
            ImageCollectionLenght += UICanvas[temporaryInt].image.Length;
            NumberCollectionLenght += UICanvas[temporaryInt].number.Length;
            ButtonLenght += UICanvas[temporaryInt].buttonEvents.Length;
        }

        PanelCollection = new Dictionary<int, CanvasGroup>(panelCollectionLenght);
        ImageCollection = new Dictionary<int, SpriteController>(ImageCollectionLenght);
        NumberCollection = new Dictionary<int, NumberController>(NumberCollectionLenght);
        MenuCollection = new Dictionary<int, PanelObject>(Menu.Length);
        ButtonCollection = new Dictionary<int, ButtonTriggerController>(ButtonLenght);

        for (temporaryInt = 0; temporaryInt < UICanvas.Length; temporaryInt++)
        {
            for (temporaryInt2 = 0; temporaryInt2 < UICanvas[temporaryInt].panel.Length; temporaryInt2++)
            {
                PanelCollection[UICanvas[temporaryInt].panel[temporaryInt2].ID] = UICanvas[temporaryInt].panel[temporaryInt2].Active;
            }

            for (temporaryInt2 = 0; temporaryInt2 < UICanvas[temporaryInt].image.Length; temporaryInt2++)
            {
                ImageCollection[UICanvas[temporaryInt].image[temporaryInt2].ID] = UICanvas[temporaryInt].image[temporaryInt2].spriteController;
            }

            for (temporaryInt2 = 0; temporaryInt2 < UICanvas[temporaryInt].number.Length; temporaryInt2++)
            {
                NumberCollection[UICanvas[temporaryInt].number[temporaryInt2].ID] = UICanvas[temporaryInt].number[temporaryInt2].numberController;
            }

            for (temporaryInt2 = 0; temporaryInt2 < UICanvas[temporaryInt].buttonEvents.Length; temporaryInt2++)
            {
                ButtonCollection[UICanvas[temporaryInt].buttonEvents[temporaryInt2].ID] = UICanvas[temporaryInt].buttonEvents[temporaryInt2].buttonController;

            }

        }

        for (temporaryInt = 0; temporaryInt < Menu.Length; temporaryInt++)
        {
            MenuCollection[Menu[temporaryInt].ID] = Menu[temporaryInt];
        }

        ScorePanelTransform = UICanvas[2].rectTransform;
        scoreStartPos_Y = ScorePanelTransform.anchoredPosition.y;
        SetButtonEven();
    }

    #region 物件設置
    public void SetObjectActive(int ID, int alpha, bool interatable, bool blockRaycast)
    {
        PanelCollection[ID].alpha = alpha;
        PanelCollection[ID].interactable = interatable;
        PanelCollection[ID].blocksRaycasts = blockRaycast;
    }

    public void CloseAllPanel()
    {
        for (temporaryInt = 0; temporaryInt < UICanvas.Length; temporaryInt++)
        {
            for (temporaryInt2 = 0; temporaryInt2 < UICanvas[temporaryInt].panel.Length; temporaryInt2++)
            {
                UICanvas[temporaryInt].panel[temporaryInt2].Active.alpha = 0;
                UICanvas[temporaryInt].panel[temporaryInt2].Active.interactable = false;
                UICanvas[temporaryInt].panel[temporaryInt2].Active.blocksRaycasts = false;
            }

        }

    }

    public void OpenMenu(int ID)
    {
        for (temporaryInt = 0; temporaryInt < MenuCollection[ID].ActiveObjectID.Length; temporaryInt++)
        {
            SetObjectActive(MenuCollection[ID].ActiveObjectID[temporaryInt], 1, true, true);
        }

    }

    public void SetNumber(int numberID, int value, int font, bool deletZore)
    {
        NumberCollection[numberID].SetNumber(value);

        if (deletZore)
        {
            NumberCollection[numberID].DeleteZero(value);
        }

        NumberCollection[numberID].ChangeColor(font);

    }

    public void SetImage(int imageID, string ImageName, bool fitSize, float scale)
    {
        ImageCollection[imageID].UIimageFitSize = fitSize;
        ImageCollection[imageID].ScaleMultiple = scale;
        ImageCollection[imageID].GetSprite(ImageName);

    }

    public void SetVerticalLayoutSpace(float space)
    {
        UICanvas[0].verticalLayoutGroup.spacing = space;
    }

    public void SetButtonEven()
    {
        ButtonCollection[0].OnClickEven = ConsumeFreeTimes_Button;
        ButtonCollection[1].OnClickEven = ConsumePoint_Button;
        ButtonCollection[2].OnClickEven = Skip_Button;
        ButtonCollection[3].OnClickEven = Again_Button;
        ButtonCollection[4].OnClickEven = Quit_Button;
        ButtonCollection[5].OnClickEven = Receive_Button;
        ButtonCollection[6].OnClickEven = Thanks_Button;
        ButtonCollection[7].OnClickEven = NextStory_Button;

    }

    public void SetPlayerInfo_PlayerInfo(string playerID)
    {
        playerInfo.Clear();
        playerInfo.AppendFormat("ID:{0}", playerID);
        UICanvas[1].PlayerID.text= playerInfo.ToString();
    }

    public void SetPlayerInfo_GameID(string gameID)
    {
        playerInfo.AppendFormat("\n{0}", gameID);
        UICanvas[1].PlayerID.text = playerInfo.ToString();
    }

    #endregion

    #region 按鈕事件們

    public void ConsumePoint_Button()
    {
        ClickSound();
        Application.ExternalCall("ConsumePlayerPoint");
        SetObjectActive(12, 1, false, false);

        UIStartStory();//*******************************************************************待刪
    }

    public void ConsumeFreeTimes_Button()
    {
        ClickSound();
        Application.ExternalCall("ConsumePlayerPoint");
        SetObjectActive(11, 1, false, false);

        UIStartStory();//*******************************************************************待刪
    }

    public void NextStory_Button()
    {
        NextStory();
    }

    public void Skip_Button()
    {
        ClickSound();
        SetObjectActive(13, 0, false, false);
        SetObjectActive(17, 0, false, false);
        SetObjectActive(0, 0, false, false);

        CurrentStory = 6;
        Story(CurrentStory);
    }

    public void Again_Button()
    {
        ClickSound();
        Application.ExternalCall("AudioPause");
        GameManager.Instance.ResetGame();
        Application.ExternalCall("JudgeFreeOrConsumePoint");
        GameManager.Instance.OpenConsumPointPanel();//*******************************************************************待刪
    }

    public void Quit_Button()
    {
        ClickSound();
        Application.ExternalCall("Quit");

    }

    public void Receive_Button()
    {
        ClickSound();
        Application.ExternalCall("Receive");

    }

    public void Thanks_Button()
    {
        ClickSound();
        SpecialThanks();
            
    }

    #endregion

    public void ClickSound()
    {
        Application.ExternalCall("AudioPlay", "Jump", 1, false);
    }

    public void OpenStartPanel(int State)//0: Free ; 1: Pay
    {
        CloseAllPanel();
        OpenMenu(0);
        ImageCollection[0].GetSprite("StoryBackground_B");
        ImageCollection[2].GetSprite("Title");
        switch (State)
        {
            case 0:
                SetImage(1, "Free", false, 1);
                SetVerticalLayoutSpace(4);
                SetObjectActive(11, 1, true, true);

                break;
            case 1:
                SetImage(1, "Coin", true, 1);
                SetObjectActive(8, 1, true, false);
                SetVerticalLayoutSpace(10);
                SetObjectActive(12, 1, true, true);

                break;
        }
    }

    public void UIStartStory()//****************************在GameManager裡被Javascript呼叫
    {
        StartCoroutine(WaitSecond());

    }

    IEnumerator WaitSecond()
    {
        yield return waitSecond;

        CloseAllPanel();
        OpenMenu(1);
        Story(0);
    }

    public void NextStory()
    {
        CurrentStory += 1;
        Story(CurrentStory);
    }

    public void Story(int stroyPage)
    {
        switch (stroyPage)
        {
            case 0:
                SetImage(10, "StoryWater_0", false, 0);
                SetImage(11, "StoryWater_0", false, 0);
                SetImage(12, "StoryText_0", true, 1f);
                break;

            case 1:
                SetImage(11, "StoryWater_1", false, 0);
                SetImage(12, "StoryText_1", true, 1f);

                ImageCollection[10].FadeOut(1.5f);
                ImageCollection[11].FadeIn(1.3f);

                ClickSound();
                break;

            case 2:
                SetImage(10, "StoryWater_2", false, 0);
                SetImage(12, "StoryText_2", true, 1f);

                ImageCollection[11].FadeOut(1.5f);
                ImageCollection[10].FadeIn(1.3f);

                ClickSound();
                break;

            case 3:
                SetImage(11, "StoryWater_3", false, 0);
                SetImage(12, "StoryText_3", true, 1f);
                SetImage(0, "StoryBackground_P", false, 0);

                ImageCollection[10].FadeOut(1.5f);
                ImageCollection[11].FadeIn(1.3f);

                ClickSound();
                break;

            case 4:
               
                SetObjectActive(4, 0, false, false);
                SetObjectActive(13, 0, false, false);
                SetObjectActive(2, 0, false, false);
                SetObjectActive(0, 0, false, false);
                SetObjectActive(17, 0, false, false);

                SetImage(0, "Story_5", false, 0);
               
                Application.ExternalCall("AudioPlay", "Boss", 1, false);
                break;

            case 5:

                SetObjectActive(4, 1, false, false);
                SetObjectActive(2, 1, true, false);
                SetImage(0, "StoryBackground_P", false, 0);
                SetImage(11, "StoryWater_4", false, 0);
                SetImage(12, "StoryText_4", true, 1f);

                ImageCollection[11].FadeIn(1.3f);

                Application.ExternalCall("AudioFadeOut");
                Application.ExternalCall("AudioPlay", "Dead", 1, false);
                break;

            case 6:
                SetObjectActive(4, 0, false, false);
                SetObjectActive(2, 0, false, false);
                SetImage(0, "NoviceTeaching", false, 0);

                break;

          
            default:
                CurrentStory = 0;
                CloseAllPanel();
                OpenMenu(4);
                Application.ExternalCall("AudioStopFadeOut");
               
               GameManager.Instance.StartGame();

                break;
        }

    }

    public void SpecialThanks()
    {
        if (!openThanks)
        {
            openThanks = true;
            CloseAllPanel();
            OpenMenu(2);

            SetImage(0, "StoryBackground_B", false, 0);
        }
        else
        {
            openThanks = false;
            CloseAllPanel();
            OpenMenu(1);
            Story(CurrentStory);
        }
    }

    public void OpenResultPanel(int winLose)
    {
        CloseAllPanel();
        OpenMenu(3);
        ImageCollection[2].GetSprite("Result");
        SetNumber(7, StageDataController.Instance.TotalScore, 4, true);
        SetVerticalLayoutSpace(-5);

        switch (winLose)
        {
            case 0:
                ImageCollection[0].GetSprite("Win");
                SetObjectActive(16, 1, true, true);
                break;
            case 1:
                ImageCollection[0].GetSprite("Lose");
                SetObjectActive(14, 1, true, true);

                break;
        }
    }

    public void OpenTimesUP()
    {
        SetObjectActive(24, 1, false, false);
    }

    public void OpenAgainButton()
    {
        SetObjectActive(14, 1, true, true);
    }

    public void OpenReceiveButton()
    {
        SetObjectActive(16, 1, true, true);

    }

    #region 分數
    public void ResetText()
    {
        SetImage(13, "Time_0", false, 1);
        SetImage(14, "colon_0", false, 1);
        SetImage(15, "Score_0", false, 1);
        SetImage(16, "Plus_0", false, 1);

        NumberCollection[5].ChangeColor(0);//score
        NumberCollection[5].DeleteZero(0);//score
    }

    public void SetPlayTextColor(int color)
    {
        ImageCollection[13].GetSprite("Time_" + color);//time_text
        ImageCollection[14].GetSprite("colon_" + color);//colon_text
        ImageCollection[15].GetSprite("Score_" + color);//score_text
        ImageCollection[16].GetSprite("Plus_" + color);//plus_text
       
        NumberCollection[3].ChangeColor(color);//minute
        NumberCollection[4].ChangeColor(color);//second
        NumberCollection[5].ChangeColor(color);//score
        NumberCollection[6].ChangeColor(color);//add score 
    }

    public void AddScore(int value)
    {
        // NumberCollection[5].SetNumber(5);
        SetObjectActive(20, 1, false, false);
        NumberCollection[6].SetNumber(value);
        NumberCollection[6].DeleteZero(value);

        NumberCollection[6].FadeOut(1);
        ImageCollection[16].FadeOut(1);
        StartCoroutine(scrollScore());

        NumberCollection[5].SetNumber(StageDataController.Instance.TotalScore);
        NumberCollection[5].DeleteZero(StageDataController.Instance.TotalScore);

    }

    public void SetTime(int minute,int second)
    {
        NumberCollection[3].SetNumber(minute);
        NumberCollection[4].SetNumber(second);

    }

    IEnumerator scrollScore()
    {
        ScoreMoveVector.x = ScorePanelTransform.anchoredPosition.x;
        ScoreMoveVector.y = ScorePanelTransform.anchoredPosition.y;

        ScoreNextVector_Y = ScorePanelTransform.anchoredPosition.y + 40;
        while (ScorePanelTransform.anchoredPosition.y < ScoreNextVector_Y)
        {
            ScoreMoveVector.y += 45 * Time.deltaTime;
            ScorePanelTransform.anchoredPosition = ScoreMoveVector;

            yield return null;

        }
        ResetPlusScore();
    }

    public void ResetPlusScore()
    {
        ScoreMoveVector.y = scoreStartPos_Y;
        ScorePanelTransform.anchoredPosition = ScoreMoveVector;

    }
    #endregion
}
