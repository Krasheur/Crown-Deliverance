using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR

[CustomEditor(typeof(BezierCurve))]
public class BezierEditor : Editor
{
    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //curve.AddKey();
        //EditorGUILayout.CurveField()
    }
}

#endif
