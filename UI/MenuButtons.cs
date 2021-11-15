using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuCharacter;
    [SerializeField] GameObject skillTree;
    [SerializeField] GameObject trading;
    [SerializeField] GameObject dropItem;
    [SerializeField] GameObject lootWindow;

    public void OpenInventory()
    {
        if (!inventory.GetComponent<InventoryManager>().SelectionInProgress
            && inventory.transform.GetChild(7).GetComponent<CanvasGroup>().alpha == 0
            && trading.GetComponent<CanvasGroup>().alpha == 0)
        {
            AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
            menuPause.GetComponent<CanvasGroup>().alpha = 0;
            menuPause.GetComponent<CanvasGroup>().interactable = false;
            menuPause.GetComponent<CanvasGroup>().blocksRaycasts = false;

            menuCharacter.GetComponent<CanvasGroup>().alpha = 0;
            menuCharacter.GetComponent<CanvasGroup>().interactable = false;
            menuCharacter.GetComponent<CanvasGroup>().blocksRaycasts = false;

            skillTree.GetComponent<CanvasGroup>().alpha = 0;
            skillTree.GetComponent<CanvasGroup>().interactable = false;
            skillTree.GetComponent<CanvasGroup>().blocksRaycasts = false;

            lootWindow.GetComponent<CanvasGroup>().alpha = 0;
            lootWindow.GetComponent<CanvasGroup>().interactable = false;
            lootWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;

            if (inventory.GetComponent<CanvasGroup>().alpha == 0)
            {
                inventory.GetComponent<CanvasGroup>().alpha = 1;
                inventory.GetComponent<CanvasGroup>().interactable = true;
                inventory.GetComponent<CanvasGroup>().blocksRaycasts = true;

                dropItem.GetComponent<CanvasGroup>().alpha = 1;
                dropItem.GetComponent<CanvasGroup>().interactable = true;
                dropItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                inventory.GetComponent<CanvasGroup>().alpha = 0;
                inventory.GetComponent<CanvasGroup>().interactable = false;
                inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;

                dropItem.GetComponent<CanvasGroup>().alpha = 0;
                dropItem.GetComponent<CanvasGroup>().interactable = false;
                dropItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void OpenMenuPause()
    {
        if (!inventory.GetComponent<InventoryManager>().SelectionInProgress
            && inventory.transform.GetChild(7).GetComponent<CanvasGroup>().alpha == 0
            && trading.GetComponent<CanvasGroup>().alpha == 0)
        {
            AkSoundEngine.PostEvent("MenuSelect_Play", gameObject); // Play Click Sound
            inventory.GetComponent<CanvasGroup>().alpha = 0;
            inventory.GetComponent<CanvasGroup>().interactable = false;
            inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;

            menuCharacter.GetComponent<CanvasGroup>().alpha = 0;
            menuCharacter.GetComponent<CanvasGroup>().interactable = false;
            menuCharacter.GetComponent<CanvasGroup>().blocksRaycasts = false;

            skillTree.GetComponent<CanvasGroup>().alpha = 0;
            skillTree.GetComponent<CanvasGroup>().interactable = false;
            skillTree.GetComponent<CanvasGroup>().blocksRaycasts = false;

            dropItem.GetComponent<CanvasGroup>().alpha = 0;
            dropItem.GetComponent<CanvasGroup>().interactable = false;
            dropItem.GetComponent<CanvasGroup>().blocksRaycasts = false;

            lootWindow.GetComponent<CanvasGroup>().alpha = 0;
            lootWindow.GetComponent<CanvasGroup>().interactable = false;
            lootWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;

            if (menuPause.GetComponent<CanvasGroup>().alpha == 0)
            {
                menuPause.GetComponent<CanvasGroup>().alpha = 1;
                menuPause.GetComponent<CanvasGroup>().interactable = true;
                menuPause.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                menuPause.GetComponent<CanvasGroup>().alpha = 0;
                menuPause.GetComponent<CanvasGroup>().interactable = false;
                menuPause.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void OpenMenuCharacter()
    {
        if (!inventory.GetComponent<InventoryManager>().SelectionInProgress 
            && inventory.transform.GetChild(7).GetComponent<CanvasGroup>().alpha == 0
            && trading.GetComponent<CanvasGroup>().alpha == 0)            
        {
            AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
            menuPause.GetComponent<CanvasGroup>().alpha = 0;
            menuPause.GetComponent<CanvasGroup>().interactable = false;
            menuPause.GetComponent<CanvasGroup>().blocksRaycasts = false;

            inventory.GetComponent<CanvasGroup>().alpha = 0;
            inventory.GetComponent<CanvasGroup>().interactable = false;
            inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;

            skillTree.GetComponent<CanvasGroup>().alpha = 0;
            skillTree.GetComponent<CanvasGroup>().interactable = false;
            skillTree.GetComponent<CanvasGroup>().blocksRaycasts = false;

            lootWindow.GetComponent<CanvasGroup>().alpha = 0;
            lootWindow.GetComponent<CanvasGroup>().interactable = false;
            lootWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;

            if (menuCharacter.GetComponent<CanvasGroup>().alpha == 0)
            {
                menuCharacter.GetComponent<CanvasGroup>().alpha = 1;
                menuCharacter.GetComponent<CanvasGroup>().interactable = true;
                menuCharacter.GetComponent<CanvasGroup>().blocksRaycasts = true;

                dropItem.GetComponent<CanvasGroup>().alpha = 1;
                dropItem.GetComponent<CanvasGroup>().interactable = true;
                dropItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                menuCharacter.GetComponent<CanvasGroup>().alpha = 0;
                menuCharacter.GetComponent<CanvasGroup>().interactable = false;
                menuCharacter.GetComponent<CanvasGroup>().blocksRaycasts = false;

                dropItem.GetComponent<CanvasGroup>().alpha = 0;
                dropItem.GetComponent<CanvasGroup>().interactable = false;
                dropItem.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }

    public void OpenSkillTree()
    {
        if (!inventory.GetComponent<InventoryManager>().SelectionInProgress
            && inventory.transform.GetChild(7).GetComponent<CanvasGroup>().alpha == 0
            && trading.GetComponent<CanvasGroup>().alpha == 0)
        {
            AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
            menuPause.GetComponent<CanvasGroup>().alpha = 0;
            menuPause.GetComponent<CanvasGroup>().interactable = false;
            menuPause.GetComponent<CanvasGroup>().blocksRaycasts = false;

            menuCharacter.GetComponent<CanvasGroup>().alpha = 0;
            menuCharacter.GetComponent<CanvasGroup>().interactable = false;
            menuCharacter.GetComponent<CanvasGroup>().blocksRaycasts = false;

            inventory.GetComponent<CanvasGroup>().alpha = 0;
            inventory.GetComponent<CanvasGroup>().interactable = false;
            inventory.GetComponent<CanvasGroup>().blocksRaycasts = false;

            dropItem.GetComponent<CanvasGroup>().alpha = 0;
            dropItem.GetComponent<CanvasGroup>().interactable = false;
            dropItem.GetComponent<CanvasGroup>().blocksRaycasts = false;

            lootWindow.GetComponent<CanvasGroup>().alpha = 0;
            lootWindow.GetComponent<CanvasGroup>().interactable = false;
            lootWindow.GetComponent<CanvasGroup>().blocksRaycasts = false;

            if (skillTree.GetComponent<CanvasGroup>().alpha == 0)
            {
                skillTree.GetComponent<CanvasGroup>().alpha = 1;
                skillTree.GetComponent<CanvasGroup>().interactable = true;
                skillTree.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                skillTree.GetComponent<CanvasGroup>().alpha = 0;
                skillTree.GetComponent<CanvasGroup>().interactable = false;
                skillTree.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }
}
