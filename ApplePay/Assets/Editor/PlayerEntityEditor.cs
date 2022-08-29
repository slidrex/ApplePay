using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PlayerEntity))]
public class PlayerEntityEditor : Editor
{
    public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PlayerEntity entity = (PlayerEntity)target;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            entity.ChangeAmount = EditorGUILayout.FloatField("Amount", entity.ChangeAmount);
            EditorGUILayout.Separator();
            if(GUILayout.Button("Change Health", GUILayout.MaxWidth(300)))
            {
                entity.ChangeHealth((int)entity.ChangeAmount);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            entity.ChangeHealthKey = (KeyCode)EditorGUILayout.EnumFlagsField("Change Health Key", entity.ChangeHealthKey, GUILayout.MaxWidth(300));
            EditorUtility.SetDirty(target);
        }
}
