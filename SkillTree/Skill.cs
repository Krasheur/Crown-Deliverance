[System.Serializable]
public class Skill {

    public int id_Skill;
    public int[] skill_DependenciesUnlocked;
    public int[] skill_DependenciesLocked;
    public bool unlocked;
    public int cost;
    public int spellID;
    public string alterationName;
}
