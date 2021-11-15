using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectHeal : ItemEffect
{
    private int heal = 5;

    public int Heal { get => heal; set => heal = value; }
    public override void ApplyEffect(Character _character)
    {
        DamageStruct dmgStruct = new DamageStruct();
        dmgStruct.amountHeal = heal;
        _character.ChangePv(dmgStruct);
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Heal : " + heal + "</size></color>").AppendLine();
    }

    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        _string.Append("<size=25><color=white>" + "Heal : " + heal + "\n</size></color>");

        return _string;
    }

    public override void UndoEffect(Character _character)
    {


    }

    override protected void Awake()
    {
        base.Awake();
        Consumable csm = transform.parent.GetComponent<Consumable>();
        int numPopo = 0;
        switch (csm.Rarity)
        {

            case RARITY.E_RARE:
                gameObject.transform.parent.name = "Potion";
                heal = 20;
                break;
            case RARITY.E_LEGENDARY:
                gameObject.transform.parent.name = "Big Potion";
                heal = 40;
                numPopo = 1;
                break;
            case RARITY.E_EPIC:
                gameObject.transform.parent.name = "Very Big Potion";
                heal = 80;
                numPopo = 2;
                break;
            case RARITY.E_UNIC:
                gameObject.transform.parent.name = "Special Potion";
                heal = 160;
                break;
            default:
                // commun popo
                heal = 10;
                gameObject.transform.parent.name = "Little Potion";
                break;

        }
        csm.Img = ObjManager.main.imgPopoConsommableHeal[numPopo];
        gameObject.transform.parent.name +=  " of Heal";
    }
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }
}
