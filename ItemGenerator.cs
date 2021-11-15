using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemGenerator : MonoBehaviour
{
    private enum ObjectSelect
    {
        CONSOMMABLE,
        WEAPONFEE,
        ARMORFEE,
        WEAPONROGUE,
        ARMORROGUE,
        WEAPONTANK,
        SHIELD,
        ARMORTANK,
        COUNT,
    };

    [SerializeField] int nbObjectWanted = 0;
    [SerializeField]  RARITY rarityMax = RARITY.E_EPIC;

    [SerializeField] List<GameObject> objectMake;
    bool closed = false;

    public void Open()
    {
        if(!closed) // else destroy this ?
        {        
            for (int it = 0; it < nbObjectWanted; it++)
            {
                GameObject newItem;

                switch (SelectOneType())
                {
                    case ObjectSelect.WEAPONFEE:
                        newItem = WeaponFeeInitialize();
                        break;
                    case ObjectSelect.ARMORFEE:
                        newItem=ArmorFeeInitialize();
                        break;
                    
                    case ObjectSelect.ARMORROGUE:
                        newItem = ArmorRogueInitialize();
                        break;
                    case ObjectSelect.WEAPONROGUE:
                        newItem = WeaponRogueInitialize();
                        break;
                   
                    case ObjectSelect.CONSOMMABLE:
                        newItem = ConsumableInitialize();
                        break;
                    
                    case ObjectSelect.WEAPONTANK:
                        newItem = WeaponTankInitialize();

                        break;
                    case ObjectSelect.ARMORTANK:
                        newItem = ArmorTankInitialize();
                        break;
                    case ObjectSelect.SHIELD:
                        newItem = ShieldTankInitialize();
                        break;
                    
                    default:
                        newItem = new GameObject();
                        break;
                }
                objectMake.Add(newItem);
                newItem.transform.position = this.transform.position;
                if(newItem.name.Contains("(Clone)")) newItem.name = newItem.name.Remove(newItem.name.Length-7);

            }

            Container containerToAddObject;
            //Pickable pickableToAddObject;
            if (GetComponentInChildren<Container>())
            {
                containerToAddObject = GetComponentInChildren<Container>();
                containerToAddObject.Golds += Random.Range(50, 501);
                containerToAddObject.PlaceMax = objectMake.Count;
                containerToAddObject.MergeListObject(objectMake);
            }
            else if (transform.parent != null && transform.parent.GetComponentInChildren<Container>() != null)
            {
                containerToAddObject = transform.parent.GetComponentInChildren<Container>();
                containerToAddObject.Golds += Random.Range(50, 501);
                containerToAddObject.PlaceMax = objectMake.Count;
                containerToAddObject.MergeListObject(objectMake);
            }
            else if (transform.parent != null && transform.parent.GetComponentInChildren<Character>().IsMerchant)
            {                
                for (int i = 0; i < objectMake.Count; i++)
                    transform.parent.GetComponentInChildren<Character>().PutInInventory(objectMake[i].GetComponent<Pickable>());
            }
            closed = true;
        }
    }


    ObjectSelect SelectOneType()
    {
        return (ObjectSelect) Random.Range(0, (int)ObjectSelect.COUNT);
    }

    int GiveRarity()
    {
        int rand;
        rand = Random.Range(0, 101);
        if (rarityMax == RARITY.E_EPIC)
        {
            if (rand < 10)
            {
                rand = (int)RARITY.E_EPIC;
            }
            else if (rand < 30)
            {
                rand = (int)RARITY.E_LEGENDARY;
            }
            else if (rand < 60)
            {
                rand = (int)RARITY.E_RARE;
            }
            else
            {
                rand = (int)RARITY.E_COMMON;
            }
        }
        else if(rarityMax == RARITY.E_LEGENDARY)
        {
            if (rand < 20)
            {
                rand = (int)RARITY.E_LEGENDARY;
            }
            else if (rand < 60)
            {
                rand = (int)RARITY.E_RARE;
            }
            else
            {
                rand = (int)RARITY.E_COMMON;
            }

        }
        else if(rarityMax == RARITY.E_RARE)
        {
            if (rand < 70)
            {
                rand = (int)RARITY.E_RARE;
            }
            else
            {
                rand = (int)RARITY.E_COMMON;
            }

        }
        else
        {
            rand = (int)RARITY.E_COMMON;
        }
        return rand;
    }

    GameObject AddEffect(GameObject _obj, int _rarity )
    {
        int cmpt = _rarity;
        if ((_obj.GetComponent<Pickable>() as Weapon) && (_obj.GetComponent<Pickable>() as Weapon).ClassRequired == CLASSES.WIZARD) cmpt *= 2; 

        while (cmpt > 0)
        {
            int rand;
            rand = Random.Range(0, ObjManager.main.itemEffectNotSpecificly.Count);
            Instantiate(ObjManager.main.itemEffectNotSpecificly[rand], _obj.transform);
            cmpt--;
        }

            if((_obj.GetComponent<Entity>() as Weapon) != null && _rarity== (int)RARITY.E_EPIC)
            {
                int rand = Random.Range(0, ObjManager.main.itemEffectAlterationWeapon.Count);
                Instantiate(ObjManager.main.itemEffectAlterationWeapon[rand], _obj.transform);
            }
        return _obj;
    }
    GameObject WeaponFeeInitialize()
    {
        int rarity = GiveRarity();
        GameObject weaponFee = Instantiate(ObjManager.main.objectTypeWeaponFee[rarity]);

        Weapon wp;
        wp = weaponFee.GetComponent<Weapon>();
        int lvl = (int)Random.Range(PlayerManager.main.FocusedCharacter.Level - 2,
                    PlayerManager.main.FocusedCharacter.Level +2);
        lvl = Mathf.Clamp(lvl, 1, 20);
        wp.LevelRequired = lvl;


        switch (rarity)
        {
            case (int)RARITY.E_COMMON:
                weaponFee.name = "Stick";
                break;
            case (int)RARITY.E_RARE:
                weaponFee.name = "Wand";
                break;
            case (int)RARITY.E_LEGENDARY:
                weaponFee.name = "Staff";
                break;
            case (int)RARITY.E_EPIC:
                weaponFee.name = "Scepter";
                break;
        }


        return AddEffect(weaponFee, rarity);
    }

    GameObject ArmorFeeInitialize()
    {

        int rarity = GiveRarity();
        GameObject armorFee = Instantiate(ObjManager.main.objectTypeArmorFee[rarity]);
        
        Armor armor;
        armor = armorFee.GetComponent<Armor>();
        int lvl = (int)Random.Range(PlayerManager.main.FocusedCharacter.Level - 2,
                    PlayerManager.main.FocusedCharacter.Level + 2);
        lvl = Mathf.Clamp(lvl, 1, 20);
        armor.LevelRequired = lvl;

        switch (rarity)
        {
            case (int)RARITY.E_COMMON:
                armorFee.name = "cloak";
                break;
            case (int)RARITY.E_RARE:
                armorFee.name = "spectral cloak";
                break;
            case (int)RARITY.E_LEGENDARY:
                armorFee.name = "wizard robe";
                break;
            case (int)RARITY.E_EPIC:
                armorFee.name = "mystic robe";
                break;
        }

        return AddEffect(armorFee, rarity);
    }
    GameObject WeaponRogueInitialize()
    {
        int rarity = GiveRarity();
        GameObject weaponRogue = Instantiate(ObjManager.main.objectTypeWeaponRogue[rarity]);

        Weapon wp;
        wp = weaponRogue.GetComponent<Weapon>();
        int lvl = (int)Random.Range(PlayerManager.main.FocusedCharacter.Level - 2,
                    PlayerManager.main.FocusedCharacter.Level + 2);
        lvl = Mathf.Clamp(lvl, 1, 20);
        wp.LevelRequired = lvl;


        switch (rarity)
        {
            case (int)RARITY.E_COMMON:
                weaponRogue.name = "Knife";
                break;
            case (int)RARITY.E_RARE:
                weaponRogue.name = "Cinquedea";
                break;
            case (int)RARITY.E_LEGENDARY:
                weaponRogue.name = "Stilleto";
                break;
            case (int)RARITY.E_EPIC:
                weaponRogue.name = "Tucks";
                break;
        }


        return AddEffect(weaponRogue, rarity);
    }
    GameObject ArmorRogueInitialize()
    {
        int rarity = GiveRarity();
        GameObject armorRogue = Instantiate(ObjManager.main.objectTypeArmorRogue[rarity]);

        Armor armor;
        armor = armorRogue.GetComponent<Armor>();
        int lvl = (int)Random.Range(PlayerManager.main.FocusedCharacter.Level - 2,
                    PlayerManager.main.FocusedCharacter.Level + 2);
        lvl = Mathf.Clamp(lvl, 1, 20);
        armor.LevelRequired = lvl;

        switch (rarity)
        {
            case (int)RARITY.E_COMMON:
                armorRogue.name = "Reinforced Tunic";
                break;
            case (int)RARITY.E_RARE:
                armorRogue.name = "Padded armour";
                break;
            case (int)RARITY.E_LEGENDARY:
                armorRogue.name = "LEATHER ARMOUR";
                break;
            case (int)RARITY.E_EPIC:
                armorRogue.name = "Studded LEATHER ARMOUR";
                break;
        }


        return AddEffect(armorRogue, rarity);
    }
    GameObject WeaponTankInitialize()
    {
        int rarity = GiveRarity();
        GameObject weaponTank = Instantiate(ObjManager.main.objectTypeWeaponTank[rarity]);

        Weapon wp;
        wp = weaponTank.GetComponent<Weapon>();
        int lvl = (int)Random.Range(PlayerManager.main.FocusedCharacter.Level - 2,
                    PlayerManager.main.FocusedCharacter.Level + 2);
        lvl = Mathf.Clamp(lvl, 1, 20);
        wp.LevelRequired = lvl;


        switch (rarity)
        {
            case (int)RARITY.E_COMMON:
                weaponTank.name = "Club";
                break;
            case (int)RARITY.E_RARE:
                weaponTank.name = "Gada";
                break;
            case (int)RARITY.E_LEGENDARY:
                weaponTank.name = "WAR HAMMER";
                break;
            case (int)RARITY.E_EPIC:
                weaponTank.name = "FLAIL";
                break;
        }


        return AddEffect(weaponTank, rarity);
    }
    GameObject ShieldTankInitialize()
    {
        int rarity = GiveRarity();
        GameObject shieldTank = Instantiate(ObjManager.main.objectTypeWeaponShield[rarity]);

        Shield wp;
        wp = shieldTank.GetComponent<Shield>();
        int lvl = (int)Random.Range(PlayerManager.main.FocusedCharacter.Level - 2,
                    PlayerManager.main.FocusedCharacter.Level + 2);
        lvl = Mathf.Clamp(lvl, 1, 20);
        wp.LevelRequired = lvl;


        switch (rarity)
        {
            case (int)RARITY.E_COMMON:
                shieldTank.name = "Bocle";
                break;
            case (int)RARITY.E_RARE:
                shieldTank.name = "Aspis";
                break;
            case (int)RARITY.E_LEGENDARY:
                shieldTank.name = "Clipeus";
                break;
            case (int)RARITY.E_EPIC:
                shieldTank.name = "Pavois";
                break;
        }


        return AddEffect(shieldTank, rarity);
    }
    GameObject ArmorTankInitialize()
    {
        int rarity = GiveRarity();
        GameObject armorTank = Instantiate(ObjManager.main.objectTypeArmorTank[rarity]);

        Armor armor;
        armor = armorTank.GetComponent<Armor>();
        int lvl = (int)Random.Range(PlayerManager.main.FocusedCharacter.Level - 2,
                    PlayerManager.main.FocusedCharacter.Level + 2);
        lvl = Mathf.Clamp(lvl, 1, 20);
        armor.LevelRequired = lvl;

        switch (rarity)
        {
            case (int)RARITY.E_COMMON:
                armorTank.name = "Guette";
                break;
            case (int)RARITY.E_RARE:
                armorTank.name = "lamellaire wisby";
                break;
            case (int)RARITY.E_LEGENDARY:
                armorTank.name = "coat of plates";
                break;
            case (int)RARITY.E_EPIC:
                armorTank.name = "Plate Armor";
                break;
        }


        return AddEffect(armorTank, rarity);
    }

    GameObject ConsumableInitialize()
    {
        int rand = Random.Range(0, ObjManager.main.objectTypeConsommable.Length);
        GameObject consumable = Instantiate(ObjManager.main.objectTypeConsommable[rand]);
        consumable.GetComponent<Pickable>().Rarity = (RARITY)GiveRarity();
        Instantiate(ObjManager.main.itemEffectConsummable[Random.Range(0, ObjManager.main.itemEffectConsummable.Count)], consumable.transform);


        return consumable;
    }
}
