using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace CustomOdinAttribute{
    public class DragPathAttributeDraw : OdinAttributeDrawer<DragPathAttribute, string>
    {
        public string path;

        protected override void Initialize()
        {
            base.Initialize();
            path = ValueEntry.SmartValue;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var e = Event.current;
            var rect = GUIHelper.GetCurrentLayoutRect();
            var isEnterArea = rect.Contains(e.mousePosition);            
            switch (e.type)
            {
                case EventType.DragUpdated when isEnterArea:
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                        e.Use();
                    break;
                case EventType.DragExited when isEnterArea:
                        path = GetDirectoryPath(DragAndDrop.paths[0]);
                        e.Use();
                    break;
            }
            GUILayout.BeginHorizontal();{
                GUILayout.Label(Property.Name,  GUILayout.Width(rect.width + 50));
                ValueEntry.SmartValue = path = GUILayout.TextArea(path);
            }
            GUILayout.EndHorizontal();
        }

        public string GetDirectoryPath(string path){
            if(!string.IsNullOrEmpty(Path.GetExtension(path))){
               return Path.GetDirectoryName(path);
            }
            else{
                return path;
            }
        }
    }
}
