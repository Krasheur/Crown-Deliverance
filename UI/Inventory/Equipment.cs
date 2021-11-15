using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [SerializeField] MonoInventoryManager monoInventory;
    [SerializeField] GameObject itemOnMouse;

    [SerializeField] Sprite empty;

    // Update is called once per frame
    void Update()
    {
        if (monoInventory.Current_item as Armor && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
        {
            transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
        }
        else
        {
            transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
        }

        if (monoInventory.Current_item as Weapon && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
        {
            transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = true;
        }
        else
        {
            transform.GetChild(1).GetChild(1).GetComponent<Image>().enabled = false;
        }

        if (monoInventory.Current_item as Shield && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1)
        {
            transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled = true;
        }
        else if(monoInventory.Current_item as Weapon && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1 && PlayerManager.main.FocusedCharacter == PlayerManager.main.Rogue)
        {
            transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled = true;
        }
        else
        {
            transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled = false;
        }

        if (PlayerManager.main.FocusedCharacter.Body != null)
        {
            transform.GetChild(0).GetComponent<Button>().image.sprite = PlayerManager.main.FocusedCharacter.Body.Img;
        }
        else
        {
            transform.GetChild(0).GetComponent<Button>().image.sprite = empty;
        }

        if (PlayerManager.main.FocusedCharacter.RightHand != null)
        {
            transform.GetChild(1).GetComponent<Button>().image.sprite = PlayerManager.main.FocusedCharacter.RightHand.Img;
        }
        else
        {
            transform.GetChild(1).GetComponent<Button>().image.sprite = empty;
        }

        if (PlayerManager.main.FocusedCharacter.LeftHand != null)
        {
            transform.GetChild(2).GetComponent<Button>().image.sprite = PlayerManager.main.FocusedCharacter.LeftHand.Img;
        }
        else
        {
            transform.GetChild(2).GetComponent<Button>().image.sprite = empty;
        }
        
        if(PlayerManager.main.FocusedCharacter == PlayerManager.main.PlayerFairy)
        {
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(1).localPosition = new Vector3(-145.5f, 245.6f, 0f);
            transform.GetChild(0).localPosition = new Vector3(-145.5f, 125.6f, 0f);
        }
        else
        {
            transform.GetChild(1).localPosition = new Vector3(-145.5f, 245.6f, 0f);
            transform.GetChild(0).localPosition = new Vector3(-145.5f, 25.6f, 0f);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(2).localPosition = new Vector3(-145.5f, 135.6f, 0f);
        }

        UpdateRarity();
    }

    public void ArmorSlot()
    {
        if(monoInventory.Current_item as Armor && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1 && monoInventory.Current_item_nb == 1 && PlayerManager.main.FocusedCharacter.Body == null)
        {            
            if (PlayerManager.main.FocusedCharacter.Equip(monoInventory.Current_item as Equipable, false, monoInventory.Current_item_index))
            {
                AkSoundEngine.PostEvent("Equip_Play", gameObject); // Play Equip Sound
                itemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                itemOnMouse.GetComponentInChildren<Image>().sprite = null;
            }
        }        
        else if (itemOnMouse.GetComponent<CanvasGroup>().alpha == 0)
        {
            bool spaceInInventory = false;

            for (int i = 0; i < 24; i++)
            {
                if (PlayerManager.main.FocusedCharacter.Inventory[i].Nb == 0)
                {
                    spaceInInventory = true;
                    break;
                }
            }

            if (spaceInInventory && PlayerManager.main.FocusedCharacter.Body != null)
            {
                PlayerManager.main.FocusedCharacter.UnEquip(PlayerManager.main.FocusedCharacter.Body, false);
                AkSoundEngine.PostEvent("Unequip_Play", gameObject); // Play Unequip Sound
                PlayerManager.main.FocusedCharacter.Body = null;
            }
        }
    }

    public void WeaponSlot()
    {
        if (monoInventory.Current_item as Weapon && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1 && monoInventory.Current_item_nb == 1)
        {            
            if (PlayerManager.main.FocusedCharacter.Equip(monoInventory.Current_item as Equipable, true, monoInventory.Current_item_index))
            {
                AkSoundEngine.PostEvent("Equip_Play", gameObject); // Play Equip Sound
                itemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                itemOnMouse.GetComponentInChildren<Image>().sprite = null;
            }
        }
        else if (itemOnMouse.GetComponent<CanvasGroup>().alpha == 0)
        {
            bool spaceInInventory = false;

            for (int i = 0; i < 24; i++)
            {
                if (PlayerManager.main.FocusedCharacter.Inventory[i].Nb == 0)
                {
                    spaceInInventory = true;
                    break;
                }
            }

            if (spaceInInventory && PlayerManager.main.FocusedCharacter.RightHand != null)
            {
                PlayerManager.main.FocusedCharacter.UnEquip(PlayerManager.main.FocusedCharacter.RightHand,false);
                AkSoundEngine.PostEvent("Unequip_Play", gameObject); // Play Unequip Sound
                PlayerManager.main.FocusedCharacter.RightHand = null;
            }
        }
    }

    public void ShieldSlot()
    {
        if (monoInventory.Current_item as Shield && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1 && monoInventory.Current_item_nb == 1)
        {            
            if (PlayerManager.main.FocusedCharacter.Equip(monoInventory.Current_item as Equipable, false, monoInventory.Current_item_index))
            {
                AkSoundEngine.PostEvent("Equip_Play", gameObject); // Play Equip Sound
                itemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                itemOnMouse.GetComponentInChildren<Image>().sprite = null;
            }
        }
        else if (monoInventory.Current_item as Weapon && itemOnMouse.GetComponent<CanvasGroup>().alpha == 1 && monoInventory.Current_item_nb == 1)
        {
            if (PlayerManager.main.FocusedCharacter.Equip(monoInventory.Current_item as Equipable, false, monoInventory.Current_item_index))
            {
                AkSoundEngine.PostEvent("Equip_Play", gameObject); // Play Equip Sound
                itemOnMouse.GetComponent<CanvasGroup>().alpha = 0;
                itemOnMouse.GetComponentInChildren<Image>().sprite = null;
            }
        }
        else if (itemOnMouse.GetComponent<CanvasGroup>().alpha == 0)
        {          
            bool spaceInInventory = false;

            for(int i = 0; i < 24; i++)
            {
                if(PlayerManager.main.FocusedCharacter.Inventory[i].Nb == 0)
                {
                    spaceInInventory = true;
                    break;
                }
            }

            if(spaceInInventory && PlayerManager.main.FocusedCharacter.LeftHand != null)
            {
                PlayerManager.main.FocusedCharacter.UnEquip(PlayerManager.main.FocusedCharacter.LeftHand,false);
                AkSoundEngine.PostEvent("Unequip_Play", gameObject); // Play Unequip Sound
            }
        }
    }

    void UpdateRarity()
    {
        if (PlayerManager.main.FocusedCharacter.Body != null)
        {
            switch(PlayerManager.main.FocusedCharacter.Body.Rarity)
            {
                case RARITY.E_COMMON:
                    transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.white;
                    break;
                case RARITY.E_RARE:
                    transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.green;
                    break;
                case RARITY.E_EPIC:
                    transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.blue;
                    break;
                case RARITY.E_LEGENDARY:
                    transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.magenta;
                    break;
            }            
        }
        else
        {
            transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Color.white;
        }

        if (PlayerManager.main.FocusedCharacter.RightHand != null)
        {
            switch (PlayerManager.main.FocusedCharacter.RightHand.Rarity)
            {
                case RARITY.E_COMMON:
                    transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.white;
                    break;
                case RARITY.E_RARE:
                    transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.green;
                    break;
                case RARITY.E_EPIC:
                    transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.blue;
                    break;
                case RARITY.E_LEGENDARY:
                    transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.magenta;
                    break;
            }
        }
        else
        {
            transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.white;
        }

        if (PlayerManager.main.FocusedCharacter.LeftHand != null)
        {
            switch (PlayerManager.main.FocusedCharacter.LeftHand.Rarity)
            {
                case RARITY.E_COMMON:
                    transform.GetChild(2).GetChild(2).GetComponent<Image>().color = Color.white;
                    break;
                case RARITY.E_RARE:
                    transform.GetChild(2).GetChild(2).GetComponent<Image>().color = Color.green;
                    break;
                case RARITY.E_EPIC:
                    transform.GetChild(2).GetChild(2).GetComponent<Image>().color = Color.blue;
                    break;
                case RARITY.E_LEGENDARY:
                    transform.GetChild(2).GetChild(2).GetComponent<Image>().color = Color.magenta;
                    break;
            }
        }
        else
        {
            transform.GetChild(2).GetChild(2).GetComponent<Image>().color = Color.white;
        }
    }
}
