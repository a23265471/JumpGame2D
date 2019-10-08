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

    public Dictionary<int, CanvasGroup> PanelCollection;
    public Dictionary<int, SpriteController> ImageCollection;
    public Dictionary<int, NumberController> NumberCollection;
    public Dictionary<int, PanelObject> MenuCollection;

    int temporaryInt;
    int temporaryInt2;

    int panelCollectionLenght;
    int ImageCollectionLenght;
    int NumberCollectionLenght;

    private void Awake()
    {
        instance = this;
    }

    public void CreatDictionary(GameObject[] uIBehaviour)
    {
        UICanvas = new UIBehaviour[uIBehaviour.Length];

        for(temporaryInt=0; temporaryInt< UICanvas.Length; temporaryInt++)
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

        for (temporaryInt=0;temporaryInt< UICanvas.Length; temporaryInt++)
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
    
    public void SetObjectActive(int ID, int alpha, bool interatable, bool blockRaycast)
    {
        PanelCollection[ID].alpha = alpha;
        PanelCollection[ID].interactable = interatable;
        PanelCollection[ID].blocksRaycasts = blockRaycast;
    }

    public void CloseAllPanel()
    {
     
    }

    public void OpenMenu(int ID)
    {
        for(temporaryInt=0;temporaryInt< MenuCollection[ID].ActiveObjectID.Length; temporaryInt++)
        {
            SetObjectActive(MenuCollection[ID].ActiveObjectID[temporaryInt], 1, true, true);
        }
        
        for(temporaryInt=0;temporaryInt< MenuCollection[ID].Image.Length; temporaryInt++)
        {
            ImageCollection[MenuCollection[ID].Image[temporaryInt].ID].GetSprite(MenuCollection[ID].Image[temporaryInt].ImageName.ToString());
        }
 
        for(temporaryInt=0;temporaryInt< MenuCollection[ID].Number.Length; temporaryInt++)
        {
            switch (MenuCollection[ID].Number[temporaryInt].DeletZore)
            {
                case true:
                    NumberCollection[MenuCollection[ID].Number[temporaryInt].ID].SetNumber(MenuCollection[ID].Number[temporaryInt].Value);
                    NumberCollection[MenuCollection[ID].Number[temporaryInt].ID].DeleteZero(MenuCollection[ID].Number[temporaryInt].Value);
                    break;

                case false:
                    NumberCollection[MenuCollection[ID].Number[temporaryInt].ID].SetNumber(MenuCollection[ID].Number[temporaryInt].Value);
                    break;

            }

        }
    }

    public void SetNumber(int menuID, int numberID, int value)
    {
        NumberCollection[MenuCollection[menuID].Number[numberID].ID].SetNumber(value);
    }

    public void SetImage(int menuID, int imageID, string ImageName)
    {
   //     MenuCollection[menuID].Image[imageID].ID

    }


}
