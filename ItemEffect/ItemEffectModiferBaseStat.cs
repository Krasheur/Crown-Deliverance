using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;



public class BaseStats
{

    public int armor;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int strength;

    public BaseStats Add(BaseStats bas2) 
        {
             BaseStats bsStat =  new BaseStats();
             bsStat.intelligence = this.intelligence + bas2.intelligence;
             bsStat.armor = this.armor + bas2.armor;
             bsStat.strength = this.strength + bas2.strength;
             bsStat.constitution = this.constitution + bas2.constitution;
             bsStat.dexterity = this.dexterity + bas2.dexterity;
             return bsStat;
        }
}
public class ItemEffectModiferBaseStat : ItemEffect
{

    BaseStats statsUp = new BaseStats();
    BaseStats statsUpOther = new BaseStats();

    public BaseStats StatsUp { get => statsUp.Add(statsUpOther); set => statsUp = value; }

    public override void ApplyEffect(Character _character)
    {
        if(StatsUp.armor != 0)
        {
            _character.ArmorBonus += StatsUp.armor;
        }
        if(StatsUp.constitution != 0)
        {
            _character.ConstitutionBonus += StatsUp.constitution;
        }
        if(StatsUp.dexterity != 0)
        {
            _character.DexterityBonus += StatsUp.dexterity;
        }
        if(StatsUp.intelligence != 0)
        {
            _character.IntelligenceBonus += StatsUp.intelligence;
        }
        if(StatsUp.strength != 0)
        {
            _character.StrengthBonus += StatsUp.strength;
        }
    }

    public override void OnGetDescription(ref StringBuilder _string)
    {
        _string.Append("<size=25><color=white>" + "Dexterity : " + "" + "</size></color>").AppendLine();
    }

    public override StringBuilder GetDescription()
    {
        StringBuilder _string = new StringBuilder();

        if (StatsUp.armor != 0)
        {   
            string write = "";
            if (StatsUp.armor > 0) write += " + " + StatsUp.armor;
            else if (StatsUp.armor < 0) write += " - " + -1 * StatsUp.armor;

           _string.Append("<size=25><color=white>" + "Armor : " + write + "</size></color>").AppendLine();
            
        }   
        if (StatsUp.strength != 0)
        {
            string write = "";
            if (StatsUp.strength > 0) write += " + " + StatsUp.strength;
            else if (StatsUp.strength < 0) write += " - " + -1 * StatsUp.strength;

            _string.Append("<size=25><color=white>" + "Strength : " + write + "</size></color>").AppendLine();
        }
        if (StatsUp.constitution != 0)
        {
            string write = "";
            if (StatsUp.constitution > 0) write += " + " + StatsUp.constitution;
            else if (StatsUp.constitution < 0) write += " - " + -1 * StatsUp.constitution;

            _string.Append("<size=25><color=white>" + "Constitution : " + write + "</size></color>").AppendLine();
        }
        if (StatsUp.dexterity != 0)
        {
            string write = "";
            if (StatsUp.dexterity > 0) write += " + " + StatsUp.dexterity;
            else if (StatsUp.dexterity < 0) write += " - " + -1 * StatsUp.dexterity;

            _string.Append("<size=25><color=white>" + "Dexterity : " + write + "</size></color>").AppendLine();
        }
        if (StatsUp.intelligence != 0)
        {
            string write = "";
            if (StatsUp.intelligence > 0) write += " + " + StatsUp.intelligence;
            else if (StatsUp.intelligence < 0) write += " - " + -1 * StatsUp.intelligence;

           _string.Append("<size=25><color=white>" + "Intelligence : " + write + "</size></color>").AppendLine();

        }

        return _string;
    }

    public override void UndoEffect(Character _character)
    {
        if (StatsUp.armor != 0)
        {
            _character.ArmorBonus -= StatsUp.armor;
        }
        if (StatsUp.constitution != 0)
        {
            _character.ConstitutionBonus -= StatsUp.constitution;

        }
        if (StatsUp.dexterity != 0)
        {
            _character.DexterityBonus -= StatsUp.dexterity;

        }
        if (StatsUp.intelligence != 0)
        {

            _character.IntelligenceBonus -= StatsUp.intelligence;
        }
        if (StatsUp.strength != 0)
        {

            _character.StrengthBonus -= StatsUp.strength;
        }

    }

    override protected void Awake()
    {
        base.Awake();
        this.transform.parent.GetComponent<Equipable>().Value += Random.Range(15, 30) * this.transform.parent.GetComponent<Equipable>().LevelRequired;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        gameObject.name = "ModifierBaseStat";
        if(    statsUp.armor == 0
            && statsUp.constitution == 0
            && statsUp.dexterity == 0
            && statsUp.intelligence == 0
            && statsUp.strength == 0
            )
        {

            switch(Random.Range(0, 5))
            {
                case 0:
                    statsUp.dexterity = Mathf.CeilToInt(0.75f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
                    break;
                case 1:
                    statsUp.armor = this.transform.parent.GetComponent<Equipable>().LevelRequired *2
                                    + Mathf.CeilToInt(0.3f * this.transform.parent.GetComponent<Equipable>().LevelRequired); ;
                    break;
                case 2:
                    statsUp.constitution = Mathf.CeilToInt(0.4f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
                    break;
                case 3:
                    statsUp.intelligence = Mathf.CeilToInt(0.75f * this.transform.parent.GetComponent<Equipable>().LevelRequired); ;
                    break;
                case 4:
                    statsUp.strength = Mathf.CeilToInt(0.75f * this.transform.parent.GetComponent<Equipable>().LevelRequired);
                    break;

            }
    
            ItemEffectModiferBaseStat itEfDex = gameObject.transform.parent.GetComponentInChildren<ItemEffectModiferBaseStat>();
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
