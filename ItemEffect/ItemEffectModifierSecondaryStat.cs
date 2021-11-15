using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class SecondaryStats
{

    public int mobilityBonus;
    public int criticalChanceBonus;
    public int dodgeBonus;
    public int criticalDamageBonus;
    public int totalPaBonus;

    public SecondaryStats Add(SecondaryStats bas2)
    {
        SecondaryStats bsStat = new SecondaryStats();
        bsStat.mobilityBonus = this.mobilityBonus + bas2.mobilityBonus;
        bsStat.criticalChanceBonus = this.criticalChanceBonus + bas2.criticalChanceBonus;
        bsStat.dodgeBonus = this.dodgeBonus + bas2.dodgeBonus;
        bsStat.criticalDamageBonus = this.criticalDamageBonus + bas2.criticalDamageBonus;
        bsStat.totalPaBonus = this.totalPaBonus + bas2.totalPaBonus;
        return bsStat;
    }
}

public class ItemEffectModifierSecondaryStat : ItemEffect
{


    SecondaryStats statsUp = new SecondaryStats();
    SecondaryStats statsUpOther = new SecondaryStats();

    public SecondaryStats StatsUp { get => statsUp.Add(statsUpOther); set => statsUp = value; }

    public override void ApplyEffect(Character _character)
    {
        if (StatsUp.mobilityBonus != 0) _character.MobilityBonus += StatsUp.mobilityBonus;
        if (StatsUp.criticalChanceBonus != 0) _character.CriticalChanceBonus += StatsUp.criticalChanceBonus;
        if (StatsUp.dodgeBonus != 0) _character.DodgeBonus += StatsUp.dodgeBonus;
        if (StatsUp.criticalDamageBonus != 0) _character.CriticalDamageBonus += StatsUp.criticalDamageBonus;
        if (StatsUp.totalPaBonus != 0)  _character.TotalPaBonus += StatsUp.totalPaBonus;
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Dexterity : " + "" + "</size></color>").AppendLine();
    }

    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();

        if (StatsUp.mobilityBonus != 0)
        {
            string write = "";
            if (StatsUp.mobilityBonus > 0) write += " + " + StatsUp.mobilityBonus;
            else if (StatsUp.mobilityBonus < 0) write += " - " + -1 * StatsUp.mobilityBonus;

            _string.Append("<size=25><color=white>" + "Mobility : " + write + "</size></color>").AppendLine();

        }
        if (StatsUp.criticalChanceBonus != 0)
        {
            string write = "";
            if (StatsUp.criticalChanceBonus > 0) write += " + " + StatsUp.criticalChanceBonus;
            else if (StatsUp.criticalChanceBonus < 0) write += " - " + -1 * StatsUp.criticalChanceBonus;

            _string.Append("<size=25><color=white>" + "Critical Chance : " + write + "</size></color>").AppendLine();
        }
        if (StatsUp.dodgeBonus != 0)
        {
            string write = "";
            if (StatsUp.dodgeBonus > 0) write += " + " + StatsUp.dodgeBonus;
            else if (StatsUp.dodgeBonus < 0) write += " - " + -1 * StatsUp.dodgeBonus;

            _string.Append("<size=25><color=white>" + "Dodge : " + write + "</size></color>").AppendLine();
        }
        if (StatsUp.criticalDamageBonus != 0)
        {
            string write = "";
            if (StatsUp.criticalDamageBonus > 0) write += " + " + StatsUp.criticalDamageBonus;
            else if (StatsUp.criticalDamageBonus < 0) write += " - " + -1 * StatsUp.criticalDamageBonus;

            _string.Append("<size=25><color=white>" + "Critical Damage : " + write + "</size></color>").AppendLine();
        }
        if (StatsUp.totalPaBonus != 0)
        {
            string write = "";
            if (StatsUp.totalPaBonus > 0) write += " + " + StatsUp.totalPaBonus;
            else if (StatsUp.totalPaBonus < 0) write += " - " + -1 * StatsUp.totalPaBonus;

            _string.Append("<size=25><color=white>" + "PA  : " + write + "</size></color>").AppendLine();

        }

        return _string;
    }

    public override void UndoEffect(Character _character)
    {
        if (StatsUp.mobilityBonus != 0) _character.MobilityBonus -= StatsUp.mobilityBonus;
        if (StatsUp.criticalChanceBonus != 0) _character.CriticalChanceBonus -= StatsUp.criticalChanceBonus;
        if (StatsUp.dodgeBonus != 0) _character.DodgeBonus -= StatsUp.dodgeBonus;
        if (StatsUp.criticalDamageBonus != 0) _character.CriticalDamageBonus -= StatsUp.criticalDamageBonus;
        if (StatsUp.totalPaBonus != 0) _character.TotalPaBonus -= StatsUp.totalPaBonus;
    }
    override protected void Awake()
    {
        base.Awake();
        this.transform.parent.GetComponent<Equipable>().Value += Random.Range(10, 20) * this.transform.parent.GetComponent<Equipable>().LevelRequired;
    }


// Start is called before the first frame update
    override protected void Start()
    {
        gameObject.name = "ModifierSecondaryStat";
        if (   statsUp.mobilityBonus == 0
            && statsUp.criticalChanceBonus == 0
            && statsUp.dodgeBonus == 0
            && statsUp.criticalDamageBonus == 0
            && statsUp.totalPaBonus == 0
            )
        {

            switch (Random.Range(0, 5))
            {
                case 0:
                    statsUp.mobilityBonus = Mathf.CeilToInt(0.75f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
                    break;
                case 1:
                    statsUp.criticalChanceBonus = Mathf.CeilToInt(0.60f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
                    break;
                case 2:
                    statsUp.dodgeBonus = Mathf.CeilToInt(0.33f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
                    break;
                case 3:
                    statsUp.criticalDamageBonus  = this.transform.parent.GetComponent<Equipable>().LevelRequired * 3
                                    + Mathf.CeilToInt(0.5f * this.transform.parent.GetComponent<Equipable>().LevelRequired); 
                    break;
                case 4:
                    statsUp.totalPaBonus = (int)(0.1f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
                    break;

            }

            ItemEffectModifierSecondaryStat itEfDex = gameObject.transform.parent.GetComponentInChildren<ItemEffectModifierSecondaryStat>();
            if (this != itEfDex)
            {
                itEfDex.statsUpOther = StatsUp.Add(itEfDex.statsUpOther);
                Destroy(this.gameObject);
                return;
            }

        }

        base.Start();
    }



}
