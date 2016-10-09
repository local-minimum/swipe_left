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
            if (Application.isPlaying)
            {
                (target as TransparencyAnim).Anim();
            } else
            {
                Debug.LogError("Needs to be in playmode to animate");
            }
        }
    }
}
