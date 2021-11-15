using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectArmor : ItemEffect
{
    private int armorBonus = 0;
    private int armorother = 0;
    private int armorSave = 0; 
    public int ArmorBonus { get => armorBonus + armorother; set => armorBonus = value; }
    public int ArmorSave { get => armorSave; set => armorSave = value; }

    public override void ApplyEffect(Character _character)
    {
        _character.ArmorBonus += ArmorBonus;
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Armor : " + ArmorBonus + "</size></color>").AppendLine();
    }

    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        string write = "";
        if (ArmorBonus > 0) write += " + " + ArmorBonus;
        else if (ArmorBonus < 0) write += " - " + -1 * ArmorBonus;

        if (ArmorBonus != 0) _string.Append("<size=25><color=white>" + "Armor : " + write + "\n</size></color>");
        return _string;
    }


    public override void UndoEffect(Character _character)
    {
        _character.ArmorBonus -= ArmorBonus;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        gameObject.name = "Armor";
        if(armorBonus ==0)
        {
            armorBonus = (armorBonus + this.transform.parent.GetComponent<Equipable>().LevelRequired)
                 * Mathf.CeilToInt(0.3f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
    
            ItemEffectArmor itEfAr = gameObject.transform.parent.GetComponentInChildren<ItemEffectArmor>();
            if (this != itEfAr)
            {
                itEfAr.armorother += ArmorBonus;
                Destroy(this.gameObject);
                return;
            }
        }

        base.Start();
    }
}
