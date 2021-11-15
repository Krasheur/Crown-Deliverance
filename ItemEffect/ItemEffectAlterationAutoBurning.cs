using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;



public class ItemEffectAlterationAutoBurning : ItemEffect
{
    [SerializeField] AddAlteration spellAlterations;

    public override void ApplyEffect(Character _character)
    {
        _character.AutoAttack.SpellModifiers.Add(spellAlterations);
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append(spellAlterations.DescriptionFull);
    }
    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();
        _string.Append(spellAlterations.DescriptionFull);
        return _string;
    }

    public override void UndoEffect(Character _character)
    {
        _character.AutoAttack.SpellModifiers.Remove(spellAlterations);
    }

    override protected void Awake()
    {
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

    }
}
