using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Image sprite_J1;
    [SerializeField] Image sprite_J2;
    [SerializeField] Image sprite_J3;

    [SerializeField] Text gold_J1;
    [SerializeField] Text gold_J2;
    [SerializeField] Text gold_J3;

    [SerializeField] GameObject panel_J1;
    [SerializeField] GameObject panel_J2;
    [SerializeField] GameObject panel_J3;

    [SerializeField] GameObject ItemOnMouse;
    [SerializeField] GameObject StackInput;

    protected Pickable current_item;
    protected int current_item_nb;

    protected int current_item_player;
    protected int current_item_index;
    
    protected int nbStackValidated;

    private bool selectionInProgress;

    [SerializeField] VisuPortraitsPermHUD visuPortraitHUD;    
    public int NbStackValidated { get => nbStackValidated; set => nbStackValidated = value; }
    public bool SelectionInProgress { get => selectionInProgress; }

    // Start is called before the first frame update
    void Start()
    {
        selectionInProgress = false;
    }

    // Update is called once per frame
    void Update()
    {        
        if (GetComponent<CanvasGroup>().alpha > 0 && !SelectionInProgress)
        {
            if (sprite_J1.sprite != PlayerManager.main.PlayerFairy.Portrait
                || sprite_J2.sprite != PlayerManager.main.PlayerRogue.Portrait
                || sprite_J3.sprite != PlayerManager.main.PlayerTank.Portrait)
            {
                ActualizePortrait();
            }

            for (int i = 0; i < panel_J1.GetComponent<Transform>().childCount; i++)
            {
                if (PlayerManager.main.PlayerFairy.Inventory[i].Item != ItemInventory.emptyPickable)
                {
                    panel_J1.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    panel_J1.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite = PlayerManager.main.PlayerFairy.Inventory[i].Item.Img;
                    panel_J1.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().text = PlayerManager.main.PlayerFairy.Inventory[i].Nb.ToString();
                }
                else
                {
                    panel_J1.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                }

                if (PlayerManager.main.PlayerFairy.Inventory[i].Nb > 1)
                {
                    panel_J1.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = true;
                    panel_J1.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = true;
                }
                else
                {
                    panel_J1.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = false;
                    panel_J1.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = false;
                }
            }

            for (int i = 0; i < panel_J2.GetComponent<Transform>().childCount; i++)
            {
                if (PlayerManager.main.PlayerRogue.Inventory[i].Item != ItemInventory.emptyPickable)
                {
                    panel_J2.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    panel_J2.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite = PlayerManager.main.PlayerRogue.Inventory[i].Item.Img;
                    panel_J2.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().text = PlayerManager.main.PlayerRogue.Inventory[i].Nb.ToString();
                }
                else
                {
                    panel_J2.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                }

                if (PlayerManager.main.PlayerRogue.Inventory[i].Nb > 1)
                {
                    panel_J2.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = true;
                    panel_J2.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = true;
                }
                else
                {
                    panel_J2.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = false;
                    panel_J2.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = false;
                }
            }

            for (int i = 0; i < panel_J3.GetComponent<Transform>().childCount; i++)
            {
                if (PlayerManager.main.PlayerTank.Inventory[i].Item != ItemInventory.emptyPickable)
                {
                    panel_J3.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    panel_J3.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite = PlayerManager.main.PlayerTank.Inventory[i].Item.Img;
                    panel_J3.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().text = PlayerManager.main.PlayerTank.Inventory[i].Nb.ToString();
                }
                else
                {
                    panel_J3.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                }

                if (PlayerManager.main.PlayerTank.Inventory[i].Nb > 1)
                {
                    panel_J3.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = true;
                    panel_J3.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = true;
                }
                else
                {
                    panel_J3.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = false;
                    panel_J3.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = false;
                }
            }
        }

        UpdateGold();
    }

    void ActualizePortrait()
    {
        sprite_J1.sprite = PlayerManager.main.PlayerFairy.Portrait;
        sprite_J2.sprite = PlayerManager.main.PlayerRogue.Portrait;
        sprite_J3.sprite = PlayerManager.main.PlayerTank.Portrait;
    }

    void UpdateGold()
    {
        gold_J1.text = PlayerManager.main.PlayerFairy.Gold.ToString();
        gold_J2.text = PlayerManager.main.PlayerRogue.Gold.ToString();
        gold_J3.text = PlayerManager.main.PlayerTank.Gold.ToString();           
    }

    public void TakeObjectInFairyInventory(int index)
    {   
        if (!SelectionInProgress)
        {
            if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.PlayerFairy.Inventory[index].Nb == 1)
            {
                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 1;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.PlayerFairy.Inventory[index].Item.Img;
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.PlayerFairy.Inventory[index].Item;

                current_item = PlayerManager.main.PlayerFairy.Inventory[index].Item;
                current_item_nb = PlayerManager.main.PlayerFairy.Inventory[index].Nb;
                current_item_player = 1;
                current_item_index = index;

                PlayerManager.main.PlayerFairy.Inventory[index].Item = ItemInventory.emptyPickable;
                PlayerManager.main.PlayerFairy.Inventory[index].Nb = 0;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.PlayerFairy.Inventory[index].Nb > 1)
            {
                StackInput.GetComponent<CanvasGroup>().alpha = 1;
                StackInput.GetComponent<CanvasGroup>().interactable = true;
                StackInput.GetComponent<CanvasGroup>().blocksRaycasts = true;
                StackInput.transform.GetChild(3).GetComponentInChildren<Text>().text = PlayerManager.main.PlayerFairy.Inventory[index].Nb.ToString();
                StackInput.GetComponent<StackInput>().MaxValue = PlayerManager.main.PlayerFairy.Inventory[index].Nb;
                StackInput.GetComponent<StackInput>().CurrentValue = PlayerManager.main.PlayerFairy.Inventory[index].Nb;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.PlayerFairy.Inventory[index].Item.Img;
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.PlayerFairy.Inventory[index].Item;
                selectionInProgress = true;
                current_item = PlayerManager.main.PlayerFairy.Inventory[index].Item;                
                current_item_player = 1;
                current_item_index = index;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
            {              
                if (   PlayerManager.main.PlayerFairy.Inventory[index].Item != null
                    && PlayerManager.main.PlayerFairy.Inventory[index].Item as Equipable == null
                    && PlayerManager.main.PlayerFairy.Inventory[index].Item.name == current_item.name)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.PlayerFairy.Inventory[index].Nb += current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                    ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                }
                else if (PlayerManager.main.PlayerFairy.Inventory[index].Item != current_item && PlayerManager.main.PlayerFairy.Inventory[index].Nb != 0)
                {
                        switch (current_item_player)
                        {
                            case 1:
                            if (!(PlayerManager.main.PlayerFairy.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb = PlayerManager.main.PlayerFairy.Inventory[index].Nb;
                                PlayerManager.main.PlayerFairy.Inventory[current_item_index].Item = PlayerManager.main.PlayerFairy.Inventory[index].Item;
                                PlayerManager.main.PlayerFairy.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerFairy.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            case 2:
                            if (!(PlayerManager.main.PlayerFairy.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerRogue.Inventory[current_item_index].Nb = PlayerManager.main.PlayerFairy.Inventory[index].Nb;
                                PlayerManager.main.PlayerRogue.Inventory[current_item_index].Item = PlayerManager.main.PlayerFairy.Inventory[index].Item;
                                PlayerManager.main.PlayerFairy.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerFairy.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            case 3:
                            if (!(PlayerManager.main.PlayerFairy.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerTank.Inventory[current_item_index].Nb = PlayerManager.main.PlayerFairy.Inventory[index].Nb;
                                PlayerManager.main.PlayerTank.Inventory[current_item_index].Item = PlayerManager.main.PlayerFairy.Inventory[index].Item;
                                PlayerManager.main.PlayerFairy.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerFairy.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            default: break;
                        }                    
                }
                else if (PlayerManager.main.PlayerFairy.Inventory[index].Nb == 0)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.PlayerFairy.Inventory[index].Item = current_item;
                    PlayerManager.main.PlayerFairy.Inventory[index].Nb = current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                    ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                }
            }
        }
    }

    public void TakeObjectInRogueInventory(int index)
    {
        if (!SelectionInProgress)
        {
            if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.PlayerRogue.Inventory[index].Nb == 1)
            {
                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 1;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.PlayerRogue.Inventory[index].Item.Img;
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.PlayerRogue.Inventory[index].Item;

                current_item = PlayerManager.main.PlayerRogue.Inventory[index].Item;
                current_item_nb = PlayerManager.main.PlayerRogue.Inventory[index].Nb;
                current_item_player = 2;
                current_item_index = index;

                PlayerManager.main.PlayerRogue.Inventory[index].Item = ItemInventory.emptyPickable;
                PlayerManager.main.PlayerRogue.Inventory[index].Nb = 0;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.PlayerRogue.Inventory[index].Nb > 1)
            {
                StackInput.GetComponent<CanvasGroup>().alpha = 1;
                StackInput.GetComponent<CanvasGroup>().interactable = true;
                StackInput.GetComponent<CanvasGroup>().blocksRaycasts = true;
                StackInput.transform.GetChild(3).GetComponentInChildren<Text>().text = PlayerManager.main.PlayerRogue.Inventory[index].Nb.ToString();
                StackInput.GetComponent<StackInput>().MaxValue = PlayerManager.main.PlayerRogue.Inventory[index].Nb;
                StackInput.GetComponent<StackInput>().CurrentValue = PlayerManager.main.PlayerRogue.Inventory[index].Nb;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.PlayerRogue.Inventory[index].Item.Img;
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.PlayerRogue.Inventory[index].Item;
                selectionInProgress = true;
                current_item = PlayerManager.main.PlayerRogue.Inventory[index].Item;
                current_item_player = 2;
                current_item_index = index;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
            {               
                if (PlayerManager.main.PlayerRogue.Inventory[index].Item != null 
                    && PlayerManager.main.PlayerRogue.Inventory[index].Item as Equipable == null
                    && PlayerManager.main.PlayerRogue.Inventory[index].Item.name == current_item.name)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.PlayerRogue.Inventory[index].Nb += current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                    ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                }
                else if (PlayerManager.main.PlayerRogue.Inventory[index].Item != current_item && PlayerManager.main.PlayerRogue.Inventory[index].Nb != 0)
                {

                        switch (current_item_player)
                        {
                            case 1:
                            if (!(PlayerManager.main.PlayerRogue.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb = PlayerManager.main.PlayerRogue.Inventory[index].Nb;
                                PlayerManager.main.PlayerFairy.Inventory[current_item_index].Item = PlayerManager.main.PlayerRogue.Inventory[index].Item;
                                PlayerManager.main.PlayerRogue.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerRogue.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            case 2:
                            if (!(PlayerManager.main.PlayerRogue.Inventory[index].Item.Img == current_item.Img
                                && PlayerManager.main.PlayerRogue.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerRogue.Inventory[current_item_index].Nb = PlayerManager.main.PlayerRogue.Inventory[index].Nb;
                                PlayerManager.main.PlayerRogue.Inventory[current_item_index].Item = PlayerManager.main.PlayerRogue.Inventory[index].Item;
                                PlayerManager.main.PlayerRogue.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerRogue.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            case 3:
                            if (!(PlayerManager.main.PlayerRogue.Inventory[index].Item.Img == current_item.Img
                                && PlayerManager.main.PlayerTank.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerTank.Inventory[current_item_index].Nb = PlayerManager.main.PlayerRogue.Inventory[index].Nb;
                                PlayerManager.main.PlayerTank.Inventory[current_item_index].Item = PlayerManager.main.PlayerRogue.Inventory[index].Item;
                                PlayerManager.main.PlayerRogue.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerRogue.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            default: break;
                        }                    
                }
                else if (PlayerManager.main.PlayerRogue.Inventory[index].Nb == 0)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.PlayerRogue.Inventory[index].Item = current_item;
                    PlayerManager.main.PlayerRogue.Inventory[index].Nb = current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                    ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                }
            }
        }
    }

    public void TakeObjectInTankInventory(int index)
    {
        if (!SelectionInProgress)
        {
            if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.PlayerTank.Inventory[index].Nb == 1)
            {
                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 1;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.PlayerTank.Inventory[index].Item.Img;
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.PlayerTank.Inventory[index].Item;

                current_item = PlayerManager.main.PlayerTank.Inventory[index].Item;
                current_item_nb = PlayerManager.main.PlayerTank.Inventory[index].Nb;
                current_item_player = 3;
                current_item_index = index;

                PlayerManager.main.PlayerTank.Inventory[index].Item = ItemInventory.emptyPickable;
                PlayerManager.main.PlayerTank.Inventory[index].Nb = 0;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.PlayerTank.Inventory[index].Nb > 1)
            {
                StackInput.GetComponent<CanvasGroup>().alpha = 1;
                StackInput.GetComponent<CanvasGroup>().interactable = true;
                StackInput.GetComponent<CanvasGroup>().blocksRaycasts = true;
                StackInput.transform.GetChild(3).GetComponentInChildren<Text>().text = PlayerManager.main.PlayerTank.Inventory[index].Nb.ToString();
                StackInput.GetComponent<StackInput>().MaxValue = PlayerManager.main.PlayerTank.Inventory[index].Nb;
                StackInput.GetComponent<StackInput>().CurrentValue = PlayerManager.main.PlayerTank.Inventory[index].Nb;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.PlayerTank.Inventory[index].Item.Img;
                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = PlayerManager.main.PlayerTank.Inventory[index].Item;

                selectionInProgress = true;
                current_item = PlayerManager.main.PlayerTank.Inventory[index].Item;
                current_item_player = 3;
                current_item_index = index;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
            {      
                if (   PlayerManager.main.PlayerTank.Inventory[index].Item != null
                    && PlayerManager.main.PlayerTank.Inventory[index].Item.name == current_item.name
                    && PlayerManager.main.PlayerTank.Inventory[index].Item as Equipable == null
                    )
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.PlayerTank.Inventory[index].Nb += current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                    ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                }
                else if (PlayerManager.main.PlayerTank.Inventory[index].Item != current_item && PlayerManager.main.PlayerTank.Inventory[index].Nb != 0)
                {                    
                        switch (current_item_player)
                        {
                            case 1:
                            if (!(PlayerManager.main.PlayerTank.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb = PlayerManager.main.PlayerTank.Inventory[index].Nb;
                                PlayerManager.main.PlayerFairy.Inventory[current_item_index].Item = PlayerManager.main.PlayerTank.Inventory[index].Item;
                                PlayerManager.main.PlayerTank.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerTank.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            case 2:
                            if (!(PlayerManager.main.PlayerTank.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.PlayerRogue.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerRogue.Inventory[current_item_index].Nb = PlayerManager.main.PlayerTank.Inventory[index].Nb;
                                PlayerManager.main.PlayerRogue.Inventory[current_item_index].Item = PlayerManager.main.PlayerTank.Inventory[index].Item;
                                PlayerManager.main.PlayerTank.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerTank.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            case 3:
                            if (!(PlayerManager.main.PlayerTank.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.PlayerTank.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.PlayerTank.Inventory[current_item_index].Nb = PlayerManager.main.PlayerTank.Inventory[index].Nb;
                                PlayerManager.main.PlayerTank.Inventory[current_item_index].Item = PlayerManager.main.PlayerTank.Inventory[index].Item;
                                PlayerManager.main.PlayerTank.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.PlayerTank.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                                ItemOnMouse.GetComponentInChildren<ImageOnMouse>().Item = null;
                            }
                                break;
                            default: break;
                        }                    
                }
                else if (PlayerManager.main.PlayerTank.Inventory[index].Nb == 0)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    PlayerManager.main.PlayerTank.Inventory[index].Item = current_item;
                    PlayerManager.main.PlayerTank.Inventory[index].Nb = current_item_nb;
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
            switch(current_item_player)
            {
                case 1:
                    PlayerManager.main.PlayerFairy.Inventory[current_item_index].Item = ItemInventory.emptyPickable;
                    PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb = 0;
                    break;
                case 2:
                    PlayerManager.main.PlayerRogue.Inventory[current_item_index].Item = ItemInventory.emptyPickable;
                    PlayerManager.main.PlayerRogue.Inventory[current_item_index].Nb = 0;
                    break;
                case 3:
                    PlayerManager.main.PlayerTank.Inventory[current_item_index].Item = ItemInventory.emptyPickable;
                    PlayerManager.main.PlayerTank.Inventory[current_item_index].Nb = 0;
                    break;
            }
            
            current_item_nb = nbStackValidated;
        }
        else
        {
            current_item_nb = nbStackValidated;

            switch (current_item_player)
            {
                case 1:
                    PlayerManager.main.PlayerFairy.Inventory[current_item_index].Nb -= nbStackValidated;
                    break;
                case 2:
                    PlayerManager.main.PlayerRogue.Inventory[current_item_index].Nb -= nbStackValidated;
                    break;
                case 3:
                    PlayerManager.main.PlayerTank.Inventory[current_item_index].Nb -= nbStackValidated;
                    break;
            }
            
        }        
    }
}
