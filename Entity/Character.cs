using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum ANIMATION_STATE
{
    IDLE,
    RUNNING,
    AUTOATTACK,
    SPELL1,
    SPELL2,
    SPELL3,
    SPELL4
}

public enum CLASSES
{
    ASSASSIN = 1 << 0,
    TANK = 1 << 1,
    WIZARD = 1 << 2,
    ALL = ASSASSIN | TANK | WIZARD
};

public enum CHARACTER_STATE
{
    FREE,
    FIGHT,
    LOCKED
}

public enum CHARACTER_HOSTILITY
{
    ALLY,
    NEUTRAL,
    ENEMY
}

// small class to help manage Item in the Inventory
public class ItemInventory
{
    public static Pickable emptyPickable = null;
    private Pickable item;
    private int nb = 0;

    public ItemInventory()
    {
        nb = 0;
        if (emptyPickable == null)
        {
            GameObject go = new GameObject("Empty");
            emptyPickable = go.AddComponent<Pickable>();
            Object.DontDestroyOnLoad(go);
        }

        item = emptyPickable;
    }
    public ItemInventory(Pickable _item, int _nb)
    {
        item = _item;
        nb = _nb;
    }

    public Pickable Item { get => item; set => item = value; }
    public int Nb { get => nb; set => nb = value; }

}



public delegate void OnCharacterMovement(Character _charact = null, float _dist = 0);

public class Character : Entity
{
    [Header("Character Info")]
    [SerializeField, Min(1)] private int level = 1;
    [SerializeField, Min(0), Tooltip(" If enemis die, player own this xp ")] private int exp = 0;
    private int nbTokenLevelup = 0;
    [SerializeField] private int expToLevelUp = 10;
    [SerializeField] private CLASSES classe;
    [SerializeField] private CHARACTER_STATE state;
    private ANIMATION_STATE anim_state = ANIMATION_STATE.IDLE;
    [SerializeField] private CHARACTER_HOSTILITY hostility;
    [SerializeField] private Sprite portrait;
    private Vector3 lastPos;

    // Stats
    [Header("Character Stats")]
    [SerializeField, Tooltip("Upgrade %armor, %DamageCrit")] private int strengthBase = 10;
    [SerializeField, Tooltip("Upgrade Mobility, %Dodge")] private int dexterityBase = 10;
    [SerializeField, Tooltip("Upgrade %LuckyCrit, increase PA")] private int intelligenceBase = 10;
    [SerializeField, Tooltip("Upgrade %Health Up," +
                                         "%Reduce cirtical damag")]
    private int constitutionBase = 10;
    [SerializeField] private int armorBase = 10;
    [SerializeField] private int armor = 0;
    [SerializeField] private int armorBonus = 0;
    [SerializeField] private int percentageArmorBonus = 0;
    [SerializeField] private int percentageReducCrit = 0;

    // Influenced Stats
    [Header("Character Influenced Stat")]

    [SerializeField] private int mobilityBase = 5;
    [SerializeField] private int criticalChanceBase = 1;
    [SerializeField] private int dodgeBase = 1;
    [SerializeField] private int criticalDamageBase = 150;


    // Action
    [Header("Character Action")]
    [SerializeField] private int totalPa = 6;
    [SerializeField] private int currentPa;
    private int totalPaBonus;
    private int currentPaBonus;
    private NavMeshAgent navAgent;
    private NavMeshObstacle navObstacle;

    // statsBonus
    private int strengthBonus;
    private int dexterityBonus;
    private int intelligenceBonus;
    private int constitutionBonus;
    private int healthPercentBonus;

    // influenced stats
    private int mobilityBonus;
    private int criticalChanceBonus;
    private int dodgeBonus;
    private int criticalDamageBonus;

    // Stuff
    private Equipable rightHand;
    private Equipable leftHand;
    private Equipable body;
    private ItemInventory[] inventory = new ItemInventory[24];
    [SerializeField] private int gold = 0;

    // Fight 
    [Header("Character Fight")]

    private Entity target;
    [SerializeField] private float distCovered;
    [SerializeField] private SpellHolder[] spellPrefab = new SpellHolder[4];
    [SerializeField] private SpellHolder[] spell = new SpellHolder[4];
    [SerializeField] private SpellHolder autoAttack;
    private bool autoSpellCasting = false;
    [SerializeField] private Spell spellCasted = null;

    [SerializeField] private int spellLearn = 4;
    [SerializeField] private float distTotal;
    [SerializeField] bool mobilityMov = true;
    Fight currentFight;

    //to SkillTree
    public int[] spellUpgrade = new int[4];
    public int availableSkillTreePoints = 100;
    //

    public bool AutoSpellCasting { get => autoSpellCasting; set => autoSpellCasting = value; }

    // Reseau // no reseau in this project 
    private bool moveOnOff = false;

    // other
    private float timingTurn;
    [Header("Merchant")]

    [SerializeField] private bool isMerchant = false;
    [SerializeField] private bool isHealer = false;
    private bool makeObjToSell = false;
    [SerializeField] private GameObject[] objToSell;
    [SerializeField] private int[] nbObjToSell;
    private Animator[] animators;
    bool reactivateAgent = false;

    private OnCharacterMovement onMovement;
    public OnCharacterMovement OnMovement { get => onMovement; set => onMovement = value; }

    [SerializeField] GameObject deadBodyPrefab;

    //Voices
    public string voice = null;
    private bool isTaunting = false;
    private bool isLaughing = false;
    private bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        navObstacle = gameObject.GetComponent<NavMeshObstacle>();
        pv = pvMax;
        armor = ArmorMax;
        CurrentPa = TotalPa;
    }

    protected override void Start()
    {
        if (PlayerManager.main && PlayerManager.main.PlayerFairy == this)
        {
            CameraBehaviour.instance.gameObject.SetActive(false);
            InputManager.main.gameObject.SetActive(false);
            InputManager.main.transform.position = PlayerManager.main.PlayerFairy.transform.position + -PlayerManager.main.PlayerFairy.transform.forward * 2f;
            InputManager.main.transform.rotation = this.transform.rotation;
            CameraBehaviour.instance.transform.position = PlayerManager.main.PlayerFairy.transform.position + -PlayerManager.main.PlayerFairy.transform.forward * 2f;
            CameraBehaviour.instance.thirdPersonCamera.m_XAxis.Value = -this.transform.rotation.eulerAngles.y;

            CameraBehaviour.instance.gameObject.SetActive(true);
            InputManager.main.gameObject.SetActive(true);
        }
        
        //to spelltree
        spellUpgrade[0] = -1;
        spellUpgrade[1] = -1;
        spellUpgrade[2] = -1;
        spellUpgrade[3] = -1;
        //

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = new ItemInventory();
            }
        }

        for (int i = 0; i < SpellLearn; i++)
        {
            if (spellPrefab[i] != null)
            {
                spell[i] = Instantiate(spellPrefab[i], transform).Init(this);
            }
        }

        if (autoAttack) autoAttack = Instantiate(autoAttack, transform).Init(this);


        // Animation        
        animators = GetComponentsInChildren<Animator>();

        ConstitutionBase = constitutionBase;
        ConstitutionBonus = constitutionBonus;
        HealthPercentBonus = healthPercentBonus;
        armor = ArmorMax;

        lastPos = this.transform.position;
    }

    // Accessor
    // something
    public int Level { get => level; set => level = value; }
    public int GetExp { get => exp; }
    public int NbTokenLevelup { get => nbTokenLevelup; set => nbTokenLevelup = value; }
    public int ExpToLevelUp { get => expToLevelUp; set => expToLevelUp = value; }
    public CHARACTER_STATE State { get => state; set => state = value; }
    public CLASSES Classe { get => classe; set => classe = value; }
    public NavMeshAgent NavAgent { get => navAgent; set => navAgent = value; }

    // Stats Base
    //    public int PVBase { get => pvBase; set => pvBase = value; }
    public int StrengthBase { get => strengthBase; set => strengthBase = value; }
    public int DexterityBase { get => dexterityBase; set => dexterityBase = value; }
    public int IntelligenceBase { get => intelligenceBase; set => intelligenceBase = value; }
    public int ConstitutionBase
    {
        get => constitutionBase;
        set
        {
            float ratioPvCons = PV / (float)PvMax;
            constitutionBase = value;
            UpdateMaxHp(ratioPvCons);
        }
    }

    void UpdateMaxHp(float ratio)
    {
        PvMax = (pvBase + (int)((pvBase) * ((constitutionBonus + constitutionBase) * 0.15)))
                    + (pvBase + (int)((pvBase) * ((constitutionBonus + constitutionBase) * 0.15))) * (healthPercentBonus / 100);
        PV = (int)(ratio * PvMax);
    }

    // Stats 
    public int GetStrength { get => (StrengthBase + StrengthBonus); }
    public int GetDexterity { get => (DexterityBase + DexterityBonus); }
    public int GetIntelligence { get => (IntelligenceBase + IntelligenceBonus); }
    public int GetConstitution { get => (ConstitutionBase + ConstitutionBonus); }


    // Stats Boost
    public int StrengthBonus { get => strengthBonus; set => strengthBonus = value; }
    public int DexterityBonus { get => dexterityBonus; set => dexterityBonus = value; }
    public int IntelligenceBonus { get => intelligenceBonus; set => intelligenceBonus = value; }
    public int ConstitutionBonus
    {
        get => constitutionBonus;
        set
        {
            float ratioPvCons = PV / (float)PvMax;
            constitutionBonus = value;
            UpdateMaxHp(ratioPvCons);
        }
    }
    public int HealthPercentBonus
    {
        get => healthPercentBonus;
        set
        {
            float ratioPvCons = PV / (float)PvMax;
            healthPercentBonus = value;
            UpdateMaxHp(ratioPvCons);
        }
    }

    // Action
    public int TotalPa { get => totalPa; set => totalPa = value; }
    public int TotalPaBonus { get => totalPaBonus; set => totalPaBonus = value; }
    public int GetPaMax { get => TotalPaBonus + TotalPa + Mathf.Max(0, (int)(GetIntelligence * 0.1)); }
    public int CurrentPa { get => currentPa; set => currentPa = value; }
    public int CurrentPaBonus { get => currentPaBonus; set => currentPaBonus = value; }
    public int GetPa { get => CurrentPa + CurrentPaBonus + Mathf.Max(0, (int)(GetIntelligence * 0.1)); }

    // Influenced Stats
    public int GetMobility { get => (MobilityBase + MobilityBonus + (int)(GetDexterity / 7)); }
    public int GetCriticalChance { get => (CriticalChanceBase + CriticalChanceBonus + (int)((float)GetIntelligence * 0.2f)); }
    public int GetDodge { get => DodgeBase + DodgeBonus + (int)(GetDexterity * 0.2f); }
    public int GetCriticalDamage { get => CriticalDamageBase + CriticalDamageBonus + (int)(GetStrength * 0.5f); }

    // Influenced Bonus
    public int MobilityBonus { get => mobilityBonus; set => mobilityBonus = value; }
    public int CriticalChanceBonus { get => criticalChanceBonus; set => criticalChanceBonus = value; }
    public int DodgeBonus { get => dodgeBonus; set => dodgeBonus = value; }
    public int CriticalDamageBonus { get => criticalDamageBonus; set => criticalDamageBonus = value; }

    // Base Influenced Stats
    public int MobilityBase { get => mobilityBase; set => mobilityBase = value; }
    public int CriticalChanceBase { get => criticalChanceBase; set => criticalChanceBase = value; }
    public int DodgeBase { get => dodgeBase; set => dodgeBase = value; }
    public int CriticalDamageBase { get => criticalDamageBase; set => criticalDamageBase = value; }
    public int PercentageReducCrit { get => percentageReducCrit; set => percentageReducCrit = value; }

    // Equipable
    public Equipable RightHand { get => rightHand; set => rightHand = value; }
    public Equipable LeftHand { get => leftHand; set => leftHand = value; }
    public Equipable Body { get => body; set => body = value; }
    public ItemInventory[] Inventory { get => inventory; set => inventory = value; }
    public int Gold { get => gold; set => gold = value; }
    public Sprite Portrait { get => portrait; set => portrait = value; }
    public int ArmorBase { get => armorBase; set => armorBase = Mathf.Max(value, 0); }
    public int ArmorBonus { get => armorBonus; set => armorBonus = Mathf.Max(value, 0); }
    public int ArmorMax { get => ArmorBase + ArmorBonus + Mathf.CeilToInt((float)(ArmorBase * (PercentageArmorBonus + (int)(GetStrength * 0.1f))) / 100); }
    public int PercentageArmorBonus { get => Mathf.Max(0, percentageArmorBonus); set => percentageArmorBonus = Mathf.Max(value, 0); }
    public int Armor { get => armor; set => armor = value; }

    public int SetXp { set => exp = value; }

    // Reseau // not use
    public bool MoveOnOff { get => moveOnOff; set => moveOnOff = value; }
    public CHARACTER_HOSTILITY Hostility { get => hostility; set => hostility = value; }
    public Vector3 GetRotattion()
    {
        Vector3 thisRotation;
        thisRotation.x = transform.rotation.x;
        thisRotation.y = transform.rotation.y;
        thisRotation.z = transform.rotation.z;
        return thisRotation;
    }
    public bool SetTargetPosition(Vector3 _target)
    {
        return navAgent.SetDestination(_target);
    }

    // fight
    public float DistCovered { get => distCovered; set => distCovered = value; }
    public Entity Target { get => target; set => target = value; }
    public SpellHolder[] SpellList { get => spell; }
    public SpellHolder AutoAttack { get => autoAttack; }
    public Spell SpellCasted { get => spellCasted; set => spellCasted = value; }
    public float DistTotal { get => distTotal; set => distTotal = value; }
    public bool MobilityMov { get => mobilityMov; set => mobilityMov = value; }
    public Vector3 LastPos { get => lastPos; set => lastPos = value; }
    public int SpellLearn
    {
        get => spellLearn;
        set
        {
            spellLearn = value;
            LearnNewSpell();
        }
    }

    //Other
    public bool IsMerchant { get => isMerchant; }
    public bool IsHealer { get => isHealer; }
    public Fight CurrentFight { get => currentFight; set => currentFight = value; }
    public ANIMATION_STATE Anim_state { get => anim_state; set => anim_state = value; }
    public Animator[] Animators { get => animators; set => animators = value; }

    // Function
    protected virtual void Move()
    {

    }
    
    public void GoForwardOrStop()
    {
        if (moveOnOff) navAgent.destination = (transform.position + (Vector3.forward / 3));
    }

    protected virtual void Attack()
    {

    }

    override protected void Update()
    {
        if (!IsDead)
        {
            switch (state)
            {
                case CHARACTER_STATE.FREE:
                    UpdateFree();
                    break;
                case CHARACTER_STATE.FIGHT:
                    UpdateFight();
                    break;
                case CHARACTER_STATE.LOCKED:
                    UpdateLocked();
                    break;
                default:
                    break;
            }
        }
       
        if (animators == null)
        {
            animators = GetComponentsInChildren<Animator>();
        }
        else
        {
            if (PlayerManager.main != null && (this == PlayerManager.main.FocusedCharacter || gameObject.tag == "Character"))
            {
                if (transform.position == lastPos)
                {
                    if (anim_state != ANIMATION_STATE.IDLE)
                    {
                        anim_state = ANIMATION_STATE.IDLE;
                        for (int i = 0; i < animators.Length; i++)
                        {
                            animators[i].SetBool("isRunning", false);
                        }
                    }
                }
                else
                {
                    if (anim_state != ANIMATION_STATE.RUNNING)
                    {
                        anim_state = ANIMATION_STATE.RUNNING;
                        for (int i = 0; i < animators.Length; i++)
                        {
                            animators[i].SetBool("isRunning", true);
                        }
                    }
                }
            }
            else
            {
                if (navAgent.velocity.magnitude < 1)
                {
                    if (anim_state != ANIMATION_STATE.IDLE)
                    {
                        anim_state = ANIMATION_STATE.IDLE;
                        if (navAgent.enabled == true) navAgent.ResetPath();
                        for (int i = 0; i < animators.Length; i++)
                        {
                            animators[i].SetBool("isRunning", false);
                        }
                    }
                }
                else
                {
                    if (anim_state != ANIMATION_STATE.RUNNING)
                    {
                        anim_state = ANIMATION_STATE.RUNNING;
                        for (int i = 0; i < animators.Length; i++)
                        {
                            animators[i].SetBool("isRunning", true);
                        }
                    }
                }
            }

            if (SpellCasted == null)
            {
                for (int i = 0; i < animators.Length; i++)
                {
                    animators[i].SetInteger("SpellCasted", -1);
                }
            }
        }

        //rotates toward target
        Vector3 aimAt;
        if (hostility == CHARACTER_HOSTILITY.ALLY && spellCasted && spellCasted.GetAim(out aimAt))
        {
            Vector3 direction = aimAt - transform.position;
            direction.y = 0;
            float diff = Mathf.Min(Vector3.Distance(transform.forward, direction.normalized), 1.0f);
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, Time.deltaTime * 5.0f / diff);
        }

        lastPos = transform.position;
    }

    private void UpdateFree()
    {
        if (navObstacle) navObstacle.enabled = false;
        navAgent.enabled = true;
        base.Update();
        timingTurn += Time.deltaTime;

        if (timingTurn > Fight.turnTime)
        {
            EndTurn();
            StartTurn();
            //change pv reset the timer 0
            if (timingTurn > Fight.turnTime) armor = ArmorMax; // Delegate to NewTurn out fight?
            timingTurn = 0;
        }
    }

    private void UpdateLocked()
    {
        if (navObstacle) navObstacle.enabled = true;
        navAgent.enabled = false;
        transform.position = lastPos;
    }


    public void StartTurn()
    {
        isLaughing = false;
        isTaunting = false;
        isAttacking = false;
        if (currentFight) State = CHARACTER_STATE.FIGHT;
        currentPa = (CHARACTER_HOSTILITY.NEUTRAL == hostility) ? 0 : Mathf.Min(4 + GetPa, TotalPa);
        distCovered = 0f;
        OnNewTurn?.Invoke();
    }
    
    public void EndTurn()
    {
        if (currentFight) State = CHARACTER_STATE.LOCKED;
        CurrentPa += GetPaMax;
        if (CHARACTER_HOSTILITY.NEUTRAL == this.hostility) CurrentPa = 0;
        MobilityMov = true;
        OnEndTurn?.Invoke();
    }

    public void MarchandInventoryMake()
    {

        if (!makeObjToSell)
        {
            // To the market *
            for (int it = 0; it < objToSell.Length; it++)
            {
                for (int nb = 0; nb < nbObjToSell[it]; nb++)
                {
                    GameObject obj = Instantiate(objToSell[it]);
                    if (obj.name.Contains("(Clone)")) obj.name = obj.name.Remove(obj.name.Length - 7);
                     Debug.Log(obj.name);
                     Debug.Log(PutInInventory(obj.GetComponent<Pickable>()));
                }
            }
            makeObjToSell = true;
        }
    }
    
    void ReactivateNavAgent()
    {
        reactivateAgent = true;
        NavMesh.onPreUpdate -= ReactivateNavAgent;
    }

    private void UpdateFight()
    {
        if (!navAgent.enabled)
        {
            if (navObstacle) navObstacle.enabled = false;
            NavMesh.onPreUpdate += ReactivateNavAgent;
        }

        if (reactivateAgent)
        {
            reactivateAgent = false;
            navAgent.enabled = true;
        }
    }
    
    public void FightMovement()
    {

    }

    public void FightAttack()
    {

    }

    private void LearnNewSpell()
    {
        for (int i = 0; i < SpellLearn; i++)
        {
            if (spell[i] == null) spell[i] = Instantiate(spellPrefab[i], transform).Init(this);
        }
    }

    public void GiveArmor(int _amount)
    {
        armor = Mathf.Clamp(armor + _amount, 0, ArmorMax);
    }

    public override DamageStruct ChangePv(DamageStruct _damageStruct)
    {

        if (OnWillTakeDamage != null) OnWillTakeDamage(_damageStruct.emitter, ref _damageStruct);
        DamageStruct dmgFeedBack = _damageStruct.Clone();
        dmgFeedBack.amountDamagToArmor = 0;
        dmgFeedBack.amountDamagToHp = 0;
        dmgFeedBack.amountHeal += dmgFeedBack.amountHealBonus;

        if (dmgFeedBack.amountArmor != 0)
        {
            int previousArmor = armor;
            GiveArmor(_damageStruct.amountArmor);
            dmgFeedBack.amountArmor = Mathf.Max(armor - previousArmor, 0);
            dmgFeedBack.amountDamagToArmor = Mathf.Abs(Mathf.Min(armor - previousArmor, 0));
            if (dmgFeedBack.amountArmor < 0)
            {
                for (int i = 0; i < Animators.Length; i++)
                {
                    Animators[i].SetBool("isTakingDamage", true);
                    if (voice != null) UseVoice(voice, "Arg_Play", gameObject);
                    if (PlayerManager.main.ItsPlayer(this.name) && (voice == PlayerManager.main.PlayerTank.name || voice == PlayerManager.main.PlayerRogue.name)) { AkSoundEngine.PostEvent("HymeliaAllyHit_Play", PlayerManager.main.PlayerFairy.gameObject); }
                }
            }
        }
        
        if (dmgFeedBack.amountHeal > 0)
        {
            int previousHP = PV;
            PV = Mathf.Min(PV + dmgFeedBack.amountHeal, PvMax);
            dmgFeedBack.amountHeal = PV - previousHP;
        }
        if (_damageStruct.amountDamag > 0)
        {
            int amountRandToTouch = Random.Range(0, 101);
            if (_damageStruct.touchAutomaticly
                || (amountRandToTouch > GetDodge))
            {
                if (_damageStruct.criticalHit > 0)
                {
                    _damageStruct.criticalHit = Mathf.Max(_damageStruct.criticalHit - PercentageReducCrit, 0);
                    _damageStruct.amountDamag = Mathf.Max(((int)((float)_damageStruct.amountDamag * ((float)_damageStruct.criticalHit / 100))) + _damageStruct.amountDamagBonus, 0);
                }

                if (_damageStruct.percArmor)
                {
                    int previousHP = PV;
                    PV = (!isKillable) ? Mathf.Max(1, PV - (_damageStruct.amountDamag + _damageStruct.amountDamagBonus))
                                        : Mathf.Max(0, PV - (_damageStruct.amountDamag + _damageStruct.amountDamagBonus));

                    for (int i = 0; i < Animators.Length; i++)
                    {
                        Animators[i].SetBool("isTakingDamage", true);
                    }
                    if (voice != null) UseVoice(voice, "Arg_Play", gameObject);
                    if (PlayerManager.main.ItsPlayer(this.name) && (voice == PlayerManager.main.PlayerTank.name || voice == PlayerManager.main.PlayerRogue.name)) { AkSoundEngine.PostEvent("HymeliaAllyHit_Play", PlayerManager.main.PlayerFairy.gameObject); }
                    dmgFeedBack.amountDamagToHp = previousHP - PV;
                    //dmgFeedBack.amountDamagToArmor = 0;
                    if (previousHP - PV < (PlayerManager.main.PlayerTank.pvMax * 0.1f) && previousHP - PV > 0 && voice == PlayerManager.main.PlayerTank.name) { UseVoice(voice, "Help_Play", gameObject); }
                    timingTurn = 0;
                }
                else
                {
                    int previousArmor = Armor;
                    Armor -= (_damageStruct.amountDamag + _damageStruct.amountDamagBonus);
                    for (int i = 0; i < Animators.Length; i++)
                    {
                        Animators[i].SetBool("isTakingDamage", true);
                    }
                    if (voice != null) UseVoice(voice, "Arg_Play", gameObject);
                    if (PlayerManager.main.ItsPlayer(this.name) && (voice == PlayerManager.main.PlayerTank.name || voice == PlayerManager.main.PlayerRogue.name)) { AkSoundEngine.PostEvent("HymeliaAllyHit_Play", PlayerManager.main.PlayerFairy.gameObject); }
                    if (Armor < 0)
                    {
                        int previousHP = PV;
                        dmgFeedBack.amountDamagToArmor += previousArmor;
                        PV = (!isKillable) ? Mathf.Max(1, PV + Armor) : Mathf.Max(0, PV + Armor);
                        dmgFeedBack.amountDamagToHp = previousHP - PV;
                        if (previousHP - PV < 10 && previousHP - PV > 0 && voice == PlayerManager.main.PlayerTank.name) { UseVoice(voice, "Help_Play", gameObject); }
                        Armor = 0;
                    }
                    else
                    {
                        dmgFeedBack.amountDamagToArmor += previousArmor - Armor;
                    }
                    timingTurn = 0;
                }

                if (OnTakeDamage != null) OnTakeDamage(_damageStruct.emitter, ref dmgFeedBack);
            }
            else
            {
                dmgFeedBack.dodged = true;
            }

        }
        dmgFeedBack.emitter = _damageStruct.emitter;
        Taunt(dmgFeedBack.emitter, dmgFeedBack);
        FeedBack.main.FeedBackHit(transform.position, dmgFeedBack);

        if (IsDead && this.Hostility == CHARACTER_HOSTILITY.ENEMY) PlayerManager.main.XpForAll(this.GetExp);
        if (IsDead)
        {
            OnDeath?.Invoke();
            ToolTipPopUp.Main.HideInfo();
            if (!PlayerManager.main.ItsPlayer(name))
            {
                GameObject deadBody = Instantiate(deadBodyPrefab, transform.position, transform.rotation);
                ItemGenerator itGen = GetComponentInChildren<ItemGenerator>();
                if (itGen != null)
                {
                    itGen.gameObject.transform.parent = deadBody.transform;
                }
                deadBody.GetComponent<Ragdoll>().Character = this;

                Destroy(gameObject);
            }
            else
            {
                enabled = false;
            }

            if (NavAgent.enabled)
            {
                NavAgent.ResetPath();
                NavAgent.enabled = false;
            }

            if (navObstacle) navObstacle.enabled = false;
        }

        return dmgFeedBack;
    }

    public bool UseConsumable(Consumable _item, int pos)
    {
        if (_item.OnUse != null)
        {
            AkSoundEngine.PostEvent("HealBottle_Play", gameObject); // Play Bottle Sound
            _item.OnUse(this);
            inventory[pos].Nb--;
            if (inventory[pos].Nb == 0)
            {
                inventory[pos].Item = new Consumable();
            }
            return true;
        }
        return false;
    }
    
    public bool Equip(Equipable _item, bool _rightHand, int _place = -1)
    {
        if (classe == _item.ClassRequired && level >= _item.LevelRequired)
        {
            if (_item as Shield != null)
            {
                if (LeftHand != null)
                {
                    LeftHand.OnUnequip?.Invoke(this);
                    if (!PutInInventory(LeftHand, _place))
                    {
                        LeftHand.gameObject.SetActive(true);
                        LeftHand.transform.position = Vector3.forward + this.transform.position;
                        SceneManager.MoveGameObjectToScene(LeftHand.gameObject, SceneManager.GetActiveScene());

                    }
                }
                _item.OnEquip?.Invoke(this);
                LeftHand = _item;
                //DontDestroyOnLoad(_item.gameObject);

                return true;
            }
            else if (_item as Armor)
            {
                if (Body != null)
                {
                    Body.OnUnequip?.Invoke(this);
                    if (!PutInInventory(Body, _place))
                    {
                        Body.gameObject.SetActive(true);
                        Body.transform.position = Vector3.forward + this.transform.position;
                        SceneManager.MoveGameObjectToScene(Body.gameObject, SceneManager.GetActiveScene());
                    }
                }
                _item.OnEquip?.Invoke(this);
                Body = _item;
                //DontDestroyOnLoad(_item.gameObject);
                return true;
            }
            else if (_item as Weapon)
            {
                if (RightHand != null && _rightHand)
                {
                    RightHand.OnUnequip?.Invoke(this);
                    if (!PutInInventory(RightHand, _place))
                    {
                        SceneManager.MoveGameObjectToScene(RightHand.gameObject, SceneManager.GetActiveScene());
                        RightHand.gameObject.SetActive(true);
                        RightHand.transform.position = Vector3.forward + this.transform.position;
                    }

                    _item.OnEquip?.Invoke(this);
                    RightHand = _item;
                    // DontDestroyOnLoad(_item.gameObject);
                    return true;
                }
                else if (LeftHand != null && !_rightHand
                    && Classe == CLASSES.ASSASSIN)
                {
                    LeftHand.OnUnequip?.Invoke(this);
                    if (!PutInInventory(LeftHand, _place))
                    {
                        LeftHand.gameObject.SetActive(true);
                        LeftHand.transform.position = Vector3.forward + this.transform.position;
                        SceneManager.MoveGameObjectToScene(LeftHand.gameObject, SceneManager.GetActiveScene());
                    }

                    _item.OnEquip?.Invoke(this);
                    LeftHand = _item;
                    //DontDestroyOnLoad(_item.gameObject);
                    return true;

                }
                else if (RightHand == null && _rightHand)
                {

                    _item.OnEquip?.Invoke(this);
                    RightHand = _item;
                    // DontDestroyOnLoad(_item.gameObject);
                    return true;
                }
                else if (LeftHand == null && !_rightHand
                    && Classe == CLASSES.ASSASSIN)
                {
                    _item.OnEquip?.Invoke(this);
                    LeftHand = _item;
                    //DontDestroyOnLoad(_item.gameObject);
                    return true;
                }
            }
        }
        return false;
    }


    public bool UnEquip(Equipable _item, bool _rightHand, int _place = -1)
    {
        if (_item as Shield != null)
        {
            if (LeftHand != null)
            {
                LeftHand.OnUnequip?.Invoke(this);
                if (!PutInInventory(LeftHand, _place))
                {
                    LeftHand.gameObject.SetActive(true);
                    LeftHand.transform.position = Vector3.forward + this.transform.position;
                    SceneManager.MoveGameObjectToScene(LeftHand.gameObject, SceneManager.GetActiveScene());
                }
                LeftHand = null;
            }
            return true;
        }
        else if (_item as Armor)
        {
            if (Body != null)
            {
                Body.OnUnequip?.Invoke(this);
                if (!PutInInventory(Body, _place))
                {
                    Body.gameObject.SetActive(true);
                    Body.transform.position = Vector3.forward + this.transform.position;
                    SceneManager.MoveGameObjectToScene(Body.gameObject, SceneManager.GetActiveScene());

                }
                Body = null;
            }
            return true;
        }
        else if (_item as Weapon)
        {
            if (RightHand != null && RightHand == _item)
            {
                RightHand.OnUnequip?.Invoke(this);
                if (!PutInInventory(RightHand, _place))
                {
                    SceneManager.MoveGameObjectToScene(RightHand.gameObject, SceneManager.GetActiveScene());
                    RightHand.gameObject.SetActive(true);
                    RightHand.transform.position = Vector3.forward + this.transform.position;
                }
                RightHand = null;
                return true;
            }
            else if (LeftHand != null && LeftHand == _item)
            {
                LeftHand.OnUnequip?.Invoke(this);
                if (!PutInInventory(LeftHand, _place))
                {
                    LeftHand.gameObject.SetActive(true);
                    LeftHand.transform.position = Vector3.forward + this.transform.position;
                    SceneManager.MoveGameObjectToScene(LeftHand.gameObject, SceneManager.GetActiveScene());
                }
                LeftHand = null;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// _obj the pickable you want add to the inventory;
    /// _place to put this _obj (default -1);
    /// _nb the number of _obj you want add ( default 1);
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_place"></param>
    /// <param name="_nb"></param>
    /// <returns></returns>
    public bool PutInInventory(Pickable _obj, int _place = -1, int _nb = 1)
    {
        if (_obj.name == "Gold" || _obj.name == "Golds")
        {
        //just stack gold
            Gold += _nb;
            AkSoundEngine.PostEvent("Gold_Play", gameObject); // Play Gold Sound
            return true;
        }

        if (_obj != null)
        {

            Collider coll;
            if (_obj.TryGetComponent<Collider>(out coll)) coll.enabled = false;
            MeshRenderer meshoui;
            if (_obj.TryGetComponent<MeshRenderer>(out meshoui)) meshoui.enabled = false;

            if (_place >= 0)
            {
                ItemInventory aAdd = new ItemInventory(_obj, _nb);
                if (PlayerManager.main != null && PlayerManager.main.ItsPlayer(this.gameObject.name)) _obj.transform.parent = GameObject.Find("ItemStorage").transform;//DontDestroyOnLoad(_obj.gameObject);
                _obj.gameObject.SetActive(true);
                inventory[_place] = aAdd;
                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                return true;
            }

            int firstFree = -1;
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i].Nb != 0
                    && inventory[i].Item.Name == _obj.Name
                    && (_obj as Equipable) == null)
                {
                    inventory[i].Nb = inventory[i].Nb + _nb;
                    if (PlayerManager.main != null && PlayerManager.main.ItsPlayer(this.gameObject.name)) _obj.transform.parent = GameObject.Find("ItemStorage").transform;//DontDestroyOnLoad(_obj.gameObject);
                    _obj.gameObject.SetActive(true);
                    AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                    return true;
                }
                else if (firstFree < 0 && inventory[i].Nb == 0)
                {
                    firstFree = i;
                }
            }

            if (firstFree >= 0)
            {
                ItemInventory aAdd = new ItemInventory(_obj, _nb);
                if (PlayerManager.main != null && PlayerManager.main.ItsPlayer(this.gameObject.name)) _obj.transform.parent = GameObject.Find("ItemStorage").transform;//DontDestroyOnLoad(_obj.gameObject);
                _obj.gameObject.SetActive(true);
                inventory[firstFree] = aAdd;
                AkSoundEngine.PostEvent("OpenInventory_Play", gameObject); // Play Inventory Click Sound
                return true;
            }

            SceneManager.MoveGameObjectToScene(_obj.gameObject, SceneManager.GetActiveScene());
            _obj.EjectHim(transform.position, transform.forward);
        }

        return false;
    }

    public void GainExp(int _nbXp)
    {
        exp += _nbXp;
        if (_nbXp > 0) FeedBack.main.XpOwnTxt(transform.position, _nbXp);

        while (exp >= ExpToLevelUp)
        {
            LevelUp();
        }
        AkSoundEngine.PostEvent("XpWin_Play", gameObject);
    }

    public void LevelUp()
    {
        Level++;
        FeedBack.main.LevelUpTxt(transform.position);
        ExpToLevelUp = (int)(ExpToLevelUp * 2.5);
        availableSkillTreePoints += 2;
        if(level%3 ==0) availableSkillTreePoints++;
        NbTokenLevelup++;
        NbTokenLevelup++;
        PVBase += ((int)(PVBase * 0.1));
        UpdateMaxHp(1);
        AkSoundEngine.PostEvent("LevelUp_Play", gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //uselfull for test
        //if (state == CHARACTER_STATE.FREE && !IsDead)
        //{
        //    Character character;
        //    if (other.TryGetComponent(out character) && character.enabled && character.currentFight == null && !character.IsDead)
        //    {
        //        if (hostility == CHARACTER_HOSTILITY.ENEMY
        //            && character.hostility == CHARACTER_HOSTILITY.ALLY
        //            && character.state == CHARACTER_STATE.FREE)
        //        {
        //            //character.state = CHARACTER_STATE.FIGHT;
        //            //state = CHARACTER_STATE.FIGHT;

        //            List<Character> fighters = new List<Character>();
        //            fighters.Add(this);

        //            FindFighter(ref fighters, transform.position);

        //            Fight fight = FightManager.main.CreateFight();
        //            fight.SetFighter(fighters);
        //            FightHUD.main.InitializeTimeline(fight);
        //        }
        //    }
        /*
        Pickable obj;
        if (other.TryGetComponent<Pickable>(out obj))
        {
            obj.TakeHim(this);
        }
        */
        //}
    }

    public void FindFighter(ref List<Character> _fighters, Vector3 _position)
    {
        Collider[] colliders = Physics.OverlapSphere(_position, 7f);
        foreach (Collider col in colliders)
        {
            Character chara;
            if (col.TryGetComponent(out chara) && chara.enabled && chara.currentFight == null && !chara.IsDead && chara.Hostility != CHARACTER_HOSTILITY.NEUTRAL)
            {
                bool isInFight = false;
                foreach (Character f in _fighters)
                {
                    if (chara == f)
                    {
                        isInFight = true;
                    }
                }

                if (!isInFight)
                {
                    _fighters.Add(chara);
                    FindFighter(ref _fighters, chara.transform.position);
                }
            }
        }
    }

    void OnMouseOver()
    {
        if (ToolTipPopUp.Main && Input.GetMouseButtonDown(1)) //RightClick to show character tooltip
        {
            ToolTipPopUp.Main.DisplayInfoCharacter(this);
        }
    }

    void OnMouseExit()
    {
        if (ToolTipPopUp.Main)
        {
            ToolTipPopUp.Main.HideInfo();
        }
    }

    public string GetTooltipInfoText()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("<color=white>Level : " + level + "</color>\n");
        builder.Append("<color=white>" + armor + "/" + ArmorMax + " armor" + "</color>\n");
        builder.Append("<color=red>" + pv + "/" + PvMax + " life points" + "</color>").AppendLine();

        AlterationIcon[] alterations = GetComponentsInChildren<AlterationIcon>();

        for (int i = 0; i < alterations.Length; i++)
        {
            builder.Append("<size=20><color=yellow>\n- " + alterations[i].DisplayName);
        }

        builder.Append("</size>");
        return builder.ToString();
    }

    public void Taunt(Entity entity, DamageStruct dmgStruct)
    {
        if (!dmgStruct.emitter) return;
        if (!isTaunting && voice != null && voice != "Bg" && !(dmgStruct.amountHeal > 0) && dmgStruct.amountDamag > 0) UseVoice(voice, "Taunt_Play", dmgStruct.emitter.gameObject); isTaunting = true;

        if (!isLaughing && voice != null && dmgStruct.criticalHit > 0) UseVoice((dmgStruct.emitter as Character).voice, "Laugh_Play", dmgStruct.emitter.gameObject); isLaughing = true;

        if (!isTaunting && (voice == "Bg" || voice == PlayerManager.main.PlayerTank.name) && voice != PlayerManager.main.PlayerRogue.name && !(dmgStruct.amountHeal > 0)) UseVoice((dmgStruct.emitter as Character).voice, "Attack_Play", dmgStruct.emitter.gameObject); isAttacking = true;
    }

    private void UseVoice(string name, string add, GameObject emitter)
    {
        AkSoundEngine.PostEvent(name + add, emitter);
    }
}
