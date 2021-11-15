using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AreaDamageApplier
{
    public Entity entity;
    public DamageArea area;
    public AreaDamageApplier(Entity _entity)
    {
        entity = _entity;
        entity.OnNewTurn += OnNewTurn; 
        //DealDamage();
    }

    public void OnNewTurn()
    {
        DealDamage();
    }

    public void DealDamage()
    {
        bool isAlly = false;
        Character emitterChar = area.Emitter as Character;
        Character entityChar = entity as Character;
        if (emitterChar && entityChar)
        {
            isAlly = (entityChar.Hostility == emitterChar.Hostility || entityChar.Hostility == CHARACTER_HOSTILITY.NEUTRAL);
        }

        DamageStruct dmg = new DamageStruct();
        dmg.emitter = area.Emitter;
        if (area.IgnoreHostility)
        {
            dmg.amountDamag = (int)(area.SpellDamage * (area.SpellDamageRatio / 100.0f));
            dmg.amountHeal = (int)(area.SpellDamage * (area.SpellHealRatio / 100.0f));
            dmg.amountArmor = (int)(area.SpellDamage * (area.SpellArmorRatio / 100.0f));
        }
        else
        {
            if (isAlly)
            {
                dmg.amountHeal = (int)(area.SpellDamage * (area.SpellHealRatio / 100.0f));
                dmg.amountArmor = (int)(area.SpellDamage * (area.SpellArmorRatio / 100.0f));
            }
            else
            {
                dmg.amountDamag = (int)(area.SpellDamage * (area.SpellDamageRatio / 100.0f));
            }
        }
        entity.ChangePv(dmg);
    }

    public void Kill()
    {
        if (entity)
        {
            entity.OnNewTurn -= OnNewTurn;
            entity = null;
        }
    }
}

public class DamageArea : MonoBehaviour
{
    //[SerializeField] Alteration[] alterations;
    [SerializeField] int duration;
    [SerializeField] int spellDamageRatio;
    [SerializeField] int spellHealRatio;
    [SerializeField] int spellArmorRatio;
    [SerializeField] bool ignoreHostility;
    int spellDamage;
    int turnsRemaining;
    List<AreaDamageApplier> DamageAppliers;
    Character emitter;
    Character sphereChar;

    public Character Emitter { get => emitter; set => emitter = value; }
    public int SpellDamageRatio { get => spellDamageRatio; set => spellDamageRatio = value; }
    public int SpellDamage { get => spellDamage; set => spellDamage = value; }
    public int SpellHealRatio { get => spellHealRatio; set => spellHealRatio = value; }
    public int SpellArmorRatio { get => spellArmorRatio; set => spellArmorRatio = value; }
    public bool IgnoreHostility { get => ignoreHostility; set => ignoreHostility = value; }

    private void Awake()
    {
        DamageAppliers = new List<AreaDamageApplier>();
    }

    void Start()
    {
        sphereChar = GetComponent<Character>();
        sphereChar.OnNewTurn += OnNewTurn;
        turnsRemaining = duration;

        if (emitter && emitter.CurrentFight)
        {
            emitter.CurrentFight.OnTriggerEnter(GetComponent<Collider>());
        }
    }

    void Update()
    {

    }

    void OnNewTurn()
    {
        turnsRemaining--;
        if (turnsRemaining <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity && DamageAppliers.Find(x => (x.entity == entity)) == null)
        {
            AreaDamageApplier newDamageApplier = new AreaDamageApplier(entity);
            newDamageApplier.area = this;
            DamageAppliers.Add(newDamageApplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity)
        {
            AreaDamageApplier damageApplier = DamageAppliers.Find(x => (x.entity == entity));
            if (damageApplier != null)
            {
                damageApplier.Kill();
                DamageAppliers.Remove(damageApplier);
            }
        }
    }

    private void OnDestroy()
    {
        sphereChar.PV = 0;

        if (sphereChar.CurrentFight)
        {
            sphereChar.CurrentFight.checkForDeath();
        }

        for (int i = 0; i < DamageAppliers.Count; i++)
        {
            DamageAppliers[i].Kill();
        }
        DamageAppliers.Clear();
    }
}
