using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomizeUI{
    [CustomEditor(typeof(LongPressBtn), true)]
    [CanEditMultipleObjects]
    public class LongPressBtnEditor : CustomizeBtnEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var enterContinuousPressStateTime = serializedObject.FindProperty("m_EnterContinuousPressStateTime");
            var performSpeed = serializedObject.FindProperty("m_CallInterval");
            var onLongPress = serializedObject.FindProperty("m_OnLongPress");
            EditorGUILayout.PropertyField(enterContinuousPressStateTime, new GUIContent("进入长按状态时间"));
            EditorGUILayout.PropertyField(performSpeed, new GUIContent("长按方法调用频率"));
            EditorGUILayout.PropertyField(onLongPress);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
