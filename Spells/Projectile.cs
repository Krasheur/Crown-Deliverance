using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrajectoryType
{
    PARABOLIC,
    INSTANTANEOUS
}

public class Projectile : MonoBehaviour
{
    protected Entity target;
    protected Vector3 targetPosition;
    protected Vector3 startPosition;
    protected TargetingMethod targetingMethod;
    [SerializeField] TrajectoryType trajectory;
    [SerializeField] float speed;
    [SerializeField] float elevation;
    [SerializeField] int damage;
    [SerializeField] int heal;
    [SerializeField] List<Alteration> alterations;
    [SerializeField] bool automaticHit;
    [SerializeField] bool ignoreArmor;
    [SerializeField] bool ignoreHostility = true;
    [SerializeField] float impactAreaRadius;
    [SerializeField] GameObject impactFX;
    [SerializeField] bool ScaleImpactFX;
    [SerializeField] bool impactOnAllTargets;
    float distance = 0.0f;
    float progress = 0.0f;
    protected Entity emitter;
    bool hasHit = false;
    ParticleSystem[] particles;
    DamageStruct damageDescriptor;
    int criticalChanceBonus;
    int criticalDamageBonus;

    public Entity Target { get => target; set => target = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public Vector3 StartPosition { get => startPosition; set => startPosition = value; }
    public Entity Emitter { get => emitter; set => emitter = value; }
    public TrajectoryType Trajectory { get => trajectory; set => trajectory = value; }
    public float Elevation { get => elevation; set => elevation = value; }
    public TargetingMethod TargetingMethod { get => targetingMethod; set => targetingMethod = value; }
    public float ImpactAreaRadius { get => impactAreaRadius; set => impactAreaRadius = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Heal { get => heal; set => heal = value; }
    public DamageStruct DamageDescriptor { get => damageDescriptor; set => damageDescriptor = value; }
    public bool IgnoreArmor { get => ignoreArmor; set => ignoreArmor = value; }
    public bool AutomaticHit { get => automaticHit; set => automaticHit = value; }
    public List<Alteration> Alterations { get => alterations; set => alterations = value; }
    public bool IgnoreHostility { get => ignoreHostility; set => ignoreHostility = value; }
    public bool HasHit { get => hasHit; }
    public int CriticalChanceBonus { get => criticalChanceBonus; set => criticalChanceBonus = value; }
    public int CriticalDamageBonus { get => criticalDamageBonus; set => criticalDamageBonus = value; }
    public float Progress { get => progress; set => progress = value; }
    public float Distance { get => distance; }

    void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        StartPosition = transform.position;
        distance = Vector3.Distance(TargetPosition, StartPosition);
        particles = GetComponentsInChildren<ParticleSystem>();
        Progress = 0;
        hasHit = false;
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "DisableOnImpact")
            {
                transform.GetChild(i).gameObject.SetActive(true);
                break;
            }
        }
    }

    void Update()
    {
        if (HasHit) WaitForDestroy();
        if (HasHit || Trajectory == TrajectoryType.INSTANTANEOUS)
        {
            Progress = 1.0f;
        }
        else
        {
            Progress += (Time.deltaTime * speed) / (Distance + 0.00001f);
        }

        if (Trajectory == TrajectoryType.PARABOLIC)
        {
            Vector3 lastPos = transform.position;
            transform.position = (1.0f - Progress) * StartPosition + Progress * TargetPosition
                + Vector3.up * Mathf.Sin(Progress * Mathf.PI) * Distance * Elevation;
            transform.forward = (transform.position - lastPos).normalized;
        }

        if (Progress >= 1.0f)
        {
            transform.position = TargetPosition;
            if (!HasHit) Hit();
        }

    }

    void ApplyAlterations(Entity _entity)
    {
        if (IgnoreHostility)
        {
            for (int i = 0; i < Alterations.Count; i++)
            {
                Instantiate(Alterations[i], _entity.transform).Emitter = emitter;
            }
        }
        else
        {
            bool isAlly = false;
            Character emitterChar = emitter as Character;
            Character entityChar = _entity as Character;
            if (emitterChar && entityChar)
            {
                isAlly = (entityChar.Hostility == emitterChar.Hostility || entityChar.Hostility == CHARACTER_HOSTILITY.NEUTRAL);
            }
            for (int i = 0; i < Alterations.Count; i++)
            {
                if (Alterations[i].IsBonus == AlterationEffect.BOTH || isAlly == (Alterations[i].IsBonus == AlterationEffect.BONUS))
                {
                    Instantiate(Alterations[i], _entity.transform).Emitter = emitter;
                }
            }
        }
    }

    void Hit()
    {     
        hasHit = true;
        MeshRenderer meshRen = GetComponent<MeshRenderer>();
        if (meshRen)
        {
            meshRen.enabled = false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "DisableOnImpact")
            {
                transform.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }

        if ((targetingMethod & TargetingMethod.CHARACTER) == TargetingMethod.CHARACTER
            || (targetingMethod & TargetingMethod.ENTITY) == TargetingMethod.ENTITY)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, ImpactAreaRadius, Physics.AllLayers/*LayerMask.NameToLayer("Entity")*/, QueryTriggerInteraction.Ignore);
            if (colliders != null && colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    DamageStruct dmg = DamageDescriptor.Clone();
                    Entity tmpEntity = colliders[i].gameObject.GetComponent<Entity>();
                    Character character = (tmpEntity as Character);
                    Character emitterChar = emitter as Character;
                    dmg.touchAutomaticly = AutomaticHit;
                    dmg.percArmor = IgnoreArmor;
                    if (emitterChar)
                    {
                        dmg.criticalHit = (Random.Range(0, 101) <= (emitterChar.GetCriticalChance + CriticalChanceBonus)) ? emitterChar.GetCriticalDamage : dmg.criticalHit;
                        dmg.criticalHit += dmg.criticalHit > 0 ? CriticalDamageBonus : 0;
                    }
                    if (tmpEntity && !tmpEntity.IsDead && !Physics.Linecast(tmpEntity.transform.position, transform.position + (tmpEntity.transform.position - transform.position).normalized * 0.01f, (1 << LayerMask.NameToLayer("Environnement")) | (1)))
                    {
                        if (character)
                        {
                            if (!IgnoreHostility)
                            {
                                dmg.amountDamag = (emitterChar ? character.Hostility != emitterChar.Hostility && character.Hostility != CHARACTER_HOSTILITY.NEUTRAL : true) ? dmg.amountDamag : 0;
                                dmg.amountDamagBonus = (emitterChar ? character.Hostility != emitterChar.Hostility && character.Hostility != CHARACTER_HOSTILITY.NEUTRAL : true) ? dmg.amountDamagBonus : 0;
                                dmg.amountHealBonus = (emitterChar ? character.Hostility == emitterChar.Hostility || character.Hostility == CHARACTER_HOSTILITY.NEUTRAL : true) ? dmg.amountHealBonus : 0;
                                dmg.amountHeal = (emitterChar ? character.Hostility == emitterChar.Hostility || character.Hostility == CHARACTER_HOSTILITY.NEUTRAL : true) ? dmg.amountHeal : 0;
                                dmg.amountArmor = (emitterChar ? character.Hostility == emitterChar.Hostility || character.Hostility == CHARACTER_HOSTILITY.NEUTRAL : true) ? dmg.amountArmor : 0;
                            }
                            DamageStruct dmgReport = character.ChangePv(dmg);
                            emitter?.OnDealDamage?.Invoke(tmpEntity, ref dmgReport);
                            if (!dmgReport.dodged)
                            {
                                ApplyAlterations(character);
                                if (impactFX && impactOnAllTargets)
                                {
                                    GameObject obj = Instantiate(impactFX, tmpEntity.transform.position, impactFX.transform.rotation);
                                    if (ScaleImpactFX)
                                    {
                                        obj.transform.localScale = Vector3.one * impactAreaRadius;
                                    }
                                }
                            }
                        }
                        else if ((targetingMethod & TargetingMethod.ENTITY) == TargetingMethod.ENTITY)
                        {
                            DamageStruct dmgReport = tmpEntity.ChangePv(dmg);
                            emitter?.OnDealDamage?.Invoke(tmpEntity, ref dmgReport);
                            if (!dmgReport.dodged)
                            {
                                ApplyAlterations(tmpEntity);
                                if (impactFX && impactOnAllTargets)
                                {
                                    GameObject obj = Instantiate(impactFX, tmpEntity.transform.position, impactFX.transform.rotation);
                                    if (ScaleImpactFX)
                                    {
                                        obj.transform.localScale = Vector3.one * impactAreaRadius;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        if (impactFX && !impactOnAllTargets)
        {
            GameObject obj = Instantiate(impactFX, transform.position + impactFX.transform.position, impactFX.transform.rotation);
            if (ScaleImpactFX)
            {
                obj.transform.localScale = Vector3.one * impactAreaRadius;
            }
        }
    }

    void WaitForDestroy()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
            if (particles[i].particleCount > 0)
            {
                return;
            }
        }
        Destroy(gameObject);
    }
}
