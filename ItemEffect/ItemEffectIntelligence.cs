using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffectIntelligence : ItemEffect
{
    private int intelligenceBonus = 0;
    private int intelligenceOther = 0;

    public int IntelligenceBonus { get => intelligenceBonus + intelligenceOther; set => intelligenceBonus = value; }

    public override void ApplyEffect(Character _character)
    {

        _character.IntelligenceBonus += IntelligenceBonus;
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Intelligence : " + IntelligenceBonus + "</size></color>").AppendLine();
    }

    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        string write = "";
        if (IntelligenceBonus > 0) write += " + " + IntelligenceBonus;
        else if(IntelligenceBonus < 0) write += " - " + -1*IntelligenceBonus;

        if(IntelligenceBonus !=0)  _string.Append("<size=25><color=white>" + "Intelligence : " + write + "\n</size></color>");

        return _string;
    }

    public override void UndoEffect(Character _character)
    {
        _character.IntelligenceBonus -= IntelligenceBonus;

    }

    // Start is called before the first frame update
    override protected void Start()
    {
        gameObject.name = "Intelligence";
        if(intelligenceBonus == 0)
        {
            intelligenceBonus = Mathf.CeilToInt(0.75f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
            
            ItemEffectIntelligence itEfINT = gameObject.transform.parent.GetComponentInChildren<ItemEffectIntelligence>();
            if (this != itEfINT)
            {
                itEfINT.intelligenceOther += IntelligenceBonus;
                Destroy(this.gameObject);
                return;
            }
        }

        base.Start();
    }
}
