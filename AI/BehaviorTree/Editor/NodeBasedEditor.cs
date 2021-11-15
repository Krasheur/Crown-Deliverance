using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

namespace BTEditor
{
    public enum CLASS
    {
        Fairy,
        Rogue,
        Warrior
    }

    public class NodeBasedEditor : EditorWindow
    {
        private List<Node> nodes;
        private List<Connection> connections;

        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;
        private GUIStyle inPointStyle;
        private GUIStyle outPointStyle;

        private ConnectionPoint selectedInPoint;
        private ConnectionPoint selectedOutPoint;

        private Vector2 offset;
        private Vector2 drag;

        // Rect for buttons to Clear, Save and Load 
        private Rect rectButtonClear;
        private Rect rectButtonSave;
        private Rect rectButtonLoad;

        // Count for nodes created
        private int nodeCount;

        // Where we store the skilltree that we are managing with this tool
        private BehaviorDataTree behaviorDataTree;

        // Dictionary with the skills in our skilltree
        private Dictionary<int, BehaviorData> behaviorDataDictionary;

        [MenuItem("Window/Behavior Tree Editor")]
        private static void OpenWindow()
        {
            NodeBasedEditor window = GetWindow<NodeBasedEditor>();
            window.titleContent = new GUIContent("Behavior Tree Editor");
        }

        private void OnEnable()
        {
            // Create the skilltree
            behaviorDataTree = new BehaviorDataTree();

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/lightskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            inPointStyle = new GUIStyle();
            inPointStyle.normal.background = Resources.Load("green") as Texture2D;
            inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn mid on.png") as Texture2D;

            outPointStyle = new GUIStyle();
            outPointStyle.normal.background = Resources.Load("red") as Texture2D;
            outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn mid on.png") as Texture2D;

            // Create buttons for clear, save and load
            rectButtonClear = new Rect(new Vector2(10, 10), new Vector2(60, 20));
            rectButtonSave = new Rect(new Vector2(80, 10), new Vector2(60, 20));
            rectButtonLoad = new Rect(new Vector2(150, 10), new Vector2(60, 20));

            // Initialize nodes with saved data
            //LoadNodes();
        }

        private void OnGUI()
        {        
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);

            // We draw our new buttons (Clear, Load and Save)
            DrawButtons();

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed)
                Repaint();
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }


        public CLASS selectedClass = CLASS.Fairy;
        // Draw our new buttons for managing the skill tree
        private void DrawButtons()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Clear"))
            {
                ClearNodes();
            }
                
            if (GUILayout.Button("Save"))
            {
                SaveSkillTree();
            }
                
            if (GUILayout.Button("Load"))
            {
                LoadNodes();
            }

            selectedClass = (CLASS)EditorGUILayout.EnumPopup(selectedClass);

            GUILayout.EndHorizontal();
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        ClearConnectionSelection();
                    }

                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void DrawConnectionLine(Event e)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawLine(selectedInPoint.rect.center - new Vector2(3f, 3f), e.mousePosition);

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawLine(selectedOutPoint.rect.center + new Vector2(3f, 3f), e.mousePosition);

                GUI.changed = true;
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Selector"), false, () => OnClickAddNode(mousePosition, "Selector", BehaviorType.Selector));
            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Sequence"), false, () => OnClickAddNode(mousePosition, "Sequence", BehaviorType.Sequence));
            genericMenu.AddSeparator("");

            UnityEngine.Object[] nodesScripts = Resources.LoadAll("Nodes/Common");

            for (int i = 0; i < nodesScripts.Length; i++)
            {
                string name = nodesScripts[i].name;
                genericMenu.AddItem(new GUIContent(name), false, () => OnClickAddNode(mousePosition, name, BehaviorType.Node));
            }

            genericMenu.AddSeparator("");

            switch (selectedClass)
            {
                case CLASS.Fairy:
                    nodesScripts = Resources.LoadAll("Nodes/Fairy");
                    break;
                case CLASS.Rogue:
                    nodesScripts = Resources.LoadAll("Nodes/Rogue");
                    break;
                case CLASS.Warrior:
                    nodesScripts = Resources.LoadAll("Nodes/Warrior");
                    break;
            }

            for (int i = 0; i < nodesScripts.Length; i++)
            {
                string name = nodesScripts[i].name;
                genericMenu.AddItem(new GUIContent(name), false, () => OnClickAddNode(mousePosition, name, BehaviorType.Node));
            }

            genericMenu.ShowAsContext();
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void OnClickAddNode(Vector2 mousePosition, string _name, BehaviorType _type)
        {
            if (nodes == null)
            {
                nodes = new List<Node>();
            }

            // We create the node with the default info for the node
            nodes.Add(new Node(mousePosition, 130, 60, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, nodeCount, _name, _type, null));
            ++nodeCount;
        }

        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < connections.Count; i++)
                {
                    if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                    {
                        connectionsToRemove.Add(connections[i]);
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    connections.Remove(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }

            nodes.Remove(node);
        }

        private void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(connection);
        }

        private void CreateConnection()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }

            connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        // Function for clearing data from the editor window
        private void ClearNodes()
        {
            nodeCount = 0;
            if (nodes != null && nodes.Count > 0)
            {
                Node node;
                while (nodes.Count > 0)
                {
                    node = nodes[0];

                    OnClickRemoveNode(node);
                }
            }
        }

        // Save data from the window to the skill tree
        private void SaveSkillTree()
        {
            if (nodes.Count > 0)
            {
                // We fill with as many skills as nodes we have
                behaviorDataTree.behaviorDataTree = new BehaviorData[nodes.Count];
                int[] dependencies;
                List<int> dependenciesList = new List<int>();

                // Iterate over all of the nodes. Populating the skills with the node info
                for (int i = 0; i < nodes.Count; ++i)
                {
                    if (connections != null)
                    {
                        List<Connection> connectionsToRemove = new List<Connection>();
                        List<ConnectionPoint> connectionsPointsToCheck = new List<ConnectionPoint>();

                        for (int j = 0; j < connections.Count; j++)
                        {
                            if (connections[j].inPoint == nodes[i].inPoint)
                            {
                                for (int k = 0; k < nodes.Count; ++k)
                                {
                                    if (connections[j].outPoint == nodes[k].outPoint)
                                    {
                                        dependenciesList.Add(k);
                                        break;
                                    }
                                }
                                connectionsToRemove.Add(connections[j]);
                                connectionsPointsToCheck.Add(connections[j].outPoint);
                            }
                        }
                    }
                    dependencies = dependenciesList.ToArray();
                    dependenciesList.Clear();
                    behaviorDataTree.behaviorDataTree[i] = nodes[i].behaviorData;
                    behaviorDataTree.behaviorDataTree[i].dependencies = dependencies;
                }

                string json = JsonUtility.ToJson(behaviorDataTree);
                string path = null;

                switch (selectedClass)
                {
                    case CLASS.Fairy:
                        path = "Assets/Scripts/AI/BehaviorTree/Data/Fairy/behaviortree.json";
                        break;
                    case CLASS.Rogue:
                        path = "Assets/Scripts/AI/BehaviorTree/Data/Rogue/behaviortree.json";
                        break;
                    case CLASS.Warrior:
                        path = "Assets/Scripts/AI/BehaviorTree/Data/Warrior/behaviortree.json";
                        break;
                }

                // Finally, we write the JSON string with the SkillTree data in our file
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(json);
                    }
                }
                UnityEditor.AssetDatabase.Refresh();

                SaveNodes();

                //CreateBehaviorTree();
            }
        }

        // Save data from the nodes (position in our custom editor window)
        private void SaveNodes()
        {
            NodeDataCollection nodeData = new NodeDataCollection();
            nodeData.nodeDataCollection = new NodeData[nodes.Count];

            for (int i = 0; i < nodes.Count; ++i)
            {
                nodeData.nodeDataCollection[i] = new NodeData();
                nodeData.nodeDataCollection[i].id_Node = nodes[i].behaviorData.id;
                nodeData.nodeDataCollection[i].position = nodes[i].rect.position;
            }

            string json = JsonUtility.ToJson(nodeData);
            string path;

            switch (selectedClass)
            {
                case CLASS.Fairy:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/Fairy/nodeData.json";
                    break;
                case CLASS.Rogue:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/Rogue/nodeData.json";
                    break;
                case CLASS.Warrior:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/Warrior/nodeData.json";
                    break;
                default:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/nodeData.json";
                    break;
            }

            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(json);
                }
            }
            UnityEditor.AssetDatabase.Refresh();
        }

        private void LoadNodes()
        {
            ClearNodes();
            string path;

            switch (selectedClass)
            {
                case CLASS.Fairy:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/Fairy/nodeData.json";
                    break;
                case CLASS.Rogue:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/Rogue/nodeData.json";
                    break;
                case CLASS.Warrior:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/Warrior/nodeData.json";
                    break;
                default:
                    path = "Assets/Scripts/AI/BehaviorTree/Data/nodeData.json";
                    break;
            }

            string dataAsJson;
            NodeDataCollection loadedData;
            if (File.Exists(path))
            {
                // Read the json from the file into a string
                dataAsJson = File.ReadAllText(path);

                // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
                loadedData = JsonUtility.FromJson<NodeDataCollection>(dataAsJson);

                BehaviorData[] _behaviorDataTree;
                List<BehaviorData> originNode = new List<BehaviorData>();
                behaviorDataDictionary = new Dictionary<int, BehaviorData>();

                switch (selectedClass)
                {
                    case CLASS.Fairy:
                        path = "Assets/Scripts/AI/BehaviorTree/Data/Fairy/behaviortree.json";
                        break;
                    case CLASS.Rogue:
                        path = "Assets/Scripts/AI/BehaviorTree/Data/Rogue/behaviortree.json";
                        break;
                    case CLASS.Warrior:
                        path = "Assets/Scripts/AI/BehaviorTree/Data/Warrior/behaviortree.json";
                        break;
                    default:
                        path = "Assets/Scripts/AI/BehaviorTree/Data/behaviortree.json";
                        break;
                }

                Vector2 pos = Vector2.zero;
                if (File.Exists(path))
                {
                    // Read the json from the file into a string
                    dataAsJson = File.ReadAllText(path);

                    // Pass the json to JsonUtility, and tell it to create a SkillTree object from it
                    BehaviorDataTree behaviorData = JsonUtility.FromJson<BehaviorDataTree>(dataAsJson);

                    // Store the SkillTree as an array of Skill
                    _behaviorDataTree = new BehaviorData[behaviorData.behaviorDataTree.Length];
                    _behaviorDataTree = behaviorData.behaviorDataTree;

                    // Create nodes
                    for (int i = 0; i < _behaviorDataTree.Length; ++i)
                    {
                        for (int j = 0; j < loadedData.nodeDataCollection.Length; ++j)
                        {
                            if (loadedData.nodeDataCollection[j].id_Node == _behaviorDataTree[i].id)
                            {
                                pos = loadedData.nodeDataCollection[j].position;
                                break;
                            }
                        }
                        LoadBehaviorDataCreateNode(_behaviorDataTree[i], pos);
                        if (_behaviorDataTree[i].dependencies.Length == 0)
                        {
                            originNode.Add(_behaviorDataTree[i]);
                        }
                        behaviorDataDictionary.Add(_behaviorDataTree[i].id, _behaviorDataTree[i]);
                    }

                    BehaviorData outSkill;
                    Node outNode = null;
                    // Create connections
                    for (int i = 0; i < nodes.Count; ++i)
                    {
                        for (int j = 0; j < nodes[i].behaviorData.dependencies.Length; ++j)
                        {
                            if (behaviorDataDictionary.TryGetValue(nodes[i].behaviorData.dependencies[j], out outSkill))
                            {
                                for (int k = 0; k < nodes.Count; ++k)
                                {
                                    if (nodes[k].behaviorData.id == outSkill.id)
                                    {
                                        outNode = nodes[k];
                                        OnClickOutPoint(outNode.outPoint);
                                        break;
                                    }
                                }
                                OnClickInPoint(nodes[i].inPoint);
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("Cannot load game data!");
                }
            }
        }

        private void LoadBehaviorDataCreateNode(BehaviorData skill, Vector2 position)
        {
            if (nodes == null)
            {
                nodes = new List<Node>();
            }

            nodes.Add(new Node(position, 130, 60, nodeStyle, selectedNodeStyle,
                inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
                skill.id, skill.name, skill.type, skill.dependencies));
            ++nodeCount;
        }
    }
}