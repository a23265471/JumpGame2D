using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [Header ("版面")]
    public Panel[] panel;

    [Header("圖片")]
    public Image[] image;

    [Header("數字")]
    public Number[] number;

    public VerticalLayoutGroup verticalLayoutGroup;


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
