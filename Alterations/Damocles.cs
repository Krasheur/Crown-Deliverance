using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damocles : Discobolus
{
    bool falling = false;
    int stackDamage = 0;

    protected override string[] format
    {
        get
        {
            return new string[1] { (BonusFromStat * 0.05f * 100.0f).ToString() };
        }
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {
        if (Projectile.HasHit)
        {
            done = true;
            if (dmgReport.dodged) return;
            DamageStruct dmg = new DamageStruct();
            dmg.amountDamag = (int)(BonusFromStat * 0.05f * stackDamage);
            _entity.ChangePv(dmg);
        }
    }

    protected override void Update()
    {
        Character character = owner as Character;

        if (Projectile)
        {
            if (falling)
            {
                //Projectile.enabled = true;
                Projectile.gameObject.SetActive(true);
                if (Projectile.Target)
                {
                    Projectile.TargetPosition = Projectile.Target.transform.position;
                }
            }
            else
            {
                if (Projectile.Progress >= 0.5f)
                {
                    Projectile.Progress = 0.5f;
                    //Projectile.enabled = false;
                    Projectile.gameObject.SetActive(false);
                }
            }
        }

        base.Update();
        if (done && (!character || character.SpellCasted))
        {
            Kill();
        }
    }

    public override void OnDeath()
    {

    }

    public override void OnEndTurn()
    {

    }

    public override void OnNewTurn()
    {
        falling = true;
        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {
        stackDamage += dmg.amountDamagToArmor + dmg.amountDamagToHp;
    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }
}
