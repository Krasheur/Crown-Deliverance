using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RightClickButton : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent rightClick;
    public int index;
    public bool globalInventory;
    public string player;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {                        
            if (PlayerManager.main.FocusedCharacter.Inventory[index].Item as Consumable && !globalInventory)
            {                
                PlayerManager.main.FocusedCharacter.UseConsumable((PlayerManager.main.FocusedCharacter.Inventory[index].Item as Consumable), index);
            }
            else if(PlayerManager.main.PlayerFairy.Inventory[index].Item as Consumable && globalInventory && player == "Fairy")
            {
                PlayerManager.main.PlayerFairy.UseConsumable((PlayerManager.main.PlayerFairy.Inventory[index].Item as Consumable), index);
            }
            else if (PlayerManager.main.PlayerRogue.Inventory[index].Item as Consumable && globalInventory && player == "Rogue")
            {
                PlayerManager.main.PlayerRogue.UseConsumable((PlayerManager.main.PlayerRogue.Inventory[index].Item as Consumable), index);
            }
            else if (PlayerManager.main.PlayerTank.Inventory[index].Item as Consumable && globalInventory && player == "Tank")
            {
                PlayerManager.main.PlayerTank.UseConsumable((PlayerManager.main.PlayerTank.Inventory[index].Item as Consumable), index);
            }
        }
    }
}
