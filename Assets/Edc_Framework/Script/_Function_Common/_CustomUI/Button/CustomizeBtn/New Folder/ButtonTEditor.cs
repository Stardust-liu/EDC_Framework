using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace CustomizeUI{
    [CustomEditor(typeof(CustomizeBtn), true)]
    [CanEditMultipleObjects]
    public class ButtonTEditor : CustomizeBtnEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var clickMsg = serializedObject.FindProperty("m_clickMsg");
            EditorGUILayout.PropertyField(clickMsg);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
