using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAngel : Alteration
{
    [SerializeField] Protected protectedPrefab;
    Character guardian;
    List<Protected> listProtected;

    private void Start()
    {
        guardian = emitter as Character;
        listProtected = new List<Protected>();
    }

    public override void OnDealDamage(Entity _entity, ref DamageStruct dmgReport)
    {

    }

    public override void OnDeath()
    {

    }

    public override void OnEndTurn()
    {

    }

    public override void OnNewTurn()
    {
        base.OnNewTurn();
    }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    public override void OnWillTakeDamage(Entity _entity, ref DamageStruct dmg)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Character otherChar = other.GetComponent<Character>();
        if (guardian && otherChar && guardian != otherChar && otherChar.Hostility == guardian.Hostility)
        {
            Protected newProtected = Instantiate(protectedPrefab, otherChar.transform);
            newProtected.Emitter = emitter;
            listProtected.Add(newProtected);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Protected protAlt = other.GetComponentInChildren<Protected>();
        if (protAlt)
        {
            if (listProtected.Contains(protAlt))
            {
                listProtected.Remove(protAlt);
                protAlt.Kill();
            }
        }
    }

    public override void Kill()
    {
        if (listProtected != null)
        {
            foreach (Protected protAlt in listProtected)
            {
                protAlt.Kill();
            }
        }
        base.Kill();
    }
}
