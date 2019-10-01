using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour
{
    public static UIPanelController instance;

    [Header ("視窗們")]
    [SerializeField]
    private GameObject uiPanel;
    [SerializeField]
    private SpriteController BackgroundSpriteContoller;
    [SerializeField]
    private GameObject BorderPanel;
    [SerializeField]
    private GameObject MessagePanel;
    private SpriteController MessagePanelSpriteController;
    [SerializeField]
    private GameObject PlayUI;
    [SerializeField]
    private GameObject SpecialThanksPanel;
    [SerializeField]
    private GameObject StoryPanel;


    [Header("故事們")]
    #region 故事們
    [SerializeField]
    private GameObject[] waterPanel = new GameObject[2];
    private SpriteController[] waterSpriteController = new SpriteController[2];
    [SerializeField]
    private GameObject TextPanel;
    private SpriteController TextSpriteController;
    #endregion


    [Header("按鈕們")]
    #region 按鈕們
    [SerializeField]
    private GameObject AgainBottun;

    [SerializeField]
    private GameObject PayForPalyButton;

    [SerializeField]
    private GameObject FreeButon;

    [SerializeField]
    private GameObject QuitButton;

    [SerializeField]
    private GameObject ReceiveButton;

    [SerializeField]
    private GameObject skipButtom;

    [SerializeField]
    private GameObject NextStoryButtom;

    [SerializeField]
    private GameObject SpecialThanksButton;

    #endregion

    [Header("文字們")]
    #region 文字們
    [SerializeField]
    private GameObject FreeTimesNumber;

    [SerializeField]
    private GameObject PayForPlayNumber;

    [SerializeField]
    private GameObject ResultScore;

    [SerializeField]
    private GameObject TimesUP;

    [SerializeField]
    private SpriteController scoreText;
    [SerializeField]
    private SpriteController colonText;
    [SerializeField]
    private SpriteController timeText;
    private SpriteController TimesUPSpriteController;

    [SerializeField]
    private Text PlayerInfo;

    #endregion

    [Header("數字們")]
    #region 文字們

    [SerializeField]
    private NumberController MinuteText;

    [SerializeField]
    private NumberController SecondText;

    [SerializeField]
    private NumberController ScoreText;

    [SerializeField]
    private NumberController ShowConsumePoint;

    [SerializeField]
    private NumberController ConsumePoint;

    [SerializeField]
    private NumberController CustomerPoint;    

    [SerializeField]
    private NumberController PresetFreeCount;

    [SerializeField]
    private NumberController PlayerFreeCount;

    private NumberController ResultScoreText;

    [SerializeField]
    private GameObject PlusScore;


    //   private AudioController audioController;
    private RectTransform plusScoreTransform;
    private NumberController plusScoreText;
    private SpriteController PlusSpriteController;
    public float ScrollDis;
    public float AddScoreFadeOutSpeed;
    private float plusScoreNext_Y;
    private Vector2 plusScoreScrollDis;
    public float PlusScoreScrollSpeed;

    private WaitForSeconds waitForStory;
    #endregion

    private void Awake()
    {
        instance = this;
        uiPanel = transform.GetChild(0).gameObject;

        TextSpriteController = TextPanel.GetComponent<SpriteController>();
        waterSpriteController[0] = waterPanel[0].GetComponent<SpriteController>();
        waterSpriteController[1] = waterPanel[1].GetComponent<SpriteController>();


        MessagePanelSpriteController = MessagePanel.transform.GetChild(0).GetComponent<SpriteController>();
        plusScoreText = PlusScore.transform.GetChild(1).gameObject.GetComponent<NumberController>();
        ResultScoreText = ResultScore.GetComponent<NumberController>();
        PlusSpriteController = PlusScore.transform.GetChild(0).gameObject.GetComponent<SpriteController>();
        plusScoreTransform = PlusScore.GetComponent<RectTransform>();

        TimesUPSpriteController = TimesUP.GetComponent<SpriteController>();
        
        plusScoreScrollDis = new Vector2(0, 0);
        waitForStory = new WaitForSeconds(0.5f);
       // SetPlayerInfo("Sindy", "201909265645dfs5");
    }

    public void SetPlayerInfo_PlayerID(string PlayerID)
    {
        PlayerInfo.text = "ID : " + PlayerID;

    }

    public void SetPlayerInfo_GameID(string GameID)
    {
        PlayerInfo.text += "\n" + GameID;
    }

    public void OpenPayPointPanel()
    {
        StoryPanel.SetActive(false);
        skipButtom.SetActive(false);
        NextStoryButtom.SetActive(false);

        PlayUI.SetActive(false);
        SpecialThanksPanel.SetActive(false);

        BorderPanel.SetActive(true);
        MessagePanel.SetActive(true);
        MessageSelect(1);

        ChangeSprite(BackgroundSpriteContoller, "StoryBackground_B");

    }

    public void OpenFreeTimesPanel()
    {
        StoryPanel.SetActive(false);
        skipButtom.SetActive(false);
        NextStoryButtom.SetActive(false);

        PlayUI.SetActive(false);
        SpecialThanksPanel.SetActive(false);

        BorderPanel.SetActive(true);
        MessagePanel.SetActive(true);
        MessageSelect(0);

        ChangeSprite(BackgroundSpriteContoller, "StoryBackground_B");

    }


    public void GameOverPanel(int ResultBackground)//ResultBackground=0 >> win , ResultBackground=1 >> Lose
    {
        switch (ResultBackground)
        {
            case 0:
                uiPanel.SetActive(true);
                MessagePanel.SetActive(false);
                BorderPanel.SetActive(false);
                StoryPanel.SetActive(false);


                ChangeSprite(BackgroundSpriteContoller, "Win");
               
                break;

            case 1:
                uiPanel.SetActive(true);
                MessagePanel.SetActive(false);
                BorderPanel.SetActive(false);
                StoryPanel.SetActive(false);

                ChangeSprite(BackgroundSpriteContoller, "Lose");
                break;               
        }
    }

    public void UIOpenResultPanel()
    {
        MessagePanel.SetActive(true);
        MessageSelect(2);
        
    }
    
    public void Story(int stroyPage)
    {
       // uiPanel.SetActive(true);

        switch (stroyPage)
        {
            case 0:
                CloseAllButton();
                MessagePanel.SetActive(false);
                BorderPanel.SetActive(true);
                skipButtom.SetActive(true);
                TextPanel.SetActive(true);
                NextStoryButtom.SetActive(true);

                waterPanel[0].SetActive(true);//111
                waterPanel[1].SetActive(false);//111

             //   Application.ExternalCall("AudioPlay", "Jump", 1, false);
                //AudioController.Instance.PlayAudio(1, 5, false);
                Application.ExternalCall("AudioPlay", "BGM_Story", 1, true);


                ChangeSprite(BackgroundSpriteContoller, "StoryBackground_B");
                ChangeSprite(TextSpriteController, "StoryText_0");

                waterSpriteController[0].ResetAlpha(1);
                ChangeSprite(waterSpriteController[0], "StoryWater_0");//111

                SpecialThanksButton.SetActive(true);

                break;
            case 1:
                Application.ExternalCall("AudioPlay", "Jump", 1, false);
                // AudioController.Instance.PlayAudio(1, 5, false);

                waterPanel[1].SetActive(true);

                ChangeSprite(BackgroundSpriteContoller, "StoryBackground_B");
                ChangeSprite(waterSpriteController[1], "StoryWater_1");
                ChangeSprite(TextSpriteController, "StoryText_1");

                waterSpriteController[0].FadeOut(1f);
                waterSpriteController[1].FadeIn(3f);
              //  TextSpriteController.FadeIn(1f);
                break;
            case 2:

                Application.ExternalCall("AudioPlay", "Jump", 1, false);
               // AudioController.Instance.PlayAudio(1, 5, false);

                ChangeSprite(waterSpriteController[0], "StoryWater_1");
                ChangeSprite(waterSpriteController[1], "StoryWater_2");

                ChangeSprite(BackgroundSpriteContoller, "StoryBackground_B");
                ChangeSprite(TextSpriteController, "StoryText_2");

                waterSpriteController[0].FadeOut(4f);
                waterSpriteController[1].FadeIn(3);
               // TextSpriteController.FadeIn(1f);
                break;
            case 3:
                Application.ExternalCall("AudioPlay", "Jump", 1,false);
                // AudioController.Instance.PlayAudio(1, 5, false);

                ChangeSprite(waterSpriteController[0], "StoryWater_2");
                ChangeSprite(waterSpriteController[1], "StoryWater_3");

                //  AudioController.Instance.AudioFadeOut_BGM(0.1f);
                ChangeSprite(BackgroundSpriteContoller, "StoryBackground_P");
                ChangeSprite(TextSpriteController, "StoryText_3");

                waterSpriteController[0].FadeOut(3f);
                waterSpriteController[1].FadeIn(3);
              //  TextSpriteController.FadeIn(1f);
                break;
            case 4:
                Application.ExternalCall("AudioPlay", "Boss", 1, false);
               // AudioController.Instance.PlayAudio(1, 2, false);

                BorderPanel.SetActive(false);
                skipButtom.SetActive(false);
                TextPanel.SetActive(false);
                waterPanel[0].SetActive(false);
                waterPanel[1].SetActive(false);

                SpecialThanksButton.SetActive(false);

                ChangeSprite(BackgroundSpriteContoller, "Story_5");
                break;
            case 5:
                Application.ExternalCall("AudioFadeOut");
                Application.ExternalCall("AudioPlay", "Dead", 1, false);
                //AudioController.Instance.PlayAudio(2, 3, false);              


                TextPanel.SetActive(true);
                waterPanel[1].SetActive(true);
             //   AudioController.Instance.PlayAudio(1, 5, false);

                ChangeSprite(BackgroundSpriteContoller, "StoryBackground_P");
                ChangeSprite(waterSpriteController[1], "StoryWater_4");
                ChangeSprite(TextSpriteController, "StoryText_4");

                waterSpriteController[1].FadeIn(5);

                break;
            case 6:
                Application.ExternalCall("AudioPlay", "Jump", 1, false);
               // AudioController.Instance.PlayAudio(1, 5, false);
               
                BorderPanel.SetActive(false);
                skipButtom.SetActive(false);
                TextPanel.SetActive(false);
                waterPanel[0].SetActive(false);
                waterPanel[1].SetActive(false);

                SpecialThanksButton.SetActive(false);


                ChangeSprite(BackgroundSpriteContoller, "NoviceTeaching");
                break;
         
            default:
                CloseAllButton();
                StoryPanel.SetActive(false);
                uiPanel.SetActive(false);
                break;

        }

    }

    public void ChangeSprite(SpriteController changedObject, string spriteName)
    {
        changedObject.GetSprite(spriteName);

    }

    public void CloseAllButton()
    {
        AgainBottun.SetActive(false);
        PayForPalyButton.SetActive(false);
        FreeButon.SetActive(false);
        QuitButton.SetActive(false);
        ReceiveButton.SetActive(false);
        skipButtom.SetActive(false);
        SpecialThanksButton.SetActive(false);
        NextStoryButtom.SetActive(false);

    }

    public void MessageSelect(int select)//select = 0 FreeTimes; select = 1 PayForPlay; select = 2 Score
    {
        switch (select)
        {
            case 0:
                FreeTimesNumber.SetActive(true);
                PayForPlayNumber.SetActive(false);
                ResultScore.SetActive(false);
                CloseAllButton();
                FreeButon.SetActive(true);
                                              
                ChangeSprite(MessagePanelSpriteController, "StartPanel_Free");
                break;

            case 1:
                PayForPlayNumber.SetActive(true);
                FreeTimesNumber.SetActive(false);
                ResultScore.SetActive(false);
                CloseAllButton();
                PayForPalyButton.SetActive(true);

                ChangeSprite(MessagePanelSpriteController, "StartPanel");

                break;

            case 2:
                ResultScore.SetActive(true);
                FreeTimesNumber.SetActive(false);
                PayForPlayNumber.SetActive(false);
                CloseAllButton();
                QuitButton.SetActive(true);
                ResultScoreText.SetNumber(StageDataController.Instance.TotalScore);
                ResultScoreText.DeleteZero(StageDataController.Instance.TotalScore);
                ResultScoreText.ChangeColor(4);
                ChangeSprite(MessagePanelSpriteController, "ResultPanel");

                break;
        }
        
    }

    public void SetPresetFreeCount(int presetFreeCount)
    {
        PresetFreeCount.SetNumber(presetFreeCount);
        PresetFreeCount.DeleteZero(presetFreeCount);
        PresetFreeCount.ChangeColor(4); 
    }

    public void SetPlayerFreeCount(int playerFreeCount)
    {
        PlayerFreeCount.SetNumber(playerFreeCount);
        PlayerFreeCount.DeleteZero(playerFreeCount);
        PlayerFreeCount.ChangeColor(4);
    }

    public void SetConsumePoint(int ConsumeOncePoint)
    {
        ShowConsumePoint.SetNumber(ConsumeOncePoint);
        ShowConsumePoint.DeleteZero(ConsumeOncePoint);
        ShowConsumePoint.ChangeColor(4);

        ConsumePoint.SetNumber(ConsumeOncePoint);
        ConsumePoint.DeleteZero(ConsumeOncePoint);
        ConsumePoint.ChangeColor(4);

    }

    public void SetCustomerPoint(int playerPoint)
    {     
        CustomerPoint.SetNumber(playerPoint);
        CustomerPoint.DeleteZero(playerPoint);
        CustomerPoint.ChangeColor(4);
    }
    
    public void OpenRestartButton()
    {
        AgainBottun.SetActive(true);
    }

    public void OpenReceivebutton()
    {
        ReceiveButton.SetActive(true);
    }

    public void OpenPlayUI()
    {
        PlayUI.SetActive(true);
        TimesUPSpriteController.ResetAlpha(0);
        TimesUP.SetActive(false);
    }

    public void ClosePlayUI()
    {
        PlayUI.SetActive(false);
    }

    public void OpengTimesUP()
    {
        TimesUP.SetActive(true);
        TimesUPSpriteController.FadeIn(4);
    }

   
    public void Timer(int minute,int second)
    {
        MinuteText.SetNumber(minute);
        SecondText.SetNumber(second);
    }

    public void Score(int score)
    {
        ScoreText.SetNumber(score);
    }

    public void ChangePlayTextColor(int color)
    {
        timeText.GetSprite("Time_" + color);
        scoreText.GetSprite("Score_" + color);
        colonText.GetSprite("colon_" + color);
        PlusSpriteController.GetSprite("Plus_" + color);
        plusScoreText.ChangeColor(color);

        MinuteText.ChangeColor(color);
        SecondText.ChangeColor(color);
        ScoreText.ChangeColor(color);
    }

    #region 按鈕的事件們

    public void ConsumePlayerPoint()
    {
        Application.ExternalCall("AudioPlay", "Jump", 1, false);
        Application.ExternalCall("ConsumePlayerPoint");

    }

    public void ConsumeFreeCount()
    {
        Application.ExternalCall("AudioPlay", "Jump", 1, false);
        Application.ExternalCall("ConsumeFreeCount");

    }

    public void UIStartStory()
    {
        StartCoroutine(startStory());
    }

    IEnumerator startStory()
    {
        yield return waitForStory;
        StoryPanel.SetActive(true);

        GameManager.Instance.StartStroy();
    }

    public void NextStory()
    {
        GameManager.Instance.NextStory();
       
    }

    public void ReStartGame()
    {
        Application.ExternalCall("AudioPause");
        Application.ExternalCall("AudioPlay", "Jump", 1, false);
      //  AudioController.Instance.PlayAudio(1, 5, false);

        GameManager.Instance.ResetGame();
        Application.ExternalCall("JudgeFreeOrConsumePoint");

    }

    public void SkipStory()
    {
        GameManager.Instance.GoToNoviceTeaching();
        
    }
    
    public void QuitGame()
    {
        Application.ExternalCall("AudioPlay", "Jump", 1, false);
       // AudioController.Instance.PlayAudio(1, 5, false);

        Application.ExternalCall("Quit");
    }


    public void ReceiveGive()
    {
          Application.ExternalCall("AudioPlay", "Jump", 1, false);
       // AudioController.Instance.PlayAudio(1, 5, false);


        Application.ExternalCall("Receive");

    }

    public void SetSpecialThanksPanel()
    {
        Application.ExternalCall("AudioPlay", "Jump", 1, false);

        if (SpecialThanksPanel.activeSelf)
        {
            SpecialThanksPanel.SetActive(false);
            StoryPanel.SetActive(true);
            skipButtom.SetActive(true);
            NextStoryButtom.SetActive(true);

        }
        else
        {
            SpecialThanksPanel.SetActive(true);
            StoryPanel.SetActive(false);
            skipButtom.SetActive(false);
            NextStoryButtom.SetActive(false);
        }

    }


    #endregion

    public void ResetText()
    {
        ScoreText.SetNumber(StageDataController.Instance.TotalScore);
        ScoreText.DeleteZero(StageDataController.Instance.TotalScore);
        
    }

    public void AddScore(int score)
    {
        plusScoreText.SetNumber(score);
        plusScoreText.DeleteZero(score);
        ScoreText.SetNumber(StageDataController.Instance.TotalScore);
        ScoreText.DeleteZero(StageDataController.Instance.TotalScore);
        ScorePlusFadeOut();

    }

    public void ScorePlusFadeOut()
    {
        PlusSpriteController.FadeOut(AddScoreFadeOutSpeed);
        plusScoreText.FadeOut(AddScoreFadeOutSpeed);
        StartCoroutine(scrollScore());
    }

    IEnumerator scrollScore()
    {
        plusScoreScrollDis.x = plusScoreTransform.anchoredPosition.x;
        plusScoreScrollDis.y = plusScoreTransform.anchoredPosition.y;

        plusScoreNext_Y = plusScoreTransform.anchoredPosition.y + ScrollDis;
        while (plusScoreTransform.anchoredPosition.y < plusScoreNext_Y)
        {
            plusScoreScrollDis.y += PlusScoreScrollSpeed * Time.deltaTime;
            plusScoreTransform.anchoredPosition = plusScoreScrollDis;

            yield return null;
 
        }
        ResetPlusScore();
    }

    public void ResetPlusScore()
    {
        plusScoreScrollDis.y = plusScoreScrollDis.y - ScrollDis;
        plusScoreTransform.anchoredPosition = plusScoreScrollDis;

    }

   
}
