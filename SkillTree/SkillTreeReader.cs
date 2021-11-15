using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkillTreeReader : MonoBehaviour 
{
    [SerializeField] public string savedPath;
    [SerializeField] public SpellAlterationLibrary characterAlteration;

    private Character character;
    // Array with all the skills in our skilltree
    public Skill[] skillTree;

    // Dictionary with the skills in our skilltree
    public Dictionary<int, Skill> skills;

    // Variable for caching the currently being inspected skill
    private Skill skillInspected;

    //public int availablePoints = 100;

    void Awake()
    {
        character = GetComponent<Character>();

        SetUpSkillTree();
    }

	// Use this for initialization of the skill tree
	void SetUpSkillTree ()
    {
        skills = new Dictionary<int, Skill>();

        LoadSkillTree();
	}

    // Update is called once per frame
    void Update ()
    {

    }

    public void LoadSkillTree()
    {
        string path = savedPath;
        string dataAsJson;
        if (File.Exists(Application.dataPath + path))
        {
            // Read the json from the file into a string
            dataAsJson = File.ReadAllText(Application.dataPath + path);

            // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
            SkillTree loadedData = JsonUtility.FromJson<SkillTree>(dataAsJson);

            // Store the SkillTree as an array of Skill
            skillTree = new Skill[loadedData.skilltree.Length];
            skillTree = loadedData.skilltree;

            // Populate a dictionary with the skill id and the skill data itself
            for (int i = 0; i < skillTree.Length; ++i)
            {
                skills.Add(skillTree[i].id_Skill, skillTree[i]);
            }
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }        
    }

    public bool IsSkillUnlocked(int id_skill)
    {
        if (skills.TryGetValue(id_skill, out skillInspected))
        {
            return skillInspected.unlocked;
        }
        else
        {
            return false;
        }
    }

    public bool CanSkillBeUnlocked(int id_skill)
    {
        bool canUnlock = true;

        if (skills.TryGetValue(id_skill, out skillInspected)) // The skill exists
        {
            if(skillInspected.cost <= character.availableSkillTreePoints) // Enough points available
            {
                if (skillInspected.id_Skill != 0 && skillInspected.id_Skill != 9 && skillInspected.id_Skill != 18 && skillInspected.id_Skill != 27
                    && skillInspected.id_Skill % 3 == 0)
                {
                    if (!skillTree[skillInspected.id_Skill - 1].unlocked && !skillTree[skillInspected.id_Skill - 2].unlocked)
                    {
                        canUnlock = false;
                    }
                }
                else
                {
                    int[] dependenciesUnlocked = skillInspected.skill_DependenciesUnlocked;
                    for (int i = 0; i < dependenciesUnlocked.Length; ++i)
                    {
                        if (skills.TryGetValue(dependenciesUnlocked[i], out skillInspected))
                        {
                            if (!skillInspected.unlocked)
                            {
                                canUnlock = false;
                                break;
                            }
                        }
                        else // If one of the dependencies doesn't exist, the skill can't be unlocked.
                        {
                            Debug.Log("If one of the dependencies doesn't exist, the skill can't be unlocked");
                            return false;
                        }
                    }

                    if (skills.TryGetValue(id_skill, out skillInspected))
                    {
                        int[] dependenciesLocked = skillInspected.skill_DependenciesLocked;
                        for (int i = 0; i < dependenciesLocked.Length; ++i)
                        {
                            if (skills.TryGetValue(dependenciesLocked[i], out skillInspected))
                            {
                                if (skillInspected.unlocked)
                                {
                                    canUnlock = false;
                                    break;
                                }
                            }
                            else // If one of the dependencies doesn't exist, the skill can't be unlocked.
                            {
                                Debug.Log("If one of the dependencies doesn't exist, the skill can't be unlocked");
                                return false;
                            }
                        }
                    }
                }
            }
            else // If the player doesn't have enough skill points, can't unlock the new skill
            {
                Debug.Log("If the player doesn't have enough skill points, can't unlock the new skill");
                return false;
            }
        }
        else // If the skill id doesn't exist, the skill can't be unlocked
        {
            Debug.Log("If the skill id doesn't exist, the skill can't be unlocked");
            return false;
        }
        return canUnlock;
    }

    public void UnlockSkill(int id_Skill)
    {
        if (CanSkillBeUnlocked(id_Skill) && !skillTree[id_Skill].unlocked)
        {
            if (skills.TryGetValue(id_Skill, out skillInspected)) // The skill exists
            {
                character.availableSkillTreePoints -= skillInspected.cost;
                skillInspected.unlocked = true;
                AkSoundEngine.PostEvent("MenuSelect_Play", gameObject);

                if (skillInspected.id_Skill == 4 || skillInspected.id_Skill == 5 || skillInspected.id_Skill == 13 || skillInspected.id_Skill == 14 
                    || skillInspected.id_Skill == 22 || skillInspected.id_Skill == 23 || skillInspected.id_Skill == 31 || skillInspected.id_Skill == 32)
                {
                    character.SpellList[skillInspected.spellID].CostModifier += 1;
                }

                if (skillInspected.id_Skill == 7 || skillInspected.id_Skill == 8 || skillInspected.id_Skill == 16 || skillInspected.id_Skill == 17
                    || skillInspected.id_Skill == 25 || skillInspected.id_Skill == 26 || skillInspected.id_Skill == 34 || skillInspected.id_Skill == 35)
                {
                    character.SpellList[skillInspected.spellID].Cooldown += 1;
                }

                Debug.Log(skillInspected.alterationName);
                character.SpellList[skillInspected.spellID].SpellModifiers.Add(characterAlteration.GetSpellAlteration(skillInspected.alterationName));

                // We replace the entry on the dictionary with the new one (already unlocked)
                skills.Remove(id_Skill);
                skills.Add(id_Skill, skillInspected);
                //return true;
            }
        }
    }

    public bool IsSkillBlock(int id_skill)
    {
        if (skills.TryGetValue(id_skill, out skillInspected)) // The skill exists
        {
            int[] dependenciesLocked = skillInspected.skill_DependenciesLocked;
            for (int i = 0; i < dependenciesLocked.Length; ++i)
            {
                if (skills.TryGetValue(dependenciesLocked[i], out skillInspected))
                {
                    if (skillInspected.unlocked)
                    {
                        return true;
                    }
                }
                else // If one of the dependencies doesn't exist, the skill can't be unlocked.
                {
                    Debug.Log("If one of the dependencies doesn't exist, the skill can't be unlocked");
                }
            }
        }

        return false;
    }
}