using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public abstract class InfiniteScroll_H<T> : BaseInfiniteScroll<T>
{
    [PropertyOrder(-1)]
    public HorizontalLayoutGroup horizontalLayoutGroup;
    private float itemOccupyW;

    /// <summary>
    /// 设置间隔
    /// </summary>
    protected override void SetItemInterval(){
        var itemIntervalHalf = itemInterval/2f;
        horizontalLayoutGroup.spacing = itemInterval;
        contentParent.offsetMax = new Vector2(-itemIntervalHalf, 0);
        contentParent.offsetMin = new Vector2(itemIntervalHalf, 0);
        SaveLayoutGroupState();
    }
    protected override void SaveLayoutGroupState()
    {
#if UNITY_EDITOR
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);
        EditorUtility.SetDirty(horizontalLayoutGroup);
        AssetDatabase.SaveAssets();
#endif
    }

    protected override void CheckHeadItem(){
        if(item[h].Pos.x < headUpdatePosEdge){          
            item[h].UpdatePos(item[t].Pos, Vector2.right * (itemOccupyW + itemInterval), dragPos);
            base.CheckHeadItem();
        }
    }

    protected override void CheckTailItem(){
        if(item[t].Pos.x > tailUpdatePosEdge){
            item[t].UpdatePos(item[h].Pos, Vector2.left * (itemOccupyW + itemInterval), dragPos);
            base.CheckTailItem();
        }
    }

    public override void Start(){
        base.Start();
        var itemIntervalHalf = itemInterval/2f;
        direction = Vector2.right;
        itemOccupyW = ((RectTransform)item[0].transform).rect.width;
        headUpdatePosEdge = - itemOccupyW - itemIntervalHalf;
        tailUpdatePosEdge = viewportSize.x + itemOccupyW + itemIntervalHalf;
        horizontalLayoutGroup.enabled = false;
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        var paddingEdge = new Vector3[4];
        var center = contentParent.position;

        GetRootSize(contentParent);
        var widthOffset = (((RectTransform)item[0].transform).rect.width + itemInterval/2) * 2;
        float width = (contentParent.rect.width + widthOffset) * rootCanvasScale.x;
        float height = contentParent.rect.height * rootCanvasScale.y;
        var offsetSize = new Vector2(width, height)/2;
        paddingEdge[0] = new Vector3(-offsetSize.x, offsetSize.y, 0) + center;//左前
        paddingEdge[1] = new Vector3(offsetSize.x, offsetSize.y, 0) + center;//右前
        paddingEdge[2] = new Vector3(offsetSize.x,  -offsetSize.y, 0) + center;//右后
        paddingEdge[3] = new Vector3(-offsetSize.x,  -offsetSize.y, 0) + center;//左后

        var paddingEdgeColor = new Color(0.85f,  1, 0.85f, 0.15f);
        Handles.DrawSolidRectangleWithOutline(paddingEdge, paddingEdgeColor, Color.green);
        base.OnDrawGizmos();
    }
#endif
}
