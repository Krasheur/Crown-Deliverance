using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class Shield : Equipable
{
    [Space(10)]
    [Header("Shield")]
    [SerializeField] protected float power;

    public float PowerBase { get => power; set => power = value; }

    protected override void Start()
    {
        base.Start();
        onGetDescription += Desciption;

        switch (rarity)
        {
            case RARITY.E_COMMON:
                if (this.value == 0) value = Random.Range(70, 100);
                break;
            case RARITY.E_RARE:
                if (this.value == 0) value = Random.Range(100, 130);
                break;
            case RARITY.E_LEGENDARY:
                if (this.value == 0) value = Random.Range(130, 170);
                break;
            case RARITY.E_EPIC:
                if (this.value == 0) value = Random.Range(170, 230);
                break;
        }


        if (this.levelRequired < 3)
        {
            power = 7;
        }
        else if (this.levelRequired < 5)
        {
            power = 16;
        }
        else if (this.levelRequired < 9)
        {
            power = 24;
        }
        else if (this.levelRequired < 12)
        {
            power = 32;
        }
        else if (this.levelRequired < 16)
        {
            power = 49;
        }
        else if (this.levelRequired < 20)
        {
            power = 60;
        }
        else if (this.levelRequired == 20)
        {
            power = 80;
        }



        this.OnEquip += ApplyEffect;
        this.OnUnequip += UndoEffect;


    }

    void Desciption(ref StringBuilder _descriptionConcat)
    {
        _descriptionConcat.Append("<size=25><color=white>" + "Protection : " + Power + "</size></color>").AppendLine();
        if (transform.GetComponentInChildren<ItemEffectModiferBaseStat>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectModiferBaseStat>().GetDescription());
        }
        if (transform.GetComponentInChildren<ItemEffectArmor>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectArmor>().GetDescription());
        }
        if (transform.GetComponentInChildren<ItemEffectStrength>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectStrength>().GetDescription());
        }
        if (transform.GetComponentInChildren<ItemEffectConstitution>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectConstitution>().GetDescription());
        }
        if (transform.GetComponentInChildren<ItemEffectDexterity>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectDexterity>().GetDescription());
        }
        if (transform.GetComponentInChildren<ItemEffectIntelligence>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectIntelligence>().GetDescription());
        }
        if (transform.GetComponentInChildren<ItemEffectModifierSecondaryStat>() != null)
        {
            _descriptionConcat.Append(transform.GetComponentInChildren<ItemEffectModifierSecondaryStat>().GetDescription());
        }
        //_descriptionConcat.Append("<size=18><color=yellow>                                                                " + "Value : " + value + " g</size></color>").AppendLine();       
    }


    public void ApplyEffect(Character _character)
    {
        _character.ArmorBonus += Power;
    }
    public void UndoEffect(Character _character)
    {

        _character.ArmorBonus -= Power;
    }

    public int Power
    {
        get
        {
            int pow = (int)(power * (1.0f + levelRequired * (levelRequired / 20.0f)));
            return pow;
        }
    }
}
