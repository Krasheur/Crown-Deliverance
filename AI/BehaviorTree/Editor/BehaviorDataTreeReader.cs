using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BTEditor
{
    public class BehaviorDataTreeReader : MonoBehaviour
    {

        private static BehaviorDataTreeReader _instance;

        public static BehaviorDataTreeReader Instance
        {
            get
            {
                return _instance;
            }
            set
            {
            }
        }

        // Array with all the skills in our skilltree
        private BehaviorData[] _behaviorDataTree;

        // Dictionary with the skills in our skilltree
        private Dictionary<int, BehaviorData> _behaviorDatas;

        public int availablePoints = 100;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
                SetUpSkillTree();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Use this for initialization of the skill tree
        void SetUpSkillTree()
        {
            _behaviorDatas = new Dictionary<int, BehaviorData>();

            LoadSkillTree();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadSkillTree()
        {
            string path = "Assets/BehaviorTree/Data/behaviortree.json";
            string dataAsJson;
            if (File.Exists(path))
            {
                // Read the json from the file into a string
                dataAsJson = File.ReadAllText(path);

                // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
                BehaviorDataTree loadedData = JsonUtility.FromJson<BehaviorDataTree>(dataAsJson);

                // Store the SkillTree as an array of Skill
                _behaviorDataTree = new BehaviorData[loadedData.behaviorDataTree.Length];
                _behaviorDataTree = loadedData.behaviorDataTree;

                // Populate a dictionary with the skill id and the skill data itself
                for (int i = 0; i < _behaviorDataTree.Length; ++i)
                {
                    _behaviorDatas.Add(_behaviorDataTree[i].id, _behaviorDataTree[i]);
                }
            }
            else
            {
                Debug.LogError("Cannot load game data!");
            }
        }
    }
}