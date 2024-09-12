using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class InfiniteScroll_V<T> : BaseInfiniteScroll<T>
{
    [PropertyOrder(-1)]
    public VerticalLayoutGroup verticalLayoutGroup;
    private float itemOccupyH;

    /// <summary>
    /// 设置间隔
    /// </summary>
    protected override void SetItemInterval(){
        var itemIntervalHalf = itemInterval/2f;
        verticalLayoutGroup.spacing = itemInterval;
        contentParent.offsetMax = new Vector2(0, -itemIntervalHalf);
        contentParent.offsetMin = new Vector2(0, itemIntervalHalf);
        SaveLayoutGroupState();
    }

    protected override void SaveLayoutGroupState()
    {
#if UNITY_EDITOR
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);
        EditorUtility.SetDirty(verticalLayoutGroup);
        AssetDatabase.SaveAssets();
#endif
    }

    protected override void CheckHeadItem(){
        if(item[h].Pos.y > headUpdatePosEdge){
            item[h].UpdatePos(item[t].Pos, Vector2.down * (itemOccupyH + itemInterval), dragPos);
            base.CheckHeadItem();
        }
    }

    protected override void CheckTailItem(){
        if(item[t].Pos.y < tailUpdatePosEdge){
            item[t].UpdatePos(item[h].Pos, Vector2.up * (itemOccupyH + itemInterval), dragPos);
            base.CheckTailItem();
        }
    }

    public override void Start(){
        base.Start();
        var itemIntervalHalf = itemInterval/2f;
        direction = Vector2.up;
        itemOccupyH = ((RectTransform)item[0].transform).rect.height;
        headUpdatePosEdge = itemOccupyH + itemIntervalHalf;
        tailUpdatePosEdge = -viewportSize.y - itemOccupyH - itemIntervalHalf;
        verticalLayoutGroup.enabled = false;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        var paddingEdge = new Vector3[4];
        var center = contentParent.position;

        GetRootSize(contentParent);
        var heightOffset = (((RectTransform)item[0].transform).rect.height + itemInterval/2) * 2;
        float width = contentParent.rect.width * rootCanvasScale.x;
        float height = (contentParent.rect.height + heightOffset) * rootCanvasScale.y;


        var offsetSize = new Vector2(width, height)/2;
        paddingEdge[0] = new Vector3(-offsetSize.x,  offsetSize.y, 0) + center;//左前
        paddingEdge[1] = new Vector3(offsetSize.x, offsetSize.y, 0) + center;//右前
        paddingEdge[2] = new Vector3(offsetSize.x, -offsetSize.y, 0) + center;//右后
        paddingEdge[3] = new Vector3(-offsetSize.x, -offsetSize.y, 0) + center;//左后
        
        var paddingEdgeColor = new Color(0.85f,  1, 0.85f, 0.15f);
        Handles.DrawSolidRectangleWithOutline(paddingEdge, paddingEdgeColor, Color.green);
        base.OnDrawGizmos();
    }
#endif
}
