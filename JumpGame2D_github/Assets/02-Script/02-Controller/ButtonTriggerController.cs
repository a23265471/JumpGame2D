using UnityEngine.UI;
using UnityEngine;


public class ButtonTriggerController : MonoBehaviour
{
    private Button button;
    public System.Action[] OnClickEven;
   // public OnClickEven[] onClickEven;

    int i;

    private void Start()
    {
      //  onClickEven = new OnClickEven();
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        for (i = 0; i < onClickEven.Length; i++) 
        {
            onClickEven
        }
    }

}
