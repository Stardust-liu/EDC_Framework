using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(RedDotTreeBaseNode))]
public class RedDotTreeEditor : NodeEditor
{
    public Color activeColor = new Color(0.45f, 0.2f, 0.2f);
    public Color defaultColor = new Color(0.26f, 0.26f, 0.26f);
    public override Color GetTint()
    {
        RedDotTreeBaseNode node = target as RedDotTreeBaseNode;
        if(node != null && node.isActive){
            return activeColor;
        }
        else{
            if(Application.isPlaying){
                return defaultColor;
            }
            {
                return base.GetTint();
            }
        }
    }
}
