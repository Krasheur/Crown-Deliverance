using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingManager : MonoBehaviour
{
    static public TradingManager main;

    private int tradeCost;
    [SerializeField] Text costText;
    [SerializeField] Image costArrow;

    private Character currentTrader;
    public Character CurrentTrader { get => currentTrader; set => currentTrader = value; }
    public int TradeCost { get => tradeCost; set => tradeCost = value; }

    private ItemInventory[] playerInventory;
    private ItemInventory[] traderInventory;

    // Start is called before the first frame update
    void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }
        main = this;
    }
    
    void Update()
    {
        if(TradeCost >= 0)
        {
            costArrow.gameObject.GetComponent<Transform>().localScale = new Vector3(-1,1,1);
        }
        else
        {
            costArrow.gameObject.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        }

        costText.text = TradeCost.ToString();
    }

    public void OpenTrade()
    {
        TradeCost = 0;
        PlayerManager.main.FocusedCharacter.State = CHARACTER_STATE.LOCKED;        
        
        playerInventory = new ItemInventory[PlayerManager.main.FocusedCharacter.Inventory.Length];
        for (int i = 0; i < PlayerManager.main.FocusedCharacter.Inventory.Length; i++)
        {
            playerInventory[i] = new ItemInventory(PlayerManager.main.FocusedCharacter.Inventory[i].Item, PlayerManager.main.FocusedCharacter.Inventory[i].Nb);
        }
        
        traderInventory = new ItemInventory[currentTrader.Inventory.Length];
        for (int i = 0; i < currentTrader.Inventory.Length; i++)
        {            
            traderInventory[i] = new ItemInventory(currentTrader.Inventory[i].Item, currentTrader.Inventory[i].Nb);
        }

        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void CloseTrade()
    {
        if (TradeCost + PlayerManager.main.FocusedCharacter.Gold >= 0 && -TradeCost + currentTrader.Gold >= 0)
        {
            AkSoundEngine.PostEvent("Gold_Play",gameObject); // Play Gold Sound
            PlayerManager.main.FocusedCharacter.Gold += TradeCost;
            PlayerManager.main.FocusedCharacter.State = CHARACTER_STATE.FREE;            
            currentTrader.Gold -= TradeCost;
            transform.GetChild(11).GetComponent<CanvasGroup>().alpha = 0;
            transform.GetChild(11).GetComponentInChildren<Image>().sprite = null;
            GetComponent<CanvasGroup>().alpha = 0;
            GetComponent<CanvasGroup>().interactable = false;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            currentTrader = null;
        }
    }

    public void CancelTrade()
    {              
        PlayerManager.main.FocusedCharacter.State = CHARACTER_STATE.FREE;         
        AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
        for (int i = 0; i < PlayerManager.main.FocusedCharacter.Inventory.Length; i++)
        {
            PlayerManager.main.FocusedCharacter.Inventory[i] = playerInventory[i];
        }
        
        for (int i = 0; i < currentTrader.Inventory.Length; i++)
        {
            currentTrader.Inventory[i] = traderInventory[i];        
        }
        transform.GetChild(11).GetComponent<CanvasGroup>().alpha = 0;
        transform.GetChild(11).GetComponentInChildren<Image>().sprite = null;
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().interactable = false;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        currentTrader = null;
    }
}
