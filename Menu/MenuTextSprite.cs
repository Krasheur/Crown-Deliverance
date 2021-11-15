using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuTextSprite : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Button_Choice;
   
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = Button_Choice;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Button_Choice == "Play")
        {
            GetComponent<TextMeshProUGUI>().text = "<sprite=0>Play <sprite=0>";
        }
        else if (Button_Choice == "Options" || Button_Choice == "Option")
        {
            GetComponent<TextMeshProUGUI>().text = "<sprite=1>Options <sprite=1>";
        }
        else if (Button_Choice == "Quit")
        {
            GetComponent<TextMeshProUGUI>().text = "<sprite=2>Quit <sprite=2>";
        }
        else if (Button_Choice == "New Save")
        {
            GetComponent<TextMeshProUGUI>().text = "<sprite=2>New Save <sprite=2>";
        }
        else if (Button_Choice == "Back")
        {
            GetComponent<TextMeshProUGUI>().text = "<sprite=1>Back <sprite=1>";
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = "<sprite=1>" + Button_Choice + " <sprite=1>";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Button_Choice == "Play")
        {
            GetComponent<TextMeshProUGUI>().text = "Play";
        }
        else if (Button_Choice == "Options" || Button_Choice == "Option")
        {
            GetComponent<TextMeshProUGUI>().text = "Options";
        }
        else if (Button_Choice == "Quit")
        {
            GetComponent<TextMeshProUGUI>().text = "Quit";
        }
        else if (Button_Choice == "Back")
        {
            GetComponent<TextMeshProUGUI>().text = "Back";
        }
        else
        {
            GetComponent<TextMeshProUGUI>().text = Button_Choice;
        }
    }
}
