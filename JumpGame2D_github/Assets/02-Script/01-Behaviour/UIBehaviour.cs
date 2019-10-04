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
        public string ID;
        public CanvasGroup Active;
    }

    [System.Serializable]
    public struct Image
    {
        public string ID;
        public SpriteController spriteController;
    }

    [System.Serializable]
    public struct Number
    {
        public string ID;
        public NumberController numberController;
    }

    private void Awake()
    {
        panel = null;
    }

    public void sss()
    {

        Debug.Log("j");
    }
}
