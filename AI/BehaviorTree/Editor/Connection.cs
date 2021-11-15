using System;
using UnityEditor;
using UnityEngine;

namespace BTEditor
{
    public class Connection
    {
        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        public Action<Connection> OnClickRemoveConnection;

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawLine(inPoint.rect.center - new Vector2(3f, 3f), outPoint.rect.center + new Vector2(3f, 3f));

            if (Handles.Button(((inPoint.rect.center + outPoint.rect.center) / 2), Quaternion.identity, 5, 8, Handles.RectangleHandleCap))
            {
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection(this);
                }
            }
        }
    }
}