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
    public float HorizontalMargin { get { return Data.horizontalMargin; } }
    public float VerticalMargin { get { return Data.verticalMargin; } }

    protected override void Init()
    {
        base.Init();
        SetAdaptation();
    }

    /// <summary>
    /// 设置水平边距
    /// </summary>
    public void SetHorizontalMargin(float value)
    {
        Data.SetHorizontalMargin(value);
        this.SendEvent<UpdateMargins>();
    }

    /// <summary>
    /// 设置垂直边距
    /// </summary>
    public void SetVerticalMargin(float value)
    {
        Data.SetVerticalMargin(value);
        this.SendEvent<UpdateMargins>();
    }

    /// <summary>
    /// 设置UI适配
    /// </summary>
    public void SetAdaptation()
    {
        int width = Screen.width;
        int height = Screen.height;

        if (width > height)
        {
            canvasScaler_UI.referenceResolution = canvasScaler_3DUI.referenceResolution = canvasScaler_CG.referenceResolution = new Vector2(1920, 1080);
            if (width / (float)height > 1920 / 1080f)
            {
                canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight = 1;

            }
            else
            {
                canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight = 0;
            }
            canvasScaler_CG.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler_UI.referenceResolution = canvasScaler_3DUI.referenceResolution = canvasScaler_CG.referenceResolution = new Vector2(1080, 1920);
            if (height / (float)width > 1920 / 1080f)
            {
                canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight = 0;

            }
            else
            {
                canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight = 1;
            }
            canvasScaler_UI.matchWidthOrHeight = canvasScaler_3DUI.matchWidthOrHeight = 1;
            canvasScaler_CG.matchWidthOrHeight = 0;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
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
