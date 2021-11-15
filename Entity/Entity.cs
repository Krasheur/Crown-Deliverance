using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public delegate void OnEntity(Entity _entity, ref DamageStruct _dmgStruct);
public delegate void OnEntities(Entity[] _entities = null);
public delegate void OnEvent();
public delegate void OnString(ref StringBuilder _description);
public delegate void OnCharacter(Character _character);


public struct DamageStruct
{
    public int amountDamag;
    public int amountDamagBonus;
    public int amountArmor;
    public int amountHeal;
    public int amountHealBonus;
    public int criticalHit;
    public bool touchAutomaticly;
    public bool percArmor;
    public bool dodged;
    public Entity emitter;


    public int amountDamagToHp;
    public int amountDamagToArmor;

    public DamageStruct(int _th=0)
    {
        amountArmor = 0;
        amountHeal = 0;
        amountHealBonus = 0;
        amountDamag = 0;
        amountDamagBonus = 0;
        criticalHit = 0;
        touchAutomaticly = false;
        percArmor = false;
        dodged = false;
        emitter = null;

        amountDamagToArmor = 0;
        amountDamagToHp = 0;
    }
    public DamageStruct Clone()
    {
        DamageStruct clone = new DamageStruct();
        clone.amountArmor = this.amountArmor;
        clone.amountHeal = this.amountHeal;
        clone.amountHealBonus = this.amountHealBonus;
        clone.amountDamag = this.amountDamag;
        clone.amountDamagBonus = this.amountDamagBonus;
        clone.criticalHit = this.criticalHit;
        clone.touchAutomaticly = this.touchAutomaticly;
        clone.percArmor = this.percArmor;
        clone.dodged = this.dodged;
        clone.emitter = this.emitter;
        clone.amountDamagToArmor = this.amountDamagToArmor;
        clone.amountDamagToHp = this.amountDamagToHp;
        return clone;
    }
};


public class Entity : MonoBehaviour
{
    [Space(10)]
    [Header("Entity")]
    [SerializeField] protected int pvMax;
    [SerializeField] protected bool isKillable = true;
    [SerializeField] protected int pv;
    [SerializeField] protected int pvBase;

    // Accessor
    public int PV { get => pv; set => pv = value; }
    public int PVBase { get => pvBase; set => pvBase = value; }
    public bool IsKillable { get => isKillable; set => isKillable = value; }
    public bool IsDead { get { return pv <= 0; } }
    public string Name { get => name; set => name = value; }
    public int PvMax { get => pvMax; set => pvMax = Mathf.Max(value,1); }
    public OnEntity OnTakeDamage { get => onTakeDamage; set => onTakeDamage = value; }
    public OnEntity OnWillTakeDamage { get => onWillTakeDamage; set => onWillTakeDamage = value; }
    public OnEntity OnDealDamage { get => onDealDamage; set => onDealDamage = value; }
    public OnEvent OnDeath { get => onDeath; set => onDeath = value; }
    public OnEvent OnNewTurn { get => onNewTurn; set => onNewTurn = value; }
    public OnEvent OnEndTurn { get => onEndTurn; set => onEndTurn = value; }
    public FirePoints FirePoints
    {
        get
        {
            if (!firePoints)
            {
                firePoints = gameObject.AddComponent<FirePoints>();
            }
            return firePoints;
        }
    }

    FirePoints firePoints;

    // Delegates

    private OnEntity onTakeDamage;
    private OnEntity onWillTakeDamage;
    private OnEntity onDealDamage;
    private OnEvent onDeath;
    private OnEvent onNewTurn;
    private OnEvent onEndTurn;

    // Fct

    virtual public DamageStruct ChangePv(DamageStruct _damageStruct)
    {
        if (OnWillTakeDamage != null) OnWillTakeDamage(_damageStruct.emitter, ref _damageStruct);

        DamageStruct dmgFeedBack = _damageStruct.Clone();
        dmgFeedBack.amountDamagToArmor = 0;
        dmgFeedBack.amountDamagToHp = 0;
        dmgFeedBack.amountHeal += dmgFeedBack.amountHealBonus;
        dmgFeedBack.amountArmor = 0;
        if (dmgFeedBack.amountHeal > 0)
        {
            PV = Mathf.Min(PV + dmgFeedBack.amountHeal, PvMax);

            FeedBack.main.FeedBackHit(transform.position, dmgFeedBack);
        }
        if (_damageStruct.amountDamag > 0)
        {
             if (_damageStruct.criticalHit > 0)
             {
                 _damageStruct.criticalHit = Mathf.Max(_damageStruct.criticalHit, 0);
                 _damageStruct.amountDamag = Mathf.Max(((int)((float)_damageStruct.amountDamag * ((float)_damageStruct.criticalHit / 100))) + _damageStruct.amountDamagBonus, 0);
             }

             if (_damageStruct.percArmor)
             {
                 PV = (!isKillable) ? Mathf.Max(1, PV - (_damageStruct.amountDamag + _damageStruct.amountDamagBonus))
                                     : Mathf.Max(0, PV - (_damageStruct.amountDamag + _damageStruct.amountDamagBonus));

                 dmgFeedBack.amountDamagToHp = _damageStruct.amountDamag + _damageStruct.amountDamagBonus;
                 dmgFeedBack.amountDamagToArmor = 0;
                 FeedBack.main.FeedBackHit(transform.position, dmgFeedBack);
                 dmgFeedBack.amountDamagToHp = 0;
             }
             else
             {

                     dmgFeedBack.amountDamagToHp = 0;
                     dmgFeedBack.amountDamagToArmor = 0;
                     PV = (!isKillable) ? Mathf.Max(1, PV - _damageStruct.amountDamag + _damageStruct.amountDamagBonus) : Mathf.Max(0, PV - _damageStruct.amountDamag + _damageStruct.amountDamagBonus);
                     dmgFeedBack.amountDamagToHp = (_damageStruct.amountDamag + _damageStruct.amountDamagBonus);
                     FeedBack.main.FeedBackHit(transform.position, dmgFeedBack);
                     dmgFeedBack.amountDamagToHp = 0;

                     dmgFeedBack.amountDamagToArmor = (_damageStruct.amountDamag + _damageStruct.amountDamagBonus);
                     FeedBack.main.FeedBackHit(transform.position, dmgFeedBack);
             }

             if (OnTakeDamage != null) OnTakeDamage(_damageStruct.emitter, ref _damageStruct);
        }

        if (IsDead)
        {
            NavMeshObstacle nvMeshObs;            

            if(TryGetComponent<NavMeshObstacle>(out nvMeshObs))
            {
                nvMeshObs.carveOnlyStationary = false;
                Destroy(nvMeshObs);
            }

                Vector3 flop = this.gameObject.transform.localScale;
                flop.y = 0.5f;
                this.gameObject.transform.localScale = flop;
                if (GetComponent<BoxCollider>())
                {                    
                    GetComponent<BoxCollider>().enabled = false;
                }
                this.enabled = false;
                Destroy(this.gameObject);

        }

        _damageStruct.emitter = this;
        return _damageStruct;
    }

    private SkinnedMeshRenderer GetSkinnedMeshRenderer(Transform _trsf)
    {
        SkinnedMeshRenderer comp;
        if (_trsf.name == "mesh_GRP")
        {
            comp = _trsf.GetComponentInChildren<SkinnedMeshRenderer>();
            if (comp != null) return comp;
        }

        for (int i = 0; i < _trsf.childCount; i++)
        {
            if (_trsf.GetChild(i).name == "mesh_GRP")
            {
                comp = GetSkinnedMeshRenderer(_trsf.GetChild(i));
                if (comp != null) return comp;
            }
        }

        for (int i = 0; i < _trsf.childCount; i++)
        {
            comp = GetSkinnedMeshRenderer(_trsf.GetChild(i));
            if (comp != null) return comp;
        }

        return null;
    }

    protected virtual void Awake()
    {
        SkinnedMeshRenderer skinMeshR = GetSkinnedMeshRenderer(transform);
        if (skinMeshR)
        {
            if (skinMeshR.sharedMesh.subMeshCount == skinMeshR.materials.Length && skinMeshR.sharedMesh.isReadable)
            {
                skinMeshR.sharedMesh.subMeshCount = skinMeshR.sharedMesh.subMeshCount + 1;
                skinMeshR.sharedMesh.SetTriangles(skinMeshR.sharedMesh.triangles, skinMeshR.sharedMesh.subMeshCount - 1);
            }
        }
        else
        {
            MeshRenderer meshR = GetComponent<MeshRenderer>();
            if (meshR)
            {
                MeshFilter meshFilter;
                if (meshR.TryGetComponent<MeshFilter>(out meshFilter) && meshFilter.mesh.isReadable && meshFilter.mesh.subMeshCount == meshR.materials.Length)
                {
                    meshFilter.mesh.subMeshCount = meshFilter.mesh.subMeshCount + 1;
                    meshFilter.mesh.SetTriangles(meshFilter.mesh.triangles, meshFilter.mesh.subMeshCount - 1);
                }
            }
        }

        firePoints = GetComponent<FirePoints>();
    }
    protected virtual void Start()
    {
        //pvMax = pvBase;
        //pv = pvMax;
    }

    virtual protected void Update()
    {

    }
}
