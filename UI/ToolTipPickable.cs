using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTipPickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] int index;
    [SerializeField] PlayerManager.CHARACTER character;
    [SerializeField] bool isMerchant = false;
    [SerializeField] private ToolTipPopUp toolTipPopUp;        

    public Pickable pickable;

    // Start is called before the first frame update
    void Start()
    {
        toolTipPopUp = GameObject.Find("TooltipPopUp").GetComponent<ToolTipPopUp>();
    }    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMerchant)
        {
            switch (character)
            {
                case PlayerManager.CHARACTER.FAIRY:
                    if (PlayerManager.main.Fairy.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.Fairy.Inventory[index].Item, PlayerManager.main.Fairy);
                    }
                    break;
                case PlayerManager.CHARACTER.ROGUE:
                    if (PlayerManager.main.Rogue.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.Rogue.Inventory[index].Item, PlayerManager.main.Rogue);
                    }
                    break;
                case PlayerManager.CHARACTER.TANK:
                    if (PlayerManager.main.Tank.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.Tank.Inventory[index].Item, PlayerManager.main.Tank);
                    }
                    break;
                case PlayerManager.CHARACTER.FOCUSED:
                    if (PlayerManager.main.FocusedCharacter.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.FocusedCharacter.Inventory[index].Item, PlayerManager.main.FocusedCharacter);
                    }
                    break;
            }
        }
        else
        {
            if (TradingManager.main.CurrentTrader.Inventory[index].Nb > 0)
            {
                toolTipPopUp.DisplayInfoPickable(TradingManager.main.CurrentTrader.Inventory[index].Item, TradingManager.main.CurrentTrader);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipPopUp.HideInfo();        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isMerchant)
        {
            switch (character)
            {
                case PlayerManager.CHARACTER.FAIRY:
                    if (PlayerManager.main.Fairy.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.Fairy.Inventory[index].Item, PlayerManager.main.Fairy);
                    }
                    break;
                case PlayerManager.CHARACTER.ROGUE:
                    if (PlayerManager.main.Rogue.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.Rogue.Inventory[index].Item, PlayerManager.main.Rogue);
                    }
                    break;
                case PlayerManager.CHARACTER.TANK:
                    if (PlayerManager.main.Tank.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.Tank.Inventory[index].Item, PlayerManager.main.Tank);
                    }
                    break;
                case PlayerManager.CHARACTER.FOCUSED:
                    if (PlayerManager.main.FocusedCharacter.Inventory[index].Nb > 0)
                    {
                        toolTipPopUp.DisplayInfoPickable(PlayerManager.main.FocusedCharacter.Inventory[index].Item, PlayerManager.main.FocusedCharacter);
                    }
                    break;
            }
        }
        else
        {
            if (TradingManager.main.CurrentTrader.Inventory[index].Nb > 0)
            {
                toolTipPopUp.DisplayInfoPickable(TradingManager.main.CurrentTrader.Inventory[index].Item, TradingManager.main.CurrentTrader);
            }
        }
    }
}
