using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackInput : MonoBehaviour
{
    protected int maxValue;
    protected int currentValue;

    [SerializeField] GameObject inventory;
    
    public int MaxValue { get => maxValue; set => maxValue = value; }
    public int CurrentValue { get => currentValue; set => currentValue = value; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    
    public void Plus()
    {
        if(currentValue < maxValue)
        {
            AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
            currentValue++;
            transform.GetChild(3).GetComponentInChildren<Text>().text = currentValue.ToString();
        }
    }

    public void Minus()
    {
        if (currentValue > 1)
        {
            AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
            currentValue--;
            transform.GetChild(3).GetComponentInChildren<Text>().text = currentValue.ToString();
        }
    }

    public void Validate()
    {
        if (inventory.GetComponent<InventoryManager>() != null)
        {
            inventory.GetComponent<InventoryManager>().NbStackValidated = currentValue; 
            inventory.GetComponent<InventoryManager>().SelectionDone();
        }
        else if (inventory.GetComponent<MonoInventoryManager>() != null)
        {
            inventory.GetComponent<MonoInventoryManager>().NbStackValidated = currentValue;
            inventory.GetComponent<MonoInventoryManager>().SelectionDone();
        }
        else if(inventory.GetComponent<TraderInventoryManager>() != null)
        {
            inventory.GetComponent<TraderInventoryManager>().NbStackValidated = currentValue;
            inventory.GetComponent<TraderInventoryManager>().SelectionDone();
        }
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
