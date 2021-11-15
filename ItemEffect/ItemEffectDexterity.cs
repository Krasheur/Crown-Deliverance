using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectDexterity : ItemEffect
{
    private int dextBonus = 0;
    private int dextother = 0;
    

    public int DextBonus { get => dextBonus+ dextother; set => dextBonus = value; }

    public override void ApplyEffect(Character _character)
    {

        _character.DexterityBonus += DextBonus;
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Dexterity : " + DextBonus + "</size></color>").AppendLine();
    }

    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();

        string write = "";
        if (DextBonus > 0) write += " + " + DextBonus;
        else if (DextBonus < 0) write += " - " + -1 * DextBonus;

        if (DextBonus != 0) _string.Append("<size=25><color=white>" + "Dexterity : " + write + "\n</size></color>");

        return _string;
    }

    public override void UndoEffect(Character _character)
    {
        _character.DexterityBonus -= DextBonus;

    }

    // Start is called before the first frame update
    override protected void Start()
    {
        gameObject.name = "Dexterity";
        if(dextBonus ==0)
        {
            dextBonus = Mathf.CeilToInt( 0.75f * this.transform.parent.GetComponent<Equipable>().LevelRequired);

            ItemEffectDexterity itEfDex = gameObject.transform.parent.GetComponentInChildren<ItemEffectDexterity>();
            if (this != itEfDex)
            {
                itEfDex.dextother += DextBonus;
                Destroy(this.gameObject);
                return;
            }
        }

        base.Start();
    }
}
