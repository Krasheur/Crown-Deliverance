using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonoInventoryManager : MonoBehaviour
{
    [SerializeField] GameObject ItemOnMouse;
    [SerializeField] GameObject StackInput;

    protected Pickable current_item;
    protected int current_item_nb;
    protected int current_item_index;

    protected int nbStackValidated;
    private bool selectionInProgress;

    public int NbStackValidated { get => nbStackValidated; set => nbStackValidated = value; }
    public bool SelectionInProgress { get => selectionInProgress; }
    public Pickable Current_item { get => current_item; set => current_item = value; }
    public int Current_item_nb { get => current_item_nb; set => current_item_nb = value; }
    public int Current_item_index { get => current_item_index; set => current_item_index = value; }

    // Start is called before the first frame update
    void Start()
    {
        selectionInProgress = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < GetComponent<Transform>().childCount; i++)
        {            
            if(PlayerManager.main.FocusedCharacter.Inventory[i].Item != ItemInventory.emptyPickable)
            {
                GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite = PlayerManager.main.FocusedCharacter.Inventory[i].Item.Img;
                GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().text = PlayerManager.main.FocusedCharacter.Inventory[i].Nb.ToString();
            }
            else
            {
                GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            }   
            
            if (PlayerManager.main.FocusedCharacter.Inventory[i].Nb > 1)
            {
                GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = true;
                GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = true;
            }
            else
            {
                GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = false;
                GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = false;
            }
        }
    }

    public void TakeObjectInCharacterInventory(int index)
    {
        if (!SelectionInProgress)
        {
            if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.FocusedCharacter.Inventory[index].Nb == 1)
            {
                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 1;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.FocusedCharacter.Inventory[index].Item.Img;               
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;
                
                current_item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;
                Current_item_nb = PlayerManager.main.FocusedCharacter.Inventory[index].Nb;
                Current_item_index = index;

                PlayerManager.main.FocusedCharacter.Inventory[index].Item = ItemInventory.emptyPickable;                            
                PlayerManager.main.FocusedCharacter.Inventory[index].Nb = 0;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.FocusedCharacter.Inventory[index].Nb > 1)
            {
                StackInput.GetComponent<CanvasGroup>().alpha = 1;
                StackInput.GetComponent<CanvasGroup>().interactable = true;
                StackInput.GetComponent<CanvasGroup>().blocksRaycasts = true;
                StackInput.transform.GetChild(3).GetComponentInChildren<Text>().text = PlayerManager.main.FocusedCharacter.Inventory[index].Nb.ToString();
                StackInput.GetComponent<StackInput>().MaxValue = PlayerManager.main.FocusedCharacter.Inventory[index].Nb;
                StackInput.GetComponent<StackInput>().CurrentValue = PlayerManager.main.FocusedCharacter.Inventory[index].Nb;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.FocusedCharacter.Inventory[index].Item.Img;
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;
                selectionInProgress = true;
                current_item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;                
                Current_item_index = index;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
            {
                if (PlayerManager.main.FocusedCharacter.Inventory[index].Item != null
                    && PlayerManager.main.FocusedCharacter.Inventory[index].Item as Equipable == null
                    && PlayerManager.main.FocusedCharacter.Inventory[index].Item.name == Current_item.name)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.FocusedCharacter.Inventory[index].Nb += Current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                    ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                }
                else if (PlayerManager.main.FocusedCharacter.Inventory[index].Item != Current_item && PlayerManager.main.FocusedCharacter.Inventory[index].Nb != 0)
                {                    
                    if (!(PlayerManager.main.FocusedCharacter.Inventory[Current_item_index].Item.Img == Current_item.Img
                          && PlayerManager.main.FocusedCharacter.Inventory[Current_item_index].Nb > 0))
                    {
                        AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                        PlayerManager.main.FocusedCharacter.Inventory[Current_item_index].Nb = PlayerManager.main.FocusedCharacter.Inventory[index].Nb;
                        PlayerManager.main.FocusedCharacter.Inventory[Current_item_index].Item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;
                        PlayerManager.main.FocusedCharacter.Inventory[index].Nb = Current_item_nb;
                        PlayerManager.main.FocusedCharacter.Inventory[index].Item = Current_item;
                        ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                        ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                        ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                    }                          
                    
                }
                else if (PlayerManager.main.FocusedCharacter.Inventory[index].Nb == 0)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.FocusedCharacter.Inventory[index].Item = Current_item;
                    PlayerManager.main.FocusedCharacter.Inventory[index].Nb = Current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                    ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                }
            }
        }
    }

    public void SelectionDone()
    {
        selectionInProgress = false;
        ItemOnMouse.GetComponent<CanvasGroup>().alpha = 1;

        if (nbStackValidated == StackInput.GetComponent<StackInput>().MaxValue)
        {            
            PlayerManager.main.FocusedCharacter.Inventory[Current_item_index].Item = ItemInventory.emptyPickable;
            PlayerManager.main.FocusedCharacter.Inventory[Current_item_index].Nb = 0;     
                       
        }
        else
        {            
            PlayerManager.main.FocusedCharacter.Inventory[Current_item_index].Nb -= nbStackValidated;
        }
        Current_item_nb = nbStackValidated;
    }    
}
