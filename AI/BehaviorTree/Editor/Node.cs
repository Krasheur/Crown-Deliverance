using System;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace BTEditor
{
    public class Node
    {
        public Rect rect;
        public string title;
        public bool isDragged;
        public bool isSelected;

        // Rect for the title of the node 
        public Rect rectName;
        public Rect rectType;

        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;

        public GUIStyle style;
        public GUIStyle defaultNodeStyle;
        public GUIStyle selectedNodeStyle;

        // GUI Style for the title
        public GUIStyle styleName;
        public GUIStyle styleType;

        public Action<Node> OnRemoveNode;

        // Skill linked with the node
        public BehaviorData behaviorData;

        // StringBuilder to create the node's title
        private StringBuilder nodeName;
        private BehaviorType nodeType;

        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle,
            GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
            Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint,
            Action<Node> OnClickRemoveNode, int id, string name, BehaviorType type, int[] dependencies)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;

            inPoint = new ConnectionPoint(this, ConnectionPointType.In,
                inPointStyle, OnClickInPoint);

            outPoint = new ConnectionPoint(this, ConnectionPointType.Out,
                outPointStyle, OnClickOutPoint);

            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = OnClickRemoveNode;

            // Create new Rect and GUIStyle for our title and custom fields
            float rowHeight = height / 7;

            rectName = new Rect(position.x, position.y + rowHeight, width, rowHeight * 2);
            styleName = new GUIStyle();
            styleName.alignment = TextAnchor.UpperCenter;
            styleName.fontStyle = FontStyle.Bold;
            styleName.fontSize = 15;

            rectType = new Rect(position.x, position.y + 2 * rowHeight + 10, width, rowHeight);
            styleType = new GUIStyle();
            styleType.alignment = TextAnchor.UpperCenter;

            // We create the skill with current node info
            behaviorData = new BehaviorData();
            behaviorData.id = id;
            behaviorData.name = name;
            behaviorData.type = type;
            behaviorData.dependencies = dependencies;

            // Create string with ID info
            nodeName = new StringBuilder();
            nodeName.Append(name);

            nodeType = type;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
            rectName.position += delta;
            rectType.position += delta;
        }

        public void MoveTo(Vector2 pos)
        {
            rect.position = pos;
            rectName.position = pos;
            rectType.position = pos;
        }

        public void Draw()
        {
            inPoint.Draw();
            outPoint.Draw();
            GUI.Box(rect, title, style);

            // Print the title
            string name = GUI.TextField(rectName, behaviorData.name, styleName);
            nodeName.Clear();
            nodeName.Append(name);
            behaviorData.name = name;

            GUI.Label(rectType, nodeType.ToString(), styleType);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            style = defaultNodeStyle;
                        }
                    }

                    if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }
    }
}