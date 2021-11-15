using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlterationEffect
{
    MALUS,
    BONUS,
    BOTH
}

public abstract class Alteration : MonoBehaviour
{
    protected Entity owner;
    protected Entity emitter;
    [SerializeField] string displayName;
    [SerializeField] string description;
    [SerializeField] protected int duration;
    [SerializeField] protected int turnsRemaining;
    [SerializeField] protected int applyingChance;
    [SerializeField] protected AlterationEffect isBonus;
    [SerializeField] protected CharacterStat ScalingStat;

    public int BonusFromStat
    {
        get
        {
            int stat = 0;
            Character emitterChar = emitter as Character;
            if (emitterChar)
            {
                switch (ScalingStat)
                {
                    case CharacterStat.DEXTERITY:
                        stat = emitterChar.GetDexterity;
                        break;
                    case CharacterStat.STRENGTH:
                        stat = (emitterChar.GetStrength);
                        break;
                    case CharacterStat.INTELLIGENCE:
                        stat = (emitterChar.GetIntelligence);
                        break;
                    case CharacterStat.CONSTITUTION:
                        stat = (emitterChar.GetConstitution);
                        break;
                    default:
                        stat = (0);
                        break;
                }
            }
            return stat;
        }
    }
    public Entity Emitter { get => emitter; set => emitter = value; }
    public int Duration { get => duration; set => duration = value; }
    public int TurnsRemaining { get => turnsRemaining; set => turnsRemaining = value; }
    public AlterationEffect IsBonus { get => isBonus; }
    public int ApplyingChance { get => applyingChance; set => applyingChance = value; }
    public string DisplayName { get => displayName; set => displayName = value; }
    public string Description {
        get
        {
            string desc = description;
            return string.Format(desc, format);
        }
    }

    protected virtual string[] format { get => new string[1]{ "" }; }

    // Start is called before the first frame update
    virtual protected void Awake()
    {
        Alteration alreadyApplied = null;
        owner = transform.parent.GetComponent<Entity>();
        int rand = Random.Range(0, 100);
        TurnsRemaining = Duration;
        if (rand < applyingChance)
        {
            FeedBack.main.CreateNewText(transform.position, DisplayName, Color.white);
            for (int i = 0; i < owner.transform.childCount; i++)
            {
                Alteration alteration = owner.transform.GetChild(i).GetComponent<Alteration>();
                if (alteration && alteration.DisplayName == DisplayName && gameObject != alteration.gameObject)
                {
                    alreadyApplied = alteration;
                }
            }

            if (alreadyApplied)
            {
                alreadyApplied.TurnsRemaining = Duration;
                Destroy(gameObject);
            }
            else
            {
                owner.OnDealDamage += OnDealDamage;
                owner.OnTakeDamage += OnTakeDamage;
                owner.OnWillTakeDamage += OnWillTakeDamage;
                owner.OnDeath += OnDeath;
                owner.OnNewTurn += OnNewTurn;
                owner.OnEndTurn += OnEndTurn;
            }
        }
        else
        {
            FeedBack.main.CreateNewText(transform.position, DisplayName + " failed", Color.white);
            Destroy(gameObject);
        }
    }

    abstract public void OnDealDamage(Entity _entity, ref DamageStruct dmgReport);
    abstract public void OnTakeDamage(Entity _entity, ref DamageStruct dmg);
    abstract public void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg);
    abstract public void OnDeath();
    virtual public void OnNewTurn()
    {
        TurnsRemaining--;
        if (TurnsRemaining <= 0)
        {
            Kill();
        }
    }
    abstract public void OnEndTurn();

    virtual protected void Update()
    {
        if (owner.IsDead)
        {
            Kill();
        }
    }

    virtual public void Kill()
    {
        Destroy(gameObject);
        owner.OnDealDamage -= OnDealDamage;
        owner.OnTakeDamage -= OnTakeDamage;
        owner.OnWillTakeDamage -= OnWillTakeDamage;
        owner.OnDeath -= OnDeath;
        owner.OnNewTurn -= OnNewTurn;
        owner.OnEndTurn -= OnEndTurn;
        StopAllCoroutines();
    }
}
