using System.Collections;
using System.Collections.Generic;
using CustomizeUI;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MsgOption))]
public class MsgOptionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUILayout.PropertyField(property.FindPropertyRelative("dataEvent"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("callEvent"));
        property.serializedObject.ApplyModifiedProperties();
    }
}
