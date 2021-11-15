using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using System.IO;

[System.Serializable]
public class BehaviorData
{

    public int id;
    public int[] dependencies;
    public string name;
    public BehaviorType type;
}

public enum BehaviorType
{
    Selector,
    Sequence,
    Node
}

[System.Serializable]
public class BehaviorDataTree
{
    public BehaviorData[] behaviorDataTree;
}

public class BTManager : MonoBehaviour
{
    public static BTManager main;

    private void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }

        main = this;
        DontDestroyOnLoad(this);
    }

    BehaviorData[] _behaviorDataTree;

    private BehaviorTree.BehaviorTree fairyTree;
    private BehaviorTree.BehaviorTree rogueTree;
    private BehaviorTree.BehaviorTree warriorTree;

    public BehaviorTree.BehaviorTree FairyTree { get => fairyTree; }
    public BehaviorTree.BehaviorTree RogueTree { get => rogueTree; }
    public BehaviorTree.BehaviorTree WarriorTree { get => warriorTree; }

    private void Start()
    {
        fairyTree = ConstructTree("Fairy");
        rogueTree = ConstructTree("Rogue");
        warriorTree = ConstructTree("Warrior");
    }

    private BehaviorTree.BehaviorTree ConstructTree(string _class)
    {
        BehaviorTree.BehaviorTree bt = new BehaviorTree.BehaviorTree();

        string path = Application.dataPath + "/Scripts/AI/BehaviorTree/Data/" + _class + "/behaviortree.json";
        string dataAsJson;

        if (File.Exists(path))
        {
            // Read the json from the file into a string
            dataAsJson = File.ReadAllText(path);

            // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
            BehaviorDataTree behaviorData = JsonUtility.FromJson<BehaviorDataTree>(dataAsJson);

            // Store the SkillTree as an array of Skill
            _behaviorDataTree = new BehaviorData[behaviorData.behaviorDataTree.Length];
            _behaviorDataTree = behaviorData.behaviorDataTree;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }


        Node[] nodes = new Node[_behaviorDataTree.Length];

        for (int i = 0; i < _behaviorDataTree.Length; i++)
        {
            switch (_behaviorDataTree[i].type)
            {
                case BehaviorType.Selector:
                    nodes[i] = new Selector();
                    break;

                case BehaviorType.Sequence:
                    nodes[i] = new Sequence();
                    break;

                case BehaviorType.Node:

                    System.Type t = System.Type.GetType(_behaviorDataTree[i].name + ", Assembly-CSharp");

                    nodes[i] = (Node)System.Activator.CreateInstance(t);

                    break;
            }
        }

        for (int i = 0; i < _behaviorDataTree.Length; i++)
        {
            if (_behaviorDataTree[i].dependencies.Length > 0)
            {
                for (int j = 0; j < _behaviorDataTree[i].dependencies.Length; j++)
                {
                    switch (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].type)
                    {
                        case BehaviorType.Selector:
                            (nodes[_behaviorDataTree[i].dependencies[j]] as Selector).AddNode(nodes[i]);
                            break;
                        case BehaviorType.Sequence:
                            (nodes[_behaviorDataTree[i].dependencies[j]] as Sequence).AddNode(nodes[i]);
                            break;
                    }

                    if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "GetTargetsCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as GetTargetsCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "GetAlliesCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as GetAlliesCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "CloudSpellAvailableCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as CloudSpellAvailableCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "CloudSpellDoneCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as CloudSpellDoneCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "TargetUnderBrandingEffectCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as TargetUnderBrandingEffectCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "CanAttackAndFleeCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as CanAttackAndFleeCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "CanBuffCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as CanBuffCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "CanDebuffCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as CanDebuffCondition).AddNode(nodes[i]);
                    }
                    else if (_behaviorDataTree[_behaviorDataTree[i].dependencies[j]].name == "IsProvokedCondition")
                    {
                        (nodes[_behaviorDataTree[i].dependencies[j]] as IsProvokedCondition).AddNode(nodes[i]);
                    }
                }
            }
        }

        bt.root = nodes[0];

        return bt;
    }
}
