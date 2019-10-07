using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour                                                                      
{
    public static UIController instance;
    private UIBehaviour[] UICanvas;

    public Dictionary<int, CanvasGroup> PanelCollection;
    public Dictionary<int, SpriteController> ImageCollection;
    public Dictionary<int, NumberController> NumberCollection;

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

        for(temporaryInt=0;temporaryInt< UICanvas.Length; temporaryInt++)
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

    }
    
    public void SetImage(SpriteController spriteController, string spriteName)
    {
        spriteController.GetSprite(spriteName);
    }

    public void SetNumber(NumberController numberController, int number)
    {
        numberController.SetNumber(number);

    }

    public void SetObjectActive(int ID, int alpha, bool interatable, bool blockRaycast)
    {
        PanelCollection[ID].alpha = alpha;
        PanelCollection[ID].interactable = interatable;
        PanelCollection[ID].blocksRaycasts = blockRaycast;
    }

    public void OpenPanel(int ID)
    {
        SetObjectActive(ID, 1, true, true);


    }

    public void OpenButton(int ID)
    {
        SetObjectActive(ID, 1, true, true);

    }

    public void CloseAllButton()
    {
        for (temporaryInt = 11; temporaryInt < PanelCollection.Count; temporaryInt++)
        {
        }
    }
}
