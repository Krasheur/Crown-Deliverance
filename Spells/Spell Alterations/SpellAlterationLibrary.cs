using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellAlterationLibrary", order = 1)]
public class SpellAlterationLibrary : ScriptableObject
{
    [SerializeField] SpellAlteration[] alterations;

    public SpellAlteration[] Alterations { get => alterations; }

    public SpellAlteration GetSpellAlteration(string _name)
    {
        for (int i = 0; i < alterations.Length; i++)
        {
            if (alterations[i] && alterations[i].name == _name)
            {
                return alterations[i];
            }
        }

        return null;
    }
}
