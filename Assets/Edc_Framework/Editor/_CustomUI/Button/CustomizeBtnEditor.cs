using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace CustomizeUI{
    [CustomEditor(typeof(CustomizeTweenBtn), true)]
    [CanEditMultipleObjects]
    public class CustomizeTweenBtnEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var IsScrollRectChild = serializedObject.FindProperty("m_IsScrollRectChild");
            var ScrollRect = serializedObject.FindProperty("m_ScrollRect");
            var pointerEnterTween = serializedObject.FindProperty("m_PointerEnterTween");
            var pointerExitTween = serializedObject.FindProperty("m_PointerExitTween");
            var pointerDownTween = serializedObject.FindProperty("m_PointerDownTween");
            var pointerDownEndTween = serializedObject.FindProperty("m_PointerDownEndTween");
            var clickAudioType = serializedObject.FindProperty("m_ClickAudioType");
            EditorGUILayout.PropertyField(IsScrollRectChild);
            if (IsScrollRectChild.boolValue)
            {
                EditorGUILayout.PropertyField(ScrollRect);

                if (GUILayout.Button("查找滚动视图"))
                {
                    CustomizeTweenBtn customizeBtn = (CustomizeTweenBtn)target;
                    customizeBtn.TryGetScrollRect();
                }
                GUILayout.Space(6);
            }

            EditorGUILayout.PropertyField(pointerEnterTween);
            EditorGUILayout.PropertyField(pointerExitTween);
            EditorGUILayout.PropertyField(pointerDownTween);
            EditorGUILayout.PropertyField(pointerDownEndTween);
            EditorGUILayout.PropertyField(clickAudioType, new GUIContent("点击音效"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
