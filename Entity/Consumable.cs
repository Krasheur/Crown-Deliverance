using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Pickable
{
    [Space(10)]
    [Header("Consumable")]
    protected int utilisationNB;
    protected OnCharacter onUse;

    protected override void Awake()
    {
        base.Awake();
        switch (rarity)
        {
            case RARITY.E_COMMON:
                value = 10;
                break;
            case RARITY.E_RARE:
                 value = 30;
                break;
            case RARITY.E_LEGENDARY:
                value = 55;
                break;
            case RARITY.E_EPIC:
                 value = 75;
                break;
        }

    }

    public OnCharacter OnUse { get => onUse; set => onUse = value; }

    public int UtilisationNB { get => utilisationNB; set => utilisationNB = value; }
}
