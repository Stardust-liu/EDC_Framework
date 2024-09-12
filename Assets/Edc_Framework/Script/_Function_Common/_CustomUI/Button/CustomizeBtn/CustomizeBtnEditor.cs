using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace CustomizeUI{
    [CustomEditor(typeof(CustomizeBtn), true)]
    [CanEditMultipleObjects]
    public class CustomizeBtnEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var img = target as CustomizeBtn;
            var test = serializedObject.FindProperty("m_test");
            var isopen = serializedObject.FindProperty("m_isOpen");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(test);
            if(EditorGUI.EndChangeCheck()){
                Debug.Log("检测到Test变化");
                img.ChangeTest(test.intValue);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(isopen);
            if(EditorGUI.EndChangeCheck()){
                Debug.Log("检测到Isopen变化");
                img.ChangeIsOpen(isopen.boolValue);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
