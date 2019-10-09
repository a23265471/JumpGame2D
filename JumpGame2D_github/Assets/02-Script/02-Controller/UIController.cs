using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public PanelObject[] Menu;

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

    private WaitForSeconds waitSecond;

    int temporaryInt;
    int temporaryInt2;

    int panelCollectionLenght;
    int ImageCollectionLenght;
    int NumberCollectionLenght;

    private void Awake()
    {
        instance = this;
        waitSecond = new WaitForSeconds(0.7f);
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
        }

        PanelCollection = new Dictionary<int, CanvasGroup>(panelCollectionLenght);
        ImageCollection = new Dictionary<int, SpriteController>(ImageCollectionLenght);
        NumberCollection = new Dictionary<int, NumberController>(NumberCollectionLenght);
        MenuCollection = new Dictionary<int, PanelObject>(Menu.Length);

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

        }

        for (temporaryInt = 0; temporaryInt < Menu.Length; temporaryInt++)
        {
            MenuCollection[Menu[temporaryInt].ID] = Menu[temporaryInt];
        }

    }

    #region 物件的設置
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
    #endregion

    #region 按鈕事件們

    public void ClickSound()
    {
        Application.ExternalCall("AudioPlay", "Jump", 1, false);
    }

    public void ConsumePoint()
    {
        Application.ExternalCall("ConsumePlayerPoint");
        SetObjectActive(12, 1, false, false);

        UIStartStory();//*******************************************************************待刪
    }

    public void ConsumeFreeTimes()
    {
        Application.ExternalCall("ConsumePlayerPoint");
        SetObjectActive(11, 1, false, false);

        UIStartStory();//*******************************************************************待刪
    }

    #endregion

    public void UIStartStory()//****************************在GameManager裡被Javascript呼叫
    {
        StartCoroutine(WaitSecond());

    }

    IEnumerator WaitSecond()
    {
        yield return waitSecond;
        OpenMenu(1);

    }

    public void Story(int stroyPage)
    {
        switch (stroyPage)
        {
            case 1:


                break;
        }

    }
}
