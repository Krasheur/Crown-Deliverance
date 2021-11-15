using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraderInventoryManager : MonoBehaviour
{
    [SerializeField] Image playerPortrait;
    [SerializeField] Image traderPortrait;   

    [SerializeField] Text playerGold;
    [SerializeField] Text traderGold;    

    [SerializeField] GameObject panel_Player;
    [SerializeField] GameObject panel_Trader;

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
            if (playerPortrait.sprite != PlayerManager.main.FocusedCharacter.Portrait
                || traderPortrait.sprite != TradingManager.main.CurrentTrader.Portrait)      
            {
                ActualizePortrait();
            }

            for (int i = 0; i < panel_Player.GetComponent<Transform>().childCount; i++)
            {
                if (PlayerManager.main.FocusedCharacter.Inventory[i].Item != ItemInventory.emptyPickable)
                {
                    panel_Player.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    panel_Player.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite = PlayerManager.main.FocusedCharacter.Inventory[i].Item.Img;
                    panel_Player.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().text = PlayerManager.main.FocusedCharacter.Inventory[i].Nb.ToString();
                }
                else
                {
                    panel_Player.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                }

                if (PlayerManager.main.FocusedCharacter.Inventory[i].Nb > 1)
                {
                    panel_Player.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = true;
                    panel_Player.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = true;
                }
                else
                {
                    panel_Player.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = false;
                    panel_Player.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = false;
                }
            }

            for (int i = 0; i < panel_Trader.GetComponent<Transform>().childCount; i++)
            {
                if (TradingManager.main.CurrentTrader.Inventory[i].Item != ItemInventory.emptyPickable)
                {
                    panel_Trader.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    panel_Trader.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().sprite = TradingManager.main.CurrentTrader.Inventory[i].Item.Img;
                    panel_Trader.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().text = TradingManager.main.CurrentTrader.Inventory[i].Nb.ToString();
                }
                else
                {
                    panel_Trader.GetComponent<Transform>().GetChild(i).GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                }

                if (TradingManager.main.CurrentTrader.Inventory[i].Nb > 1)
                {
                    panel_Trader.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = true;
                    panel_Trader.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = true;
                }
                else
                {
                    panel_Trader.GetComponent<Transform>().GetChild(i).GetChild(1).GetComponent<Image>().enabled = false;
                    panel_Trader.GetComponent<Transform>().GetChild(i).transform.GetComponentInChildren<Text>().enabled = false;
                }
            }

            UpdateGold();
        }        
    }

    void ActualizePortrait()
    {
        playerPortrait.sprite = PlayerManager.main.FocusedCharacter.Portrait;
        if (TradingManager.main.CurrentTrader != null)
        {
            traderPortrait.sprite = TradingManager.main.CurrentTrader.Portrait;
        }
    }

    void UpdateGold()
    {
        playerGold.text = PlayerManager.main.FocusedCharacter.Gold.ToString();
        if (TradingManager.main.CurrentTrader != null)
        {
            traderGold.text = TradingManager.main.CurrentTrader.Gold.ToString();
        }
    }

    public void TakeObjectInPlayerInventory(int index)
    {
        if (!SelectionInProgress)
        {
            if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && PlayerManager.main.FocusedCharacter.Inventory[index].Nb == 1)
            {
                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 1;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = PlayerManager.main.FocusedCharacter.Inventory[index].Item.Img;                

                current_item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;
                current_item_nb = PlayerManager.main.FocusedCharacter.Inventory[index].Nb;
                current_item_player = 1;
                current_item_index = index;

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
                selectionInProgress = true;
                current_item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;
                current_item_player = 1;
                current_item_index = index;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
            {
                if (   PlayerManager.main.FocusedCharacter.Inventory[index].Item != null
                    && PlayerManager.main.FocusedCharacter.Inventory[index].Item as Equipable == null
                    && PlayerManager.main.FocusedCharacter.Inventory[index].Item.name == current_item.name)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    if (current_item_player == 2)
                    {
                        TradingManager.main.TradeCost -= current_item.Value * current_item_nb;
                    }
                    PlayerManager.main.FocusedCharacter.Inventory[index].Nb += current_item_nb;                    
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                }
                else if (PlayerManager.main.FocusedCharacter.Inventory[index].Item != current_item && PlayerManager.main.FocusedCharacter.Inventory[index].Nb != 0)
                {
                    switch (current_item_player)
                    {
                        case 1:
                            if (!(PlayerManager.main.FocusedCharacter.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Nb = PlayerManager.main.FocusedCharacter.Inventory[index].Nb;
                                PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Item = PlayerManager.main.FocusedCharacter.Inventory[index].Item;
                                PlayerManager.main.FocusedCharacter.Inventory[index].Nb = current_item_nb;
                                PlayerManager.main.FocusedCharacter.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                            }
                            break;
                        case 2:
                            if (!(PlayerManager.main.FocusedCharacter.Inventory[index].Item.name == current_item.name
                                && TradingManager.main.CurrentTrader.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                TradingManager.main.TradeCost -= (int)(current_item.Value * current_item_nb);
                                TradingManager.main.TradeCost += PlayerManager.main.FocusedCharacter.Inventory[index].Item.Value * PlayerManager.main.FocusedCharacter.Inventory[index].Nb;   
                                
                                TradingManager.main.CurrentTrader.PutInInventory(PlayerManager.main.FocusedCharacter.Inventory[index].Item, current_item_index);
                                TradingManager.main.CurrentTrader.Inventory[current_item_index].Nb = PlayerManager.main.FocusedCharacter.Inventory[index].Nb;

                                PlayerManager.main.FocusedCharacter.PutInInventory(current_item, index);
                                PlayerManager.main.FocusedCharacter.Inventory[index].Nb = current_item_nb;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                            }
                            break;                        
                        default: break;
                    }
                }
                else if (PlayerManager.main.FocusedCharacter.Inventory[index].Nb == 0)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    if (current_item_player == 2)
                    {
                        TradingManager.main.TradeCost -= (int)(current_item.Value * current_item_nb);
                    }
                    PlayerManager.main.FocusedCharacter.PutInInventory(current_item, index);
                    PlayerManager.main.FocusedCharacter.Inventory[index].Nb = current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;  
                }               
            }
        }
    }

    public void TakeObjectInTraderInventory(int index)
    {
        if (!SelectionInProgress)
        {
            if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && TradingManager.main.CurrentTrader.Inventory[index].Nb == 1)
            {
                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 1;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = TradingManager.main.CurrentTrader.Inventory[index].Item.Img;

                current_item = TradingManager.main.CurrentTrader.Inventory[index].Item;
                current_item_nb = TradingManager.main.CurrentTrader.Inventory[index].Nb;
                current_item_player = 2;
                current_item_index = index;

                TradingManager.main.CurrentTrader.Inventory[index].Item = ItemInventory.emptyPickable;
                TradingManager.main.CurrentTrader.Inventory[index].Nb = 0;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 0 && TradingManager.main.CurrentTrader.Inventory[index].Nb > 1)
            {
                StackInput.GetComponent<CanvasGroup>().alpha = 1;
                StackInput.GetComponent<CanvasGroup>().interactable = true;
                StackInput.GetComponent<CanvasGroup>().blocksRaycasts = true;
                StackInput.transform.GetChild(3).GetComponentInChildren<Text>().text = TradingManager.main.CurrentTrader.Inventory[index].Nb.ToString();
                StackInput.GetComponent<StackInput>().MaxValue = TradingManager.main.CurrentTrader.Inventory[index].Nb;
                StackInput.GetComponent<StackInput>().CurrentValue = TradingManager.main.CurrentTrader.Inventory[index].Nb;
                ItemOnMouse.GetComponentInChildren<Image>().sprite = TradingManager.main.CurrentTrader.Inventory[index].Item.Img;
                selectionInProgress = true;
                current_item = TradingManager.main.CurrentTrader.Inventory[index].Item;
                current_item_player = 2;
                current_item_index = index;
            }
            else if (ItemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
            {
                if (   TradingManager.main.CurrentTrader.Inventory[index].Item != null
                    && TradingManager.main.CurrentTrader.Inventory[index].Item as Equipable == null
                    && TradingManager.main.CurrentTrader.Inventory[index].Item.name == current_item.name)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    if (current_item_player == 1)
                    {
                        TradingManager.main.TradeCost += current_item.Value * current_item_nb;
                    }
                    TradingManager.main.CurrentTrader.Inventory[index].Nb += current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                }
                else if (TradingManager.main.CurrentTrader.Inventory[index].Item != current_item && TradingManager.main.CurrentTrader.Inventory[index].Nb != 0)
                {
                    switch (current_item_player)
                    {
                        case 1:
                            if (!(TradingManager.main.CurrentTrader.Inventory[index].Item.name == current_item.name
                                && PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                TradingManager.main.TradeCost += current_item.Value * current_item_nb;
                                TradingManager.main.TradeCost -= (int)(TradingManager.main.CurrentTrader.Inventory[index].Item.Value * TradingManager.main.CurrentTrader.Inventory[index].Nb * 1.5f);
                                int tempo = TradingManager.main.CurrentTrader.Inventory[index].Nb;
                                PlayerManager.main.FocusedCharacter.PutInInventory(TradingManager.main.CurrentTrader.Inventory[index].Item, current_item_index);
                                PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Nb = tempo;
                                TradingManager.main.CurrentTrader.PutInInventory(current_item, index);
                                TradingManager.main.CurrentTrader.Inventory[index].Nb = current_item_nb;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                            }
                            break;
                        case 2:
                            if (!(TradingManager.main.CurrentTrader.Inventory[index].Item.name == current_item.name
                                && TradingManager.main.CurrentTrader.Inventory[current_item_index].Nb > 0))
                            {
                                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                                TradingManager.main.CurrentTrader.Inventory[current_item_index].Nb = TradingManager.main.CurrentTrader.Inventory[index].Nb;
                                TradingManager.main.CurrentTrader.Inventory[current_item_index].Item = TradingManager.main.CurrentTrader.Inventory[index].Item;
                                TradingManager.main.CurrentTrader.Inventory[index].Nb = current_item_nb;
                                TradingManager.main.CurrentTrader.Inventory[index].Item = current_item;
                                ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                                ItemOnMouse.GetComponentInChildren<Image>().sprite = null;
                            }
                            break;
                        default: break;
                    }
                }
                else if (TradingManager.main.CurrentTrader.Inventory[index].Nb == 0)
                {
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    if (current_item_player == 1)
                    {
                        TradingManager.main.TradeCost += current_item.Value * current_item_nb;
                    }
                    TradingManager.main.CurrentTrader.PutInInventory(current_item, index);
                    TradingManager.main.CurrentTrader.Inventory[index].Nb = current_item_nb;
                    ItemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                    ItemOnMouse.GetComponentInChildren<Image>().sprite = null;   
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
            switch (current_item_player)
            {
                case 1:
                    PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Item = ItemInventory.emptyPickable;
                    PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Nb = 0;
                    break;
                case 2:
                    TradingManager.main.CurrentTrader.Inventory[current_item_index].Item = ItemInventory.emptyPickable;
                    TradingManager.main.CurrentTrader.Inventory[current_item_index].Nb = 0;
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
                    PlayerManager.main.FocusedCharacter.Inventory[current_item_index].Nb -= nbStackValidated;
                    break;
                case 2:
                    TradingManager.main.CurrentTrader.Inventory[current_item_index].Nb -= nbStackValidated;
                    break;               
            }

        }
    }
}