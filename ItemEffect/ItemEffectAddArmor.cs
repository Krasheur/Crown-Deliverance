using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectAddArmor : ItemEffect
{
    private int armor = 5;

    public int Armor { get => armor; set => armor = value; }

    public override void ApplyEffect(Character _character)
    {
        _character.GiveArmor(Armor);
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Recover armor : " + Armor + "</size></color>").AppendLine();
    }
    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        _string.Append("<size=25><color=white>" + "Recover armor : " + Armor + "\n</size></color>");

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
                armor = 30;
                break;
            case RARITY.E_LEGENDARY:
                gameObject.transform.parent.name = "Big Potion";
                armor = 60;
                numPopo = 1;
                break;
            case RARITY.E_EPIC:
                gameObject.transform.parent.name = "Very Big Potion";
                armor = 120;
                numPopo = 2;
                break;
            case RARITY.E_UNIC:
                armor = 240;
                gameObject.transform.parent.name = "Special Potion";
                break;
            default:
                // commun popo
                gameObject.transform.parent.name = "Little Potion";
                armor = 15;
                break;

        }
        csm.Img = ObjManager.main.imgPopoConsommableArmor[numPopo];
        gameObject.transform.parent.name += " of Armor";
    }
    

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }
}
