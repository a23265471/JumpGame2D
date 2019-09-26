using UnityEngine;

public class NumberController : MonoBehaviour
{
    private SpriteController[] number;
    private Transform m_transform;

    private int thousand;
    private int hundred;
    private int ten;
    private int one;

    int currentNumber;
    int fixInput;
    int currentColor;

    int i;
    int j;
    
    private void Awake()
    {
        number = new SpriteController[gameObject.transform.childCount];
        m_transform = gameObject.GetComponent<Transform>();

        for (i = 0; i < m_transform.childCount; i++)
        {
            j = m_transform.childCount - (1 + i);

            number[j] = m_transform.GetChild(i).gameObject.GetComponent<SpriteController>();
        }
        currentColor = 0;
    }
    
    public void SetNumber(int InputValue)
    {
        for (i = 0; i < number.Length; i++)
        {
            j = number.Length - (1 + i);

            if (!number[j].isActiveAndEnabled)
            {
                number[j].VisibleUIImage();
            }
            else
            {
                break;
            }

        }

        fixInput = InputValue - ((InputValue / (int)Mathf.Pow(10, number.Length)) * (int)Mathf.Pow(10, number.Length));
    

        for (i = 0; i < number.Length; i++)
        {
        
            currentNumber = (fixInput / (int)Mathf.Pow(10, i)) - ((fixInput / (int)Mathf.Pow(10, i + 1)) * 10);
            
            number[i].GetSprite("Number_" + currentColor + currentNumber.ToString());
            
        }
       
    }

    public void DeleteZero(int InputValue)
    {
        fixInput = InputValue - ((InputValue / (int)Mathf.Pow(10, number.Length)) * (int)Mathf.Pow(10, number.Length));
        for (i = 0; i < number.Length; i++)
        {
            currentNumber = fixInput / ((int)Mathf.Pow(10, (number.Length - 1)) / (int)Mathf.Pow(10, i));
            if (currentNumber == 0)
            {
                if (number.Length - (1 + i) != 0)
                {
                    number[number.Length - (1 + i)].InvisibleUIImage();
                }
            }
            else
            {
                break;
            }
        }

    }

    public void ChangeColor(int color)
    {
        currentColor = color;
        for (i = 0; i < number.Length; i++)
        {
            currentNumber = (fixInput / (int)Mathf.Pow(10, i)) - ((fixInput / (int)Mathf.Pow(10, i + 1)) * 10);

            number[i].GetSprite("Number_" + currentColor + currentNumber.ToString());
        }

    }

    public void FadeOut(float FadeOutSpeed)
    {
        for (i = 0; i < number.Length; i++)
        {

            if (m_transform.GetChild(m_transform.childCount-1-i).gameObject.activeInHierarchy)
            {

                  number[i].ResetAlpha(1);

                  number[i].FadeOut(FadeOutSpeed);
            }

        }
    }

    public void ResetAlpha()
    {
        for (i = 0; i < number.Length; i++)
        {
            number[i].ResetAlpha(1);
        }

    }
}
