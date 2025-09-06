using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
namespace CustomOdinAttribute{
    public class ClampStringAttributeDraw : OdinAttributeDrawer<ClampStringAttribute, string>
    {
      
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var currentValue = ValueEntry.SmartValue;
            var selectedIndex = Attribute.array.ToList().IndexOf(currentValue);

            Rect rect = EditorGUILayout.GetControlRect(hasLabel: true, height: EditorGUIUtility.singleLineHeight);
        
            var index = SirenixEditorFields.Dropdown(rect, label, selectedIndex, Attribute.array, SirenixGUIStyles.CenteredBlackMiniLabel);
            this.ValueEntry.SmartValue = this.Attribute.array[index];
        }
    }
}
#endif
