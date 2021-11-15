using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Equipable : Pickable
{
    [Space(10)]
    [Header("Equipable")]
    [SerializeField] protected int levelRequired;
    [SerializeField] protected CLASSES classRequired;

    OnCharacter onEquip;
    OnCharacter onUnequip;

    public int LevelRequired { get => levelRequired; set => levelRequired = value; }
    public CLASSES ClassRequired { get => classRequired; set => classRequired = value; }
    public OnCharacter OnEquip { get => onEquip; set => onEquip = value; }
    public OnCharacter OnUnequip { get => onUnequip; set => onUnequip = value; }

    protected override void Awake()
    {
        base.Awake();
        switch (rarity)
        {
            case RARITY.E_COMMON:
                if (this.value == 0) value = Random.Range(100 + 20 * levelRequired, 140 + 20 * levelRequired);
                break;
            case RARITY.E_RARE:
                if (this.value == 0) value = Random.Range(160 + 20 * levelRequired, 200 + 20 * levelRequired);
                break;
            case RARITY.E_LEGENDARY:
                if (this.value == 0) value = Random.Range(230 + 20 * levelRequired, 260 + 20 * levelRequired);
                break;
            case RARITY.E_EPIC:
                if (this.value == 0) value = Random.Range(400 + 20 * levelRequired, 450 + 20 * levelRequired);
                break;
        }

    }

    protected override void Start()
    {
        base.Start();
        this.isKillable = false;
        onGetDescription += Desciption;
    }

    void Desciption(ref StringBuilder _descriptionConcat)
    {
        _descriptionConcat.Append("<size=25><color=white>").Append( "Level : " + levelRequired ).AppendLine();
        _descriptionConcat.Append( "Class required : " + classRequired + "\n</size></color>").AppendLine();         
    }
}
