using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectConstitution : ItemEffect
{
    private int consBonus = 0;
    private int consOther = 0;

    public int ConsBonus { get => consBonus + consOther; set => consBonus = value; }

    public override void ApplyEffect(Character _character)
    {

        _character.ConstitutionBonus += ConsBonus;
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Constitution : " + ConsBonus + "</size></color>").AppendLine();
    }
    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        string write = "";
        if (ConsBonus > 0) write += " + " + ConsBonus;
        else if (ConsBonus < 0) write += " - " + -1 * ConsBonus;

        if (ConsBonus != 0) _string.Append("<size=25><color=white>" + "Constitution : " + write + "\n</size></color>");

        return _string;
    }


    public override void UndoEffect(Character _character)
    {
        _character.ConstitutionBonus -= ConsBonus;

    }

    // Start is called before the first frame update
    override protected void Start()
    {
            gameObject.name = "Constitution";
        if(consBonus == 0)
        {
            consBonus = Mathf.CeilToInt(0.4f * this.transform.parent.GetComponent<Equipable>().LevelRequired);

            ItemEffectConstitution itEfINT = gameObject.transform.parent.GetComponentInChildren<ItemEffectConstitution>();
            if (this != itEfINT)
            {
                itEfINT.consOther += ConsBonus;
                Destroy(this.gameObject);
                return;
            }

        }
        base.Start();

    }
}
