using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    public Panel[] panel;

    public Image[] image;

    public Number[] number;

    [System.Serializable]
    public struct Panel
    {
        public string Name;
        public int ID;
        public CanvasGroup Active;
    }

    [System.Serializable]
    public struct Image
    {
        public string Name;
        public int ID;
        public SpriteController spriteController;
    }

    [System.Serializable]
    public struct Number
    {
        public string Name;
        public int ID;
        public NumberController numberController;
    }


}
