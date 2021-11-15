using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class Weapon : Equipable
{
    [Space(10)]
    [Header("Weapon")]
    [SerializeField] protected float power;

    public float PowerBase { get => power; set => power = value; }



    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        base.Start();
        onGetDescription += Desciption;
        if(this.levelRequired <3)
        {
            power = 5;
        }
        else if (this.levelRequired < 5)
        {
            power = 10;
        }
        else if (this.levelRequired < 9)
        {
            power = 20;
        }
        else if (this.levelRequired < 12)
        {
            power = 25;
        }
        else if (this.levelRequired < 16)
        {
            power = 45;
        }
        else if (this.levelRequired < 20)
        {
            power = 60;
        }
        else if (this.levelRequired == 20)
        {
            power = 80;
        }


    }

    void Desciption(ref StringBuilder _descriptionConcat)
    {
        _descriptionConcat.Append("<size=25><color=white>" + "Damage : " + Power.x + " - " + Power.y + "\n</size></color>").AppendLine();
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
        ItemEffect[] itEffect = transform.GetComponentsInChildren<ItemEffect>();
        for (int i = 0; i < itEffect.Length; i++)
        {
            if (   (itEffect[i] as ItemEffectModiferBaseStat) == null
                && (itEffect[i] as ItemEffectModifierSecondaryStat) == null
                && (itEffect[i] as ItemEffectIntelligence) == null
                && (itEffect[i] as ItemEffectDexterity) == null
                && (itEffect[i] as ItemEffectConstitution) == null
                && (itEffect[i] as ItemEffectStrength) == null
                && (itEffect[i] as ItemEffectArmor) == null
               )
            {
                _descriptionConcat.Append(itEffect[i].GetDescription());

            }
        }
        //_descriptionConcat.Append("<size=18><color=yellow>                                                                " + "Value : " + value + " g</size></color>").AppendLine();
    }

    public Vector2Int Power {
        get
        {
            int pow = (int)(power * (1.0f + levelRequired * (levelRequired / 20.0f)));
            return new Vector2Int((int)(pow * 0.9f), (int)(pow * 1.1f));
        } 
    }

}
