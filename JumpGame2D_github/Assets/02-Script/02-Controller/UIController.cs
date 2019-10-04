using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour                                                                      
{
    public UIBehaviour[] UICanvas;
   // public CanvasGroup sss;

    public Dictionary<string, CanvasGroup> PanelCollection;
    public Dictionary<string, SpriteController> ImageCollection;
    public Dictionary<string, NumberController> NumberCollection;

    int temporaryInt;
    int temporaryInt2;

    int panelCollectionLenght;
    int ImageCollectionLenght;
    int NumberCollectionLenght;

    public void Awake()
    {
        for (temporaryInt = 0; temporaryInt < UICanvas.Length; temporaryInt++)
        {
            panelCollectionLenght += UICanvas[temporaryInt].panel.Length;
            ImageCollectionLenght += UICanvas[temporaryInt].image.Length;
            NumberCollectionLenght += UICanvas[temporaryInt].number.Length;
        }
        
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
        
    }

    public void SetImage(SpriteController spriteController, string spriteName)
    {
        spriteController.GetSprite(spriteName);
    }

    public void SetNumber(NumberController numberController, int number)
    {
        numberController.SetNumber(number);

    }

    public void SetPanelActive(CanvasGroup canvasGroup, int active, bool interatable, bool blockRaycast)
    {
        canvasGroup.alpha = active;
        canvasGroup.interactable = interatable;
        canvasGroup.blocksRaycasts = blockRaycast;
    }


}
