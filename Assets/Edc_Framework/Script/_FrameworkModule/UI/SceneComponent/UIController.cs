using System.Collections;
using System.Collections.Generic;
using ArchiveData;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerEventName{
    public const string UpdateMargins = nameof(UpdateMargins);
}

public class UIController : MonoBehaviour
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
    public int LeftMargin {get{return data.leftMargin;}}
    public int RightMargin {get{return data.rightMargin;}}
    public int BottomMargin {get{return data.bottomMargin;}}
    public int TopMargin {get{return data.topMargin;}}
    private UIControllerData data;
    public static readonly EventCenter eventCenter = new EventCenter();

    public void Init(){
        data = GameArchive.UIControllerData;
        SetAdaptation();
    }

    /// <summary>
    /// 设置左边距
    /// </summary>
    public void SetLeftMargin(int value){
        data.SetLeftMargin(value);
        eventCenter.OnEvent(UIControllerEventName.UpdateMargins);
    }

    /// <summary>
    /// 设置右边距
    /// </summary>
    public void SetRightMargin(int value){
        data.SetRightMargin(value);
        eventCenter.OnEvent(UIControllerEventName.UpdateMargins);
    }

    /// <summary>
    /// 设置下边距
    /// </summary>
    public void SetBottomMargin(int value){
        data.SetBottomMargin(value);
        eventCenter.OnEvent(UIControllerEventName.UpdateMargins);
    }

    /// <summary>
    /// 设置上边距
    /// </summary>
    public void SetTopMargin(int value){
        data.SetTopMargin(value);
        eventCenter.OnEvent(UIControllerEventName.UpdateMargins);
    }

    /// <summary>
    /// 设置3DUI的PlaneDisTance
    /// </summary>
    public void SetPlaneDisTance(int planeDisTance){
        canvas_3DUI.planeDistance = planeDisTance;
    }

    /// <summary>
    /// 设置UI适配
    /// </summary>
    private void SetAdaptation(){
        if(Screen.width > Screen.height){
            canvasScaler_UI.referenceResolution = canvasScaler_3DUI.referenceResolution = canvasScaler_CG.referenceResolution = new Vector2(1920, 1080);
            canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight =  0;
            canvasScaler_CG.matchWidthOrHeight = 1;
            if(!data.isInitSave){
                SetLeftMargin(150);
                SetRightMargin(150);
            }
        }
        else{
            canvasScaler_UI.referenceResolution = canvasScaler_3DUI.referenceResolution = canvasScaler_CG.referenceResolution = new Vector2(1080, 1920);
            canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight = 1;
            canvasScaler_CG.matchWidthOrHeight = 0;
            if(!data.isInitSave){
                SetBottomMargin(150);
                SetTopMargin(150);
            }
        }
    }

    /// <summary>
    /// 设置页边距
    /// </summary>
    private void SetMargins(RectTransform rectTransform){
        rectTransform.offsetMin = new Vector2(data.leftMargin, data.bottomMargin);
        rectTransform.offsetMax = new Vector2(-data.rightMargin, -data.topMargin);
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
