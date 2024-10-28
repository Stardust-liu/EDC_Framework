using System.Collections;
using System.Collections.Generic;
using CustomizeUI;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace CustomizeUI{
    [CustomEditor(typeof(TabOptionBase), true)]
    [CanEditMultipleObjects]
    public class TabOptionEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var IsScrollRectChild = serializedObject.FindProperty("m_IsScrollRectChild");
            var ScrollRect = serializedObject.FindProperty("m_ScrollRect");
            var pointerEnterTween = serializedObject.FindProperty("m_PointerEnterTween");
            var pointerExitTween = serializedObject.FindProperty("m_PointerExitTween");
            var selectTween = serializedObject.FindProperty("m_SelectTween");
            var cancelSelectTween = serializedObject.FindProperty("m_CancelSelectTween");
            var PointerDownTween = serializedObject.FindProperty("m_PointerDownTween");
            var pointerDownEndTween = serializedObject.FindProperty("m_PointerDownEndTween");
            var isCloseItself = serializedObject.FindProperty("m_IsCloseItself");
            var unSelectAudioType = serializedObject.FindProperty("m_UnSelectAudioType");
            var selectAudioType = serializedObject.FindProperty("m_SelectAudioType");
            EditorGUILayout.PropertyField(IsScrollRectChild);
            if (IsScrollRectChild.boolValue)
            {
                EditorGUILayout.PropertyField(ScrollRect);

                if (GUILayout.Button("查找滚动视图"))
                {
                    CustomizeBtn customizeBtn = (CustomizeBtn)target;
                    customizeBtn.TryGetScrollRect();
                }
                GUILayout.Space(6);
            }
            
            EditorGUILayout.PropertyField(pointerEnterTween);
            EditorGUILayout.PropertyField(pointerExitTween);
            EditorGUILayout.PropertyField(selectTween);
            EditorGUILayout.PropertyField(cancelSelectTween);
            EditorGUILayout.PropertyField(PointerDownTween);
            EditorGUILayout.PropertyField(pointerDownEndTween);
            EditorGUILayout.PropertyField(isCloseItself);
            EditorGUILayout.PropertyField(unSelectAudioType, new GUIContent("非选中状态点击音效"));
            EditorGUILayout.PropertyField(selectAudioType, new GUIContent("选中状态点击音效"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}