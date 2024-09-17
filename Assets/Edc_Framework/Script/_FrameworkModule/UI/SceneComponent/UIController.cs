using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [LabelText("标记勾选RaycastTarget的UI")]
    public bool isMarkRaycastTargetUI;
    public CanvasScaler canvas_UI;
    public CanvasScaler canvas_CG;
    public RectTransform panelParent;

    /// <summary>
    /// 设置左边距
    /// </summary>
    public void SetLeftMargin(int value){
        var bottonMargin = panelParent.offsetMin.y;
        panelParent.offsetMin = new Vector2(value, bottonMargin);
    }

    /// <summary>
    /// 设置右边距
    /// </summary>
    public void SetRightMargin(int value){
        var topMargin = panelParent.offsetMax.y;
        panelParent.offsetMax = new Vector2(-value, topMargin);
    }


    /// <summary>
    /// 设置下边距
    /// </summary>
    public void SetBottomMargin(int value){
        var leftMargin = panelParent.offsetMin.x;
        panelParent.offsetMin = new Vector2(leftMargin, value);
    }

    /// <summary>
    /// 设置上边距
    /// </summary>
    public void SetTopMargin(int value){
        var rightMargin = panelParent.offsetMax.x;
        panelParent.offsetMax = new Vector2(rightMargin, -value);
    }

    /// <summary>
    /// 设置UI适配
    /// </summary>
    public void SetAdaptation(){
        if(Screen.width > Screen.height){
            canvas_CG.referenceResolution = canvas_UI.referenceResolution = new Vector2(1920, 1080);
            canvas_UI.matchWidthOrHeight = 0;
            canvas_CG.matchWidthOrHeight = 1;
        }
        else{
            canvas_CG.referenceResolution = canvas_UI.referenceResolution = new Vector2(1080, 1920);
            canvas_UI.matchWidthOrHeight = 1;
            canvas_CG.matchWidthOrHeight = 0;
        }
    }

    private void OnDrawGizmos(){
        if (isMarkRaycastTargetUI)
        {
            Vector3[] boxSize = new Vector3[4];
            foreach (var item in FindObjectsOfType<MaskableGraphic>())
            {
                if (item.raycastTarget)
                {
                    Gizmos.color = Color.blue;
                    for (int i = 0; i < 4; i++)
                    {
                        item.rectTransform.GetWorldCorners(boxSize);
                        Gizmos.DrawLine(boxSize[i], boxSize[(i + 1) % 4]);
                    }
                }
            }
        }
    }

}
