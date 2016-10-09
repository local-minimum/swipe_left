using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PsychologyProfile), true)]
public class PsyProfileEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PsychologyProfile myTarget = target as PsychologyProfile;

        EditorGUI.BeginChangeCheck();

        if (!myTarget.isSocial) {
            if (GUILayout.Button("+ Make Social"))
            {
                myTarget.ExpandToAllDimension();
            }
        } else if (!myTarget.hasAllDimensions && GUILayout.Button("+ Add Missing Dimensions"))
        {
            myTarget.ExpandToAllDimension();
        } else if (GUILayout.Button("Make featureless"))
        {
            for (int i = 1, l = myTarget.nDimensions + 1; i < l; i++)
            {
                SocialDimension sd = (SocialDimension)i;
                myTarget.SetValue(sd, 0.5f);
            }
        }

        string[] sdNames = System.Enum.GetNames(typeof(SocialDimension));
        for (int i = 1, l=myTarget.nDimensions + 1; i< l; i++)
        {
            SocialDimension sd = (SocialDimension)i;

            float val = myTarget.GetValue(sd);
            float newVal = EditorGUILayout.Slider(sdNames[i], val, 0, 1);
            if (val != newVal)
            {
                myTarget.SetValue(sd, newVal);
            }            
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }
}
