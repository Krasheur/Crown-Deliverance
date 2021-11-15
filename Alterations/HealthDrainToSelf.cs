using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrainToSelf : Alteration
{
    static public HealthDrainToSelf instance;

    List<Projectile> projs;

    public List<Projectile> Projs { get => projs; set => projs = value; }

    protected override void Awake()
    {
        instance = this;
        Projs = new List<Projectile>();
        base.Awake();
    }

    protected override void Update()
    {
        bool allProjHit = true;
        if (projs.Count == 0) allProjHit = false;
        for (int i = 0; i < projs.Count; i++)
        {
            ProjectileBounce pB = projs[i].GetComponent<ProjectileBounce>();
            if (!projs[i].HasHit || (pB && pB.BounceNum > 0))
            {
                allProjHit = false;
                break;
            }
        }

        if (allProjHit)
        {
            Kill();
        }

        base.Update();
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        DamageStruct dmg = new DamageStruct();
        dmg.amountHeal = dmgReport.amountDamagToArmor + dmgReport.amountDamagToHp;
        owner.ChangePv(dmg);
    }

    public override void OnDeath()
    {

    }

    public override void OnEndTurn()
    {

    }

    public override void OnNewTurn()
    {

    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
