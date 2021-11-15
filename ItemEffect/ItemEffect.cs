using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : MonoBehaviour
{
    // Start is called before the first frame update
    virtual protected void Awake()
    {

    }
    virtual protected void Start()
    {
        Pickable item = transform.parent.GetComponent<Pickable>();

        if ((item as Equipable) != null)
        {
            Equipable equipable = (item as Equipable);
            equipable.OnEquip += ApplyEffect;
            equipable.OnUnequip += UndoEffect;
            //item.OnGetDescription += OnGetDescription;
        }
        else if ((item as Consumable) != null)
        {
            Consumable consumable = (item as Consumable);
            consumable.OnUse += ApplyEffect;
            //item.OnGetDescription += OnGetDescription;
        }
    }

    abstract public void OnGetDescription(ref StringBuilder _string);
    abstract public StringBuilder GetDescription();

    abstract public void ApplyEffect(Character _character);
    abstract public void UndoEffect(Character _character);
}
