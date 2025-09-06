using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : BaseMonoIOCComponent<UIManagerData>, ISendEvent
{
    [LabelText("标记勾选RaycastTarget的UI")]
    [SerializeField]
    private bool isMarkRaycastTargetUI;
    [SerializeField]
    private Canvas canvas_3DUI;
    [SerializeField]
    private CanvasScaler canvasScaler_UI;
    [SerializeField]
    private CanvasScaler canvasScaler_3DUI;
    [SerializeField]
    private CanvasScaler canvasScaler_CG;
    public int LeftMargin {get{return Data.leftMargin;}}
    public int RightMargin {get{return Data.rightMargin;}}
    public int BottomMargin {get{return Data.bottomMargin;}}
    public int TopMargin {get{return Data.topMargin;}}

    protected override void Init(){
        SetAdaptation();
    }

    /// <summary>
    /// 设置左边距
    /// </summary>
    public void SetLeftMargin(int value){
        Data.SetLeftMargin(value);
        this.SendEvent<UpdateMargins>();
    }

    /// <summary>
    /// 设置右边距
    /// </summary>
    public void SetRightMargin(int value){
        Data.SetRightMargin(value);
        this.SendEvent<UpdateMargins>();
    }

    /// <summary>
    /// 设置下边距
    /// </summary>
    public void SetBottomMargin(int value){
        Data.SetBottomMargin(value);
        this.SendEvent<UpdateMargins>();
    }

    /// <summary>
    /// 设置上边距
    /// </summary>
    public void SetTopMargin(int value){
        Data.SetTopMargin(value);
        this.SendEvent<UpdateMargins>();
    }

    /// <summary>
    /// 设置UI适配
    /// </summary>
    private void SetAdaptation(){
        if(Screen.width > Screen.height){
            canvasScaler_UI.referenceResolution = canvasScaler_3DUI.referenceResolution = canvasScaler_CG.referenceResolution = new Vector2(1920, 1080);
            canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight =  0;
            canvasScaler_CG.matchWidthOrHeight = 1;
            // if(!data.isInitSave){
            //     SetLeftMargin(150);
            //     SetRightMargin(150);
            // }
        }
        else{
            canvasScaler_UI.referenceResolution = canvasScaler_3DUI.referenceResolution = canvasScaler_CG.referenceResolution = new Vector2(1080, 1920);
            canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight = 1;
            canvasScaler_CG.matchWidthOrHeight = 0;
            // if(!data.isInitSave){
            //     SetBottomMargin(150);
            //     SetTopMargin(150);
            // }
        }
    }

    /// <summary>
    /// 设置页边距
    /// </summary>
    private void SetMargins(RectTransform rectTransform){
        rectTransform.offsetMin = new Vector2(Data.leftMargin, Data.bottomMargin);
        rectTransform.offsetMax = new Vector2(-Data.rightMargin, -Data.topMargin);
    }

#if UNITY_EDITOR
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
#endif
}
