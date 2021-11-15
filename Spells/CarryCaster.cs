using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryCaster : MonoBehaviour
{
    Projectile projectile;
    Character caster;
    Vector3 startPos;

    void Start()
    {
        projectile = GetComponent<Projectile>();
        caster = projectile.Emitter as Character;
        if (!caster)
        {
            Destroy(this);
            return;
        }
        startPos = caster.transform.position;

    }

    void Update()
    {
        Vector3 pos = (1 - projectile.Progress) * startPos + projectile.Progress * projectile.TargetPosition;
        caster.transform.position = pos;
        caster.LastPos = pos;
        if (caster.NavAgent.hasPath)
        {
            caster.NavAgent?.ResetPath();
        }
    }
}
