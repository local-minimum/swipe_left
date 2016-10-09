using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NPC))]
public class NPCEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Restart"))
        {
            
            NPC myTaget = target as NPC;
            myTaget.ResetChat();
            EditorUtility.SetDirty(myTaget);
        }
    }
}
