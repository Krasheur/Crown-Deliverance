using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum SpellFlagsMask
{
    None,
    DealDamage = 1<<0,
    Heal = 1<<1,
    GiveArmor = 1<<2,
    AddBonus = 1<<3, 
    AddMalus = 1<<4, 
    CanTargetAllies = 1<<5,
    CanTargetEnemies = 1<<6,
    HealCaster = 1<<7,
    AddBonusToCaster = 1<<8,
    RemoveBonusToEnemy = 1<<9,
    RemoveMalusToAlly = 1<<10,
    TeleportEnemy = 1<<11,
    TeleportCaster = 1<<12,
    AreaOfImpact = 1<<13,
}



public class SpellHolder : MonoBehaviour
{
    [SerializeField] Spell spell;
    [SerializeField] int cooldown;
    [SerializeField] List<SpellAlteration> spellModifiers;
    [SerializeField] [EnumFlag] SpellFlagsMask flags;
    int costModifier;
    int turnsRemaining;
    float timer;
    int spellID;
    Character ownerChar;
    Entity ownerEnt;
    public int TurnsRemaining { get => turnsRemaining; set => turnsRemaining = value; }
    public Spell Spell { get => spell; set => spell = value; }
    public int Cooldown { get => cooldown; set => cooldown = value; }
    public int CostModifier { get => costModifier; set => costModifier = value; }
    public int Cost { get => spell.Cost + CostModifier; }
    public float Timer { get => timer; set => timer = value; }
    public List<SpellAlteration> SpellModifiers { get => spellModifiers; set => spellModifiers = value; }

    public StringBuilder Description
    {
        get
        {
            StringBuilder desc = new StringBuilder("<size=25>" + spell.Description);
            if (SpellModifiers.Count > 0)
            { 
                string descAdd = "\n\n<color=yellow>";
                for (int i = 0; i < SpellModifiers.Count; i++)
                {
                     descAdd += "   - " + (SpellModifiers[i] ? SpellModifiers[i].DescriptionShort : "") + "\n";
                }
                desc.Append(descAdd + "</color></size>");
            }
            return desc;
        }
    }

    public SpellFlagsMask Flags
    {
        get
        {
            SpellFlagsMask tmpFlags = flags;

            for (int i = 0; i < spellModifiers.Count; i++)
            {
                tmpFlags = tmpFlags | spellModifiers[i].Flags;
            }

            return tmpFlags;
        }
    }

    public int SpellID { get => spellID; }


    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        if (ownerChar && ownerChar.Animators.Length > 0)
        {
            bool found = false;
            for (int i = 0; i < 4; i++)
            {
                if (this == ownerChar.SpellList[i])
                {
                    found = true;
                    spellID = 1 + i;
                }
            }
            if (!found)
            {
                spellID = 0;
            }
        }
    }

    public SpellHolder Init(Entity _Owner)
    {
        ownerEnt = _Owner;
        ownerChar = _Owner as Character;
        if (ownerChar)
        {
            ownerChar.OnNewTurn += ReduceCooldown;
        }
        return this;
    }

    public void ReduceCooldown()
    {
        if (turnsRemaining > 0 && (ownerChar == null || ownerChar.State != CHARACTER_STATE.FREE))
        {
            if (--turnsRemaining == 0) timer = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (turnsRemaining > 0)
        {
            if ((ownerChar && ownerChar.State == CHARACTER_STATE.FREE) || ownerChar == null)
            {
                timer += Time.deltaTime;
                if (timer >= Fight.turnTime)
                {
                    timer -= Fight.turnTime;
                    if (--turnsRemaining == 0) timer = 0.0f;
                }
            }
        }
    }

    public Spell Activate(bool _showTrajectoryPreview = false)
    {
        //if((ownerChar.SpellCasted == null && ownerChar.Animators.Length == null))
        //{
        //    return null;
        //}      

        if (turnsRemaining == 0 && (!ownerChar || (ownerChar.SpellCasted == null && (ownerChar.Animators.Length == 0 || ownerChar.Animators[0].GetInteger("SpellCasted") == -1)  && ownerChar.CurrentPa >= spell.Cost)))
        {
            Spell newSpell = Instantiate(Spell, (ownerEnt != null) ? ownerEnt.transform : transform);
            newSpell.SpellHolder = this;
            newSpell.ShowTrajectoryPreview = _showTrajectoryPreview;
            if (ownerChar)
            {                
                ownerChar.SpellCasted = newSpell;

                if (ownerChar && ownerChar.Animators.Length > 0)
                {
                    for (int i = 0; i < ownerChar.Animators.Length; i++)
                    {
                        ownerChar.Animators[i].SetInteger("Channeling", SpellID);
                    }
                }
            }
            for (int i = 0; i < spellModifiers.Count; i++)
            {
                Instantiate(spellModifiers[i], newSpell.transform);
            }
            return newSpell;
        }
        return null;
    }

    public void CancelLaunch()
    {
        if (ownerChar)
        {
            for (int i = 0; i < ownerChar.Animators.Length; i++)
            {
                ownerChar.Animators[i].SetInteger("Channeling", -1);
            }
            ownerChar.SpellCasted = null;
        }
    }

    public void ConfirmLaunch()
    {
        turnsRemaining = cooldown;

        if (ownerChar/* && ownerChar.State == CHARACTER_STATE.FIGHT*/)
        {
            ownerChar.CurrentPa -= Cost;
        }
        ownerChar.SpellCasted = null;
    }

    private void OnDestroy()
    {
        if (ownerChar)
        {
            ownerChar.OnNewTurn -= ReduceCooldown;
        }
    }
}
