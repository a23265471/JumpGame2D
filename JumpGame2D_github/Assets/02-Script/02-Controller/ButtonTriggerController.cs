using UnityEngine.UI;
using UnityEngine;


public class ButtonTriggerController : MonoBehaviour
{
    private Button button;
    public System.Action OnClickEven;
   // public OnClickEven[] onClickEven;
    private void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnClickEven();

    }

}
