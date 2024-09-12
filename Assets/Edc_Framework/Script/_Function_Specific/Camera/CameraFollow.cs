using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class CameraFollow : BaseCamera<CameraFollow>
{
    public enum CamerViewType{
        SV, //侧视角
        TD, //俯视角
    }

    public enum FollowState{
        ForcedFollow,//强制跟随
        MomentFollow,//及时跟随
        TriggerFollow,//触发跟随
    }

    public Transform target;
    public CamerViewType camerViewType;
    public bool isActiveVerticalAxis;
    public bool isDrawFollowInfo;
    public Vector3 targetOffset;

    [BoxGroup("缓动相关"), LabelText("垂直轴缓动因子"), Range(2, 5f)]
    [ShowIf("isActiveVerticalAxis")]
    public float verticalAxisDamping = 3;

    [BoxGroup("缓动相关"), LabelText("水平缓动因子"), Range(2, 5f)]
    public float horizontalDamping = 3;

    [BoxGroup("缓动相关"), LabelText("垂直缓动因子"), Range(2, 5f)]
    public float verticalDamping = 3;


    [BoxGroup("禁止跟随区域")]
    public float prohibitFollowX;
    [BoxGroup("禁止跟随区域")]
    public float prohibitFollowY;
    [BoxGroup("禁止跟随区域")]
    public Vector2 prohibitFollowPosOffset;

    [BoxGroup("强制跟随区域")]
    public float forcedFollowX;
    [BoxGroup("强制跟随区域")]
    public float forcedFollowY;
    [BoxGroup("强制跟随区域")]
    public Vector2 forcedFollowPosOffset;

    [BoxGroup("参考标记相关")][Range(0.2f, 0.8f)]
    public float forcedFollowSize = 0.5f;
    [BoxGroup("参考标记相关")][Range(0.1f, 0.4f)]
    public float prohibitFollowSize = 0.3f;

    [BoxGroup("参考标记相关")][Range(0.05f, 0.5f)]
    public float targetMarkSize = 0.2f;

    private FollowState followState;
    private Vector3 followPos;
    private Vector3 targetOffsetMovement;
    private Vector3 lastTargetOffsetPos;
    public Vector3 TargetPos {get{return target.position; }}
    public Vector3 TargetOffsetPos {get{return target.position + targetOffset; }}
    public Vector3 ProhibitFollowCenter 
    {
        get
        {
            if(camerViewType == CamerViewType.SV){
                return CameraPos + (Vector3)prohibitFollowPosOffset; 
            }
            else{
                return CameraPos + new Vector3(prohibitFollowPosOffset.x, 0, prohibitFollowPosOffset.y);
            }
        }
    }
    public Vector3 ForcedFollowCenter 
    {
        get
        {
            if(camerViewType == CamerViewType.SV){
                return CameraPos + (Vector3)forcedFollowPosOffset; 
            }
            else{
                return CameraPos + new Vector3(forcedFollowPosOffset.x, 0, forcedFollowPosOffset.y);
            }
        }
    }

    [Button("使相机与Target垂直")]
    public void CameraVerticallyToTarget(){
       
        if(camerViewType == CamerViewType.SV){
            cameraTransform.position = new Vector3(TargetPos.x, TargetPos.y, CameraPos.z);
        }
        else{
            cameraTransform.position = new Vector3(TargetPos.x, CameraPos.y, TargetPos.z);
        }    
    }

    [Button("使禁止跟随区域与Target垂直")]
    [GUIColor(0.7f, 1, 0.8f)]
    public void prohibitFollowVerticallyToTarget(){
       
        if(camerViewType == CamerViewType.SV){
            prohibitFollowPosOffset = new Vector2(TargetPos.x- CameraPos.x, TargetPos.y- CameraPos.y);
        }
        else{
            prohibitFollowPosOffset = new Vector2(TargetPos.x- CameraPos.x, TargetPos.z- CameraPos.z);
        }    
    }

    [Button("使跟随区域与Target垂直")]
    [GUIColor(0.7f, 0.8f, 1)]
    public void FollowVerticallyToTarget(){
       
        if(camerViewType == CamerViewType.SV){
            forcedFollowPosOffset = new Vector2(TargetPos.x- CameraPos.x, TargetPos.y - CameraPos.y);
        }
        else{
            forcedFollowPosOffset = new Vector2(TargetPos.x- CameraPos.x, TargetPos.z- CameraPos.z);
        }    
    }

    protected override void Awake() {//保证最先加入LateUpdate列表
        base.Awake();
        if(FrameworkManager.isInitFinish){
            offset = cameraTransform.position - TargetOffsetPos;
            lastTargetOffsetPos = TargetOffsetPos;
            followState = FollowState.TriggerFollow;
            if(forcedFollowX == 0f && forcedFollowY == 0){
                followState = FollowState.ForcedFollow;
            }
            if(prohibitFollowX == 0 && prohibitFollowY == 0){
                followState = FollowState.MomentFollow;
            }
        }
    }

    public override void OnLateUpdate()
    {
        var deltaTime = Time.deltaTime;
        switch (followState)
        {
            case FollowState.ForcedFollow:
                ForcedFollow();
            break;
            case FollowState.MomentFollow:
                MomentFollow(deltaTime);
            break;
            case FollowState.TriggerFollow:
                TriggerFollow(deltaTime);
            break;
        }
    }

    /// <summary>
    /// 强制跟随
    /// </summary>
    private void ForcedFollow(){
        followPos = TargetOffsetPos + offset;
        if(!isActiveVerticalAxis){
            if(camerViewType == CamerViewType.SV){
                followPos.z = CameraPos.z;
            }
            else{
                followPos.y = CameraPos.y;
            }
        }
        cameraTransform.position = followPos;
    }
    
    /// <summary>
    /// 及时跟随
    /// </summary>
    private void MomentFollow(float deltaTime){
        followPos = TargetOffsetPos + offset;
        targetOffsetMovement = TargetOffsetPos - lastTargetOffsetPos;
        lastTargetOffsetPos = TargetOffsetPos;

        if(!CheckIsOutsideArea_X(ForcedFollowCenter.x, forcedFollowX)){
            followPos.x = Mathf.Lerp(CameraPos.x, followPos.x, horizontalDamping * deltaTime);
        }
        else{          
            followPos.x =CameraPos.x + targetOffsetMovement.x;
        }
        if(camerViewType == CamerViewType.SV){
            if(!CheckIsOutsideArea_Y(ForcedFollowCenter.y, forcedFollowY)){
                followPos.y = Mathf.Lerp(CameraPos.y, followPos.y, verticalDamping * deltaTime);
            }
            else{
                followPos.y = CameraPos.y + targetOffsetMovement.y;
            }
            if(isActiveVerticalAxis){
                followPos.z = Mathf.Lerp(CameraPos.z, followPos.z, verticalAxisDamping * deltaTime);
            }
            else{
                followPos.z = CameraPos.z;
            }
        }
        else{
            if(!CheckIsOutsideArea_Y(ForcedFollowCenter.z, forcedFollowY)){
                followPos.z = Mathf.Lerp(CameraPos.z, followPos.z, verticalDamping * deltaTime);
            }
            else{
                followPos.z = CameraPos.z + targetOffsetMovement.z;
            }
            if(isActiveVerticalAxis){
                followPos.y = Mathf.Lerp(CameraPos.y, followPos.y, verticalAxisDamping * deltaTime);
            }
            else{
                followPos.y = CameraPos.y;
            }
        }
        cameraTransform.position = followPos;
    }

    /// <summary>
    /// 触发跟随
    /// </summary>
    private void TriggerFollow(float deltaTime){
        if(camerViewType == CamerViewType.SV){
            if(CheckIsOutsideArea_X(ProhibitFollowCenter.x, prohibitFollowX)||
               CheckIsOutsideArea_Y(ProhibitFollowCenter.y, prohibitFollowY)){
                MomentFollow(deltaTime);
                return;
            }
        }
        else{
            if(CheckIsOutsideArea_X(ProhibitFollowCenter.x, prohibitFollowX)||
               CheckIsOutsideArea_Y(ProhibitFollowCenter.z, prohibitFollowY)){
                MomentFollow(deltaTime);
                return;
            }
        }
        offset = cameraTransform.position - TargetOffsetPos;

        if(isActiveVerticalAxis){
            followPos = CameraPos;
            if(camerViewType == CamerViewType.SV){
                followPos.z = Mathf.Lerp(CameraPos.z, followPos.z, verticalAxisDamping);
            }
            else{
                followPos.y = Mathf.Lerp(CameraPos.y, followPos.y, verticalAxisDamping);
            }
            cameraTransform.position = followPos;
        }
    }

    //检查 水平方向 是否超出区域
    private bool CheckIsOutsideArea_X(float areaCenterX, float areaSizeX){
        return Mathf.Abs(TargetOffsetPos.x - areaCenterX) > areaSizeX;
    }

    //检查 垂直方向 是否超出区域
    private bool CheckIsOutsideArea_Y(float areaCenterY, float areaSizeY){
        if(camerViewType == CamerViewType.SV){
            return Mathf.Abs(TargetOffsetPos.y - areaCenterY) > areaSizeY;
        }
        else{
            return Mathf.Abs(TargetOffsetPos.z - areaCenterY) > areaSizeY;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (target != null && isDrawFollowInfo)
        {
            var markOffset = Vector3.zero;
            if(camerViewType == CamerViewType.SV){
                markOffset = Vector3.forward * (TargetOffsetPos.z - CameraPos.z);
                var forcedFollowXRange = new Vector3[4];
                forcedFollowXRange[0] = new Vector3(ForcedFollowCenter.x - forcedFollowX, ForcedFollowCenter.y + forcedFollowY, TargetOffsetPos.z);//左上
                forcedFollowXRange[1] = new Vector3(ForcedFollowCenter.x + forcedFollowX, ForcedFollowCenter.y + forcedFollowY, TargetOffsetPos.z);//右上
                forcedFollowXRange[2] = new Vector3(ForcedFollowCenter.x + forcedFollowX, ForcedFollowCenter.y - forcedFollowY, TargetOffsetPos.z);//右下
                forcedFollowXRange[3] = new Vector3(ForcedFollowCenter.x - forcedFollowX, ForcedFollowCenter.y - forcedFollowY, TargetOffsetPos.z);//左下
                var forcedFollowXRangeColor = new Color(0.85f,  0.95f, 1, 0.15f);
                Handles.DrawSolidRectangleWithOutline(forcedFollowXRange, forcedFollowXRangeColor, Color.red);

                var prohibitFollowRange = new Vector3[4];
                prohibitFollowRange[0] = new Vector3(ProhibitFollowCenter.x - prohibitFollowX, ProhibitFollowCenter.y + prohibitFollowY, TargetOffsetPos.z);//左上
                prohibitFollowRange[1] = new Vector3(ProhibitFollowCenter.x + prohibitFollowX, ProhibitFollowCenter.y + prohibitFollowY, TargetOffsetPos.z);//右上
                prohibitFollowRange[2] = new Vector3(ProhibitFollowCenter.x + prohibitFollowX, ProhibitFollowCenter.y - prohibitFollowY, TargetOffsetPos.z);//右下
                prohibitFollowRange[3] = new Vector3(ProhibitFollowCenter.x - prohibitFollowX, ProhibitFollowCenter.y - prohibitFollowY, TargetOffsetPos.z);//左下
                var prohibitFollowRangeColor = new Color(0.85f,  1, 0.85f, 0.15f);
                Handles.DrawSolidRectangleWithOutline(prohibitFollowRange, prohibitFollowRangeColor, Color.green);
            }
            else{
                markOffset = Vector3.up * (TargetOffsetPos.y - CameraPos.y);
                var forcedFollowXRange = new Vector3[4];
                forcedFollowXRange[0] = new Vector3(ForcedFollowCenter.x - forcedFollowX, TargetOffsetPos.y, ForcedFollowCenter.z + forcedFollowY);//左前
                forcedFollowXRange[1] = new Vector3(ForcedFollowCenter.x + forcedFollowX, TargetOffsetPos.y, ForcedFollowCenter.z + forcedFollowY);//右前
                forcedFollowXRange[2] = new Vector3(ForcedFollowCenter.x + forcedFollowX, TargetOffsetPos.y, ForcedFollowCenter.z - forcedFollowY);//右后
                forcedFollowXRange[3] = new Vector3(ForcedFollowCenter.x - forcedFollowX, TargetOffsetPos.y, ForcedFollowCenter.z - forcedFollowY);//左后
                var forcedFollowXRangeColor = new Color(0.85f,  0.95f, 1, 0.15f);
                Handles.DrawSolidRectangleWithOutline(forcedFollowXRange, forcedFollowXRangeColor, Color.red);

                var prohibitFollowRange = new Vector3[4];
                prohibitFollowRange[0] = new Vector3(ProhibitFollowCenter.x - prohibitFollowX, TargetOffsetPos.y, ProhibitFollowCenter.z + prohibitFollowY);//左前
                prohibitFollowRange[1] = new Vector3(ProhibitFollowCenter.x + prohibitFollowX, TargetOffsetPos.y, ProhibitFollowCenter.z + prohibitFollowY);//右前
                prohibitFollowRange[2] = new Vector3(ProhibitFollowCenter.x + prohibitFollowX, TargetOffsetPos.y, ProhibitFollowCenter.z - prohibitFollowY);//右后
                prohibitFollowRange[3] = new Vector3(ProhibitFollowCenter.x - prohibitFollowX, TargetOffsetPos.y, ProhibitFollowCenter.z - prohibitFollowY);//左后
                var prohibitFollowRangeColor = new Color(0.85f,  1, 0.85f, 0.15f);
                Handles.DrawSolidRectangleWithOutline(prohibitFollowRange, prohibitFollowRangeColor, Color.green);
            }
            Vector3 p1;
            Vector3 p2;
            Handles.color = new Color(1, 0.2f, 0.3f);
            p1 = ForcedFollowCenter + Vector3.up*forcedFollowSize + markOffset;
            p2 = ForcedFollowCenter + Vector3.down*forcedFollowSize + markOffset;
            Handles.DrawLine(p1, p2);
            
            p1 = ForcedFollowCenter + Vector3.right*forcedFollowSize + markOffset;
            p2 = ForcedFollowCenter + Vector3.left*forcedFollowSize + markOffset;
            Handles.DrawLine(p1, p2);
            
            p1 = ForcedFollowCenter + Vector3.forward*forcedFollowSize + markOffset;
            p2 = ForcedFollowCenter + Vector3.back*forcedFollowSize + markOffset;
            Handles.DrawLine(p1, p2);
            
            Handles.color = new Color(0.2f, 0.1f, 0.9f);
            p1 = ProhibitFollowCenter + new Vector3(1,1,1)*prohibitFollowSize + markOffset;
            p2 = ProhibitFollowCenter + new Vector3(-1,-1,-1)*prohibitFollowSize + markOffset;
            Handles.DrawLine(p1, p2);

            p1 = ProhibitFollowCenter + new Vector3(-1,1,1)*prohibitFollowSize + markOffset;
            p2 = ProhibitFollowCenter + new Vector3(1,-1,-1)*prohibitFollowSize + markOffset;
            Handles.DrawLine(p1, p2);

            p1 = ProhibitFollowCenter + new Vector3(0,-1,1)*prohibitFollowSize*1.02f + markOffset;
            p2 = ProhibitFollowCenter + new Vector3(0,1,-1)*prohibitFollowSize*1.02f + markOffset;
            Handles.DrawLine(p1, p2);


            Handles.color = Color.yellow;
            Handles.DrawWireDisc(TargetOffsetPos, Vector3.up, targetMarkSize);
            Handles.DrawWireDisc(TargetOffsetPos, Vector3.forward, targetMarkSize);
            Handles.DrawWireDisc(TargetOffsetPos, Vector3.right, targetMarkSize);

            Handles.BeginGUI();//这样可以让渲染效果保持在最前方
            Handles.EndGUI();
        }
    }
#endif
}
