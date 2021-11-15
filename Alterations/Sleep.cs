using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : Frost
{
    bool canWake = true;

    public bool CanWake { get => canWake; set => canWake = value; }

    public override void OnTakeDamage(Entity _entity, ref DamageStruct dmg)
    {
        if (canWake)
        {
            Kill();
        }
    }
}
