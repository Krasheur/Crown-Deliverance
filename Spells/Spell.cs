using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStat
{
    STRENGTH,
    DEXTERITY,
    INTELLIGENCE,
    CONSTITUTION,
    NONE
}

public enum TargetingMethod
{
    UNCHANGED = 0,
    ENTITY = 1 << 0,
    CHARACTER = 1 << 1,
    POSITION = 1 << 2,
    POSITION_CHARACTERS = POSITION | CHARACTER,
    POSITION_ENTITIES = POSITION | ENTITY
}

public delegate void OnGetInt(ref int _value);
public delegate void OnGetBool(ref bool _value);
public delegate void OnGetFloat(ref float _value);
public delegate void OnGetProjectile(ref Projectile _projectile);
public delegate void OnFireProjectile(Projectile _projectile);
public delegate void OnSetTarget(Entity _ent, Vector3 _position);
public delegate void OnValidateTarget(Entity _ent, Vector3 _position, ref bool valid);

public class Spell : MonoBehaviour
{
    [SerializeField] protected Sprite thumbnail;
    [SerializeField] protected string description;
    [SerializeField] protected TargetingMethod targetingMethod;
    [SerializeField] protected int cost;
    [SerializeField] private float range;
    [SerializeField] private int targetNumberNeeded;
    [SerializeField] protected GameObject idleFXObject;
    [SerializeField] protected GameObject LaunchFXObject;
    [SerializeField] Projectile proj;
    [SerializeField] TrajectoryVisualiser trajVisuPrefab;
    [SerializeField] GameObject RangeVisualiser;
    [SerializeField] CharacterStat benefitsFrom = CharacterStat.DEXTERITY;
    [SerializeField] FirePointID idleFXPointID;
    [SerializeField] FirePointID firePointID;
    [SerializeField] bool rotateIdleFX;
    protected DamageStruct damageDescriptor;
    protected TrajectoryVisualiser aimVisualiser;
    protected int targetNumberCurrent = 0;
    protected bool interrupted = false;
    protected float interruption;
    protected Entity emitter;
    protected SpellHolder spellHolder;
    protected List<Entity> targetEntities = new List<Entity>();
    protected List<Vector3> targetPositions = new List<Vector3>();
    private OnFireProjectile onFireProjectile;
    bool showTrajectoryPreview = true;
    OnGetInt onGetDamageAmount;
    OnGetInt onGetHealAmount;
    OnGetFloat onGetImpactRadius;
    OnGetInt onGetTargetNeeded;
    OnGetBool onGetIgnoreArmor;
    OnGetBool onGetAutomaticHit;
    OnGetProjectile onGetProjectile;
    OnSetTarget onSetTarget;
    OnValidateTarget onValidateTarget;

    public Sprite Thumbnail { get => thumbnail; set => thumbnail = value; }
    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public int Cost { get => cost; set => cost = value; }
    public SpellHolder SpellHolder { get => spellHolder; set => spellHolder = value; }

    public Vector3 IdleFXPoint
    {
        get
        {
            if (emitter && spellHolder)
            {
                return emitter.FirePoints.GetFirePoint(idleFXPointID);
            }
            else
            {
                return transform.position;
            }
        }
    }

    public Vector3 FirePoint
    {
        get
        {
            if (emitter && spellHolder)
            {
                return emitter.FirePoints.GetFirePoint(firePointID);
            }
            else
            {
                return transform.position;
            }
        }
    }
    
    public Quaternion IdleFXRotation
    {
        get
        {
            if (emitter && spellHolder)
            {
                return emitter.FirePoints.GetFirePointRotation(idleFXPointID);
            }
            else
            {
                return transform.rotation;
            }
        }
    }

    public bool ShowTrajectoryPreview
    {
        get => showTrajectoryPreview;
        set
        {
            showTrajectoryPreview = value;
            if (AimVisualiser)
            {
                AimVisualiser.gameObject.SetActive(showTrajectoryPreview);
                RangeVisualiser.SetActive(showTrajectoryPreview || RangeVisualiser.activeSelf);
            }
        }
    }

    /////////////////////////
    public float Range
    {
        get => range;
        set
        {
            range = value;
            UpdateVisualiser();
        }
    }

    public OnFireProjectile OnFireProjectile { get => onFireProjectile; set => onFireProjectile = value; }
    public OnGetInt OnGetDamageAmount { get => onGetDamageAmount; set => onGetDamageAmount = value; }
    public OnGetFloat OnGetImpactRadius { get => onGetImpactRadius; set => onGetImpactRadius = value; }
    public OnGetInt OnGetHealAmount { get => onGetHealAmount; set => onGetHealAmount = value; }
    public OnGetBool OnGetIgnoreArmor { get => onGetIgnoreArmor; set => onGetIgnoreArmor = value; }
    public OnGetBool OnGetAutomaticHit { get => onGetAutomaticHit; set => onGetAutomaticHit = value; }
    public OnGetProjectile OnGetProjectile { get => onGetProjectile; set => onGetProjectile = value; }
    public OnGetInt OnGetTargetNeeded { get => onGetTargetNeeded; set => onGetTargetNeeded = value; }
    public OnSetTarget OnSetTarget { get => onSetTarget; set => onSetTarget = value; }
    public OnValidateTarget OnValidateTarget { get => onValidateTarget; set => onValidateTarget = value; }
    public Projectile Proj
    {
        get
        {
            Projectile prj = proj;
            OnGetProjectile?.Invoke(ref prj);
            return prj;
        }
    }
    public int Damage
    {
        get
        {
            int dmg = Proj.Damage;
            OnGetDamageAmount?.Invoke(ref dmg);
            return dmg;
        }
    }

    public float ImpactRadius
    {
        get
        {
            float radius = Proj.ImpactAreaRadius;
            OnGetImpactRadius?.Invoke(ref radius);
            return radius;
        }
    }
    public int Heal
    {
        get
        {
            int hl = Proj.Heal;
            OnGetHealAmount?.Invoke(ref hl);
            return hl;
        }
    }
    public bool IgnoreArmor
    {
        get
        {
            bool ignArm = Proj.IgnoreArmor;
            OnGetIgnoreArmor?.Invoke(ref ignArm);
            return ignArm;
        }
    }
    public bool AutomaticHit
    {
        get
        {
            bool autHit = Proj.AutomaticHit;
            OnGetAutomaticHit?.Invoke(ref autHit);
            return autHit;
        }
    }
    public int TargetNumberNeeded
    {
        get
        {
            int targNeed = targetNumberNeeded;
            OnGetTargetNeeded?.Invoke(ref targNeed);
            return targNeed;
        }
        set => targetNumberNeeded = value;
    }

    public TargetingMethod TargetingMethod { get => targetingMethod; set => targetingMethod = value; }
    public int TargetNumberCurrent { get => targetNumberCurrent; set => targetNumberCurrent = value; }
    public TrajectoryVisualiser AimVisualiser { get => aimVisualiser; set => aimVisualiser = value; }
    public Entity Emitter { get => emitter; set => emitter = value; }




    /////////////////////////

    private void Awake()
    {
        AimVisualiser = Instantiate(trajVisuPrefab, transform);
    }

    public bool GetAim(out Vector3 _aimAt)
    {
        _aimAt = AimVisualiser.position;
        return !interrupted;
    }

    DamageStruct ApplySpellCastingAbility()
    {
        Character character = Emitter as Character;
        int stat = 0;
        if (character)
        {
            switch (benefitsFrom)
            {
                case CharacterStat.DEXTERITY:
                    stat = character.GetDexterity;
                    break;
                case CharacterStat.STRENGTH:
                    stat = (character.GetStrength);
                    break;
                case CharacterStat.INTELLIGENCE:
                    stat = (character.GetIntelligence);
                    break;
                case CharacterStat.CONSTITUTION:
                    stat = (character.GetConstitution);
                    break;
                default:
                    stat = (0);
                    break;
            }
        }

        damageDescriptor = new DamageStruct();
        damageDescriptor.amountDamag = (Damage + (int)Mathf.Ceil((Damage * 0.07f) * (2*stat)));
        damageDescriptor.amountHeal = (Heal + (int)Mathf.Ceil((Heal * 0.07f) * (2*stat)));
        damageDescriptor.emitter = Emitter;
        damageDescriptor.percArmor = IgnoreArmor;
        damageDescriptor.touchAutomaticly = AutomaticHit;
        return damageDescriptor;
    }

    void UpdateVisualiser()
    {
        AimVisualiser.elevation = Proj.Elevation;
        AimVisualiser.radius = ImpactRadius;
        RangeVisualiser.transform.localScale = Vector3.one * Range * 2.0f;
    }

    protected virtual void Start()
    {
        // will instantiate on ui click to start the idle state of the spell and set the player's matching animation/state
        if (transform.parent)
        {
            Emitter = transform.parent.GetComponent<Entity>();
        }
        if (Emitter)
        {
            if (idleFXObject)
            {
                idleFXObject = Instantiate(idleFXObject, transform);
                idleFXObject.transform.position = IdleFXPoint;
            }
        }
        AimVisualiser.gameObject.SetActive(false);
        AimVisualiser.startPos = FirePoint;
        UpdateVisualiser();
        RangeVisualiser.SetActive(ShowTrajectoryPreview);
        if (Proj.Trajectory == TrajectoryType.INSTANTANEOUS)
        {
            AimVisualiser.elevation = 0;
        }
    }

    protected bool IsTrajectoryInterrupted(Entity _entity, Vector3 _position)
    {
        Vector3 targetPosition = ((TargetingMethod & TargetingMethod.POSITION) != TargetingMethod.POSITION) ? _entity.transform.position : _position;
        if (Proj.Trajectory == TrajectoryType.INSTANTANEOUS)
        {
            RaycastHit hit;
            interrupted = Physics.Raycast(FirePoint, (targetPosition - FirePoint).normalized, out hit,
                (targetPosition - FirePoint).magnitude - 0.01f, (1 << LayerMask.NameToLayer("Environnement")) | (1), QueryTriggerInteraction.Ignore);
            if (interrupted) interruption = (hit.point - FirePoint).magnitude / (targetPosition - FirePoint).magnitude;
            return interrupted;
        }
        else
        {
            Vector3 lastpos = FirePoint;
            Vector3 pos = FirePoint;
            float distance = Vector3.Distance(FirePoint, targetPosition);
            int stepNum = (int)distance;

            for (int i = 0; i < stepNum; i++)
            {
                RaycastHit hit;
                float progress = i / (float)stepNum;
                lastpos = pos;
                pos = (1.0f - progress) * FirePoint + progress * targetPosition
                + Vector3.up * Mathf.Sin(progress * Mathf.PI) * distance * Proj.Elevation;

                if (Physics.Raycast(lastpos, (pos - lastpos).normalized, out hit, (pos - lastpos).magnitude, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                {
                    Entity hitEntity = hit.collider.GetComponentInParent<Entity>();
                    float distanceHitTarget = Vector3.Distance(hit.point, targetPosition);
                    if (
                        (Emitter == null || hitEntity != Emitter) &&
                        (((TargetingMethod == TargetingMethod.ENTITY || TargetingMethod == TargetingMethod.CHARACTER) && hitEntity != _entity) ||
                        ((TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION && distanceHitTarget > 0.5f))
                       )
                    {
                        interruption = Vector2.Distance(new Vector2(hit.point.x, hit.point.z),
                            new Vector2(FirePoint.x, FirePoint.z)) /
                            Vector2.Distance(new Vector2(targetPosition.x, targetPosition.z),
                            new Vector2(FirePoint.x, FirePoint.z));
                        interrupted = true;
                        return true;
                    }
                }
            }

            interrupted = false;
            return false;
        }
    }

    public virtual bool ValidateTarget(Entity _entity, Vector3 _position)
    {
        if (TargetNumberCurrent >= TargetNumberNeeded || 
            ((emitter as Character) && (emitter as Character).Target && (emitter as Character).Target != _entity)) return false;
        bool valid = false;
        if (_entity || (TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION)
        {
            Vector3 targetPosition = ((TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION) ? _position : _entity.transform.position;
            AimVisualiser.transform.rotation = Quaternion.Euler(0, 0, 0);
            AimVisualiser.position = targetPosition;
            if (Vector3.Distance(transform.position, targetPosition) <= Range)
            {
                if (!IsTrajectoryInterrupted(_entity, _position))
                {
                    if ((TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION)
                    {
                        interrupted = false;
                        valid = true;
                    }
                    else if (!_entity.IsDead)
                    {
                        Character character = (_entity as Character);
                        if (TargetingMethod == TargetingMethod.ENTITY)
                        {
                            interrupted = false;
                            valid = true;
                        }
                        else if (TargetingMethod == TargetingMethod.CHARACTER && character)
                        {
                            interrupted = false;
                            valid = true;
                        }
                    }
                }
            }
        }
        //ShowTrajectoryPreview = false;
        /*if (!valid)*/ AimVisualiser.position = _position;
        onValidateTarget?.Invoke(_entity, _position, ref valid);
        interrupted = !valid;
        return valid;
    }

    public virtual bool SetTarget(Entity _entity, Vector3 _position)
    {
        if (ValidateTarget(_entity, _position) && TargetNumberCurrent < TargetNumberNeeded)
        {
            targetPositions.Add(_position);
            targetEntities.Add(_entity);
            Instantiate(AimVisualiser, transform).locked = 1;
            TargetNumberCurrent++;
            OnSetTarget?.Invoke(_entity, _position);
            UpdateVisualiser();
            return true;
        }
        return false;
    }

    void StartAnimation()
    {
        Character ownerChar = Emitter as Character;
        if (ownerChar && ownerChar.Animators.Length > 0)
        {
            for (int i = 0; i < ownerChar.Animators.Length; i++)
            {
                ownerChar.Animators[i].SetInteger("SpellCasted", SpellHolder.SpellID);
            }
        }
        else
        {
            Launch();
        }
    }

    protected virtual void Update()
    {
        if (idleFXObject)
        {
            idleFXObject.transform.position = IdleFXPoint;
            if (rotateIdleFX)
            {
                idleFXObject.transform.rotation = IdleFXRotation;
            }
        }

        AimVisualiser.interruption = (interrupted) ? interruption : 1.0f;
        if (TargetNumberCurrent >= TargetNumberNeeded)
        {
            ShowTrajectoryPreview = false;
            //Launch();
            StartAnimation();
        }
    }

    public virtual void Launch()
    {
        if (Emitter)
        {
            if (LaunchFXObject)
            {
                LaunchFXObject = Instantiate(LaunchFXObject, Emitter.transform);
                LaunchFXObject.transform.position = FirePoint;
            }
        }
        for (int i = 0; i < TargetNumberCurrent; i++)
        {
            Projectile newProj = Instantiate(Proj, FirePoint, transform.rotation);
            newProj.Target = (targetingMethod == TargetingMethod.POSITION) ? null : targetEntities[i];
            newProj.TargetPosition = ((TargetingMethod & TargetingMethod.POSITION) == TargetingMethod.POSITION) ? targetPositions[i] : newProj.Target.transform.position;
            newProj.Emitter = Emitter;
            newProj.TargetingMethod = TargetingMethod;
            newProj.DamageDescriptor = ApplySpellCastingAbility();
            newProj.ImpactAreaRadius = ImpactRadius;
            newProj.IgnoreArmor = IgnoreArmor;
            newProj.AutomaticHit = AutomaticHit;
            onFireProjectile?.Invoke(newProj);
        }
        
        spellHolder?.ConfirmLaunch();        

        if (Emitter) Destroy(idleFXObject);
        Destroy(gameObject);
    }

    public virtual void Cancel()
    {
        // undo initialisation and destroy the "spell"
        spellHolder?.CancelLaunch();
        Destroy(gameObject);
        if (Emitter)
        {
            Destroy(idleFXObject);
        }
    }
}
