using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TransparencyAnim))]
public class TransAnimEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Test"))
        {
            (target as TransparencyAnim).Anim();
        }
    }
}
