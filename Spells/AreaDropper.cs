using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDropper : MonoBehaviour
{
    [SerializeField] DamageArea damageArea;
    [SerializeField] protected CharacterStat ScalingStat;
    Projectile proj;
    protected int BonusFromStat
    {
        get
        {
            int stat = 0;
            Character emitterChar = proj.Emitter as Character;
            if (emitterChar)
            {
                switch (ScalingStat)
                {
                    case CharacterStat.DEXTERITY:
                        stat = emitterChar.GetDexterity;
                        break;
                    case CharacterStat.STRENGTH:
                        stat = (emitterChar.GetStrength);
                        break;
                    case CharacterStat.INTELLIGENCE:
                        stat = (emitterChar.GetIntelligence);
                        break;
                    case CharacterStat.CONSTITUTION:
                        stat = (emitterChar.GetConstitution);
                        break;
                    default:
                        stat = (0);
                        break;
                }
            }
            return stat;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        proj = transform.parent.GetComponent<Projectile>();

    }

    // Update is called once per frame
    void Update()
    {
        if (proj.HasHit)
        {
            DamageArea newDmgArea = Instantiate(damageArea, proj.transform.position, proj.transform.rotation);
            newDmgArea.Emitter = proj.Emitter as Character;
            newDmgArea.SpellDamage = (int)(BonusFromStat * 0.2f);

            if (newDmgArea.Emitter) newDmgArea.GetComponent<Character>().Hostility = newDmgArea.Emitter.Hostility;
            
            RaycastHit hit;
            if (Physics.Raycast(proj.transform.position, -Vector3.up, out hit, 10, LayerMask.NameToLayer("Environnement")))
            {
                newDmgArea.transform.position = hit.point;
                newDmgArea.transform.up = hit.normal;
            }
        }
    }
}
