using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectStrength : ItemEffect
{
    private int strengthBonus = 0;
    private int strengthother = 0;

    private int strengthSave = 0;
    public int StrengthBonus { get => strengthBonus + strengthother; set => strengthBonus = value; }
    public int StrengthSave { get => strengthSave; set => strengthSave = value; }

    public override void ApplyEffect(Character _character)
    {
        _character.StrengthBonus += StrengthBonus;
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Strength : " + StrengthBonus + "</size></color>").AppendLine();
    }
    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        string write = "";
        if (StrengthBonus > 0) write += " + " + StrengthBonus;
        else if (StrengthBonus < 0) write += " - " + -1 * StrengthBonus;

        if (StrengthBonus != 0) _string.Append("<size=25><color=white>" + "Strength : " + write + "\n</size></color>");

        return _string;
    }

    public override void UndoEffect(Character _character)
    {
        _character.StrengthBonus -= StrengthBonus;

    }

    // Start is called before the first frame update
    override protected void Start()
    {
        gameObject.name = "Strength";
        if(strengthBonus ==0)
        {

            strengthBonus = Mathf.CeilToInt(0.75f * this.transform.parent.GetComponent<Equipable>().LevelRequired);

            ItemEffectStrength itEfSTR = gameObject.transform.parent.GetComponentInChildren<ItemEffectStrength>();
            if (this != itEfSTR)
            {
                itEfSTR.strengthother += StrengthBonus;
                Destroy(this.gameObject);
                return;
            }
        }

        base.Start();
    }
}
