using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehindTarget : MonoBehaviour
{
    Projectile proj;

    private void Awake()
    {
        proj = GetComponent<Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (proj.Emitter && proj.Target)
        {
            proj.Emitter.transform.position = proj.Target.transform.position - proj.Target.transform.forward;
            proj.Emitter.transform.rotation = proj.Target.transform.rotation;
            if (proj.Emitter as Character)
            {
                (proj.Emitter as Character).LastPos = proj.Emitter.transform.position;
                (proj.Emitter as Character).NavAgent?.ResetPath();
            }
        }
    }
}
