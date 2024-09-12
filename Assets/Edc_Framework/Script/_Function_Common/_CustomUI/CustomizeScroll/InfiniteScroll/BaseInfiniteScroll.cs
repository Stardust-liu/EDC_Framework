using System;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseInfiniteScroll<T> : BaseCustomizeScroll<T>, IPointerDownHandler, ILateUpdate, IEndDragHandler, IBeginDragHandler, IDragHandler
{    
    public bool applicationInertia;
    [ShowIf("applicationInertia")]
    [Range(0.95f, 0.999f)]
    public float inertia = 0.98f;
    public BaseInfiniteScrollItem<T>[] item;
    private float inertiaSpeed;
    private float inertiaSpeedOffset;
    private bool isStopInertia = true;
    private bool isAddLateUpdate;
    protected Vector2 dragPos;
    protected Vector2 lastDragPos;
    private Vector2 inertiaDirection;
    private Vector2 dragReferencePos;
    private Vector2 lastDragReferencePos;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(applicationInertia){
            if(isStopInertia && !isAddLateUpdate){
                Hub.Update.AddLateUpdate(this);
                isAddLateUpdate = true;
                inertiaSpeedOffset = inertiaSpeed = 0;
            }
            lastDragReferencePos = dragReferencePos;
            isStopInertia = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {   
        lastDragPos = eventData.position* direction;
        foreach (var item in item)
        {
            item.BeginDrag(eventData.position* direction);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragPos = eventData.position * direction;
        lastDragPos = dragPos;
        DragPosControl();
        CheckIsNeedPadding();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(applicationInertia){
            isStopInertia = false;
        }
    }

    public override void Start(){
        base.Start();
        var contentRect = contentParent.rect;
        itemCount = item.Length;
        t = itemCount-1;
        viewportSize = new Vector2(contentRect.width, contentRect.height);
        InitData();
    }

    protected override void InitData(){
        base.InitData();
        for (int i = 0; i < itemCount; i++)
        {
            var dataIndex = i % dataCount;
            item[i].UpdateData(dataIndex, scrollData[i]);
        }
    }

    /// <summary>
    /// 更新头部数据
    /// </summary>
    protected override void CheckHeadItem(){
        var i = GetCorrectIndex(item[t].CurrentDataIndex +1);
        item[h].UpdateData(i, scrollData[i]);
        base.CheckHeadItem();
    }

    /// <summary>
    /// 更新尾部数据
    /// </summary>
    protected override void CheckTailItem(){
        var i = GetCorrectIndex(item[h].CurrentDataIndex -1);
        item[t].UpdateData(i, scrollData[i]);
        base.CheckTailItem();
    }

    private void DragPosControl(){
        foreach (var item in item)
        {
            item.Drag(dragPos);
        }
        if(isStopInertia){
            dragReferencePos = dragPos;
        }
    }

    private void InertiaOffset(Vector2 offset){
        foreach (var item in item)
        {
            item.InertiaOffset(offset);
        }
    }
    
    public void OnLateUpdate()
    {
        ApplicationInertia();
    }


    private void ApplicationInertia(){
        float deltaTime = Time.unscaledDeltaTime;
        if(!isStopInertia){
            InertiaOffset(inertiaDirection * inertiaSpeedOffset * deltaTime);
            CheckIsNeedPadding();
            inertiaSpeedOffset -= math.lerp(inertiaSpeedOffset,0, inertia * (1-deltaTime));
            if(inertiaSpeedOffset < 0.01f){
                inertiaSpeedOffset = 0;
                isStopInertia = true;
                isAddLateUpdate = false;
                Hub.Update.RemoveLateUpdate(this);
            }
        }
        else{
            inertiaSpeed = Vector2.Distance(lastDragReferencePos, dragReferencePos) / deltaTime;
            if(lastDragReferencePos == dragReferencePos){
                inertiaSpeedOffset -= inertiaSpeedOffset * deltaTime * 15;
            }
            else{
                inertiaDirection = (dragReferencePos - lastDragReferencePos).normalized;
                lastDragReferencePos = dragReferencePos;
                inertiaSpeedOffset = inertiaSpeed;
            }
        }
    }

#if UNITY_EDITOR
    protected Vector2 rootCanvasScale;
    protected virtual void OnDrawGizmos()
    {
        Handles.BeginGUI();//这样可以让渲染效果保持在最前方
        Handles.EndGUI();
    }

    protected void GetRootSize(Transform rectTransform){
        var canvas = rectTransform.GetComponentInParent<Canvas>();
        if (!canvas.isRootCanvas)
        {
            GetRootSize(canvas.transform.parent);
        }
        else{
            rootCanvasScale = canvas.transform.localScale;
        }
    }
#endif
}
