using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class CameraTargetLerp : BaseCamera<CameraTargetLerp>
{
    [BoxGroup("缓动相关"), LabelText("水平缓动因子"), Range(0.01f, 1f)]
    public float damping = 0.5f;
    [BoxGroup("参考标记相关")][Range(0.05f, 0.5f)]
    public float targetMarkSize = 0.2f;
    private LinkedList<Transform> target = new LinkedList<Transform>();
    private Transform target1;
    private Transform target2;
    private float lerp;
    private Vector3 followTarget;
    private Vector3 followPos;

    public override void OnLateUpdate()
    {
        Follow();
    }

    public void Follow(){
        if(target1 && target2 != null){
            followTarget = Vector3.Lerp(target1.position, target2.position, lerp);
            followPos = followTarget + offset;
            followPos = Vector3.Lerp(CameraPos, followPos, damping * Time.deltaTime);
            cameraTransform.position = followPos;
        }
    }

    /// <summary>
    /// 跟随第一个目标
    /// </summary>
    public void FollowFirstTarget(){
        target1 = target2 = target.First.Value;
    }

    /// <summary>
    /// 跟随最后一个目标
    /// </summary>
    public void FollowLastTarget(){
        target1 = target2 = target.Last.Value;
    }

    /// <summary>
    /// 跟随第一个目标与最后一个目标之间的插值
    /// </summary>
    public void FollowFirstLastTarget(float lerp){
        target1 = target.First.Value;
        target2 = target.Last.Value;
        this.lerp = lerp;
    }

    /// <summary>
    /// 跟随末尾2个目标间的插值
    /// </summary>
    public void FollowLasttwoTarget(float lerp){
        target1 = target.Last.Value;
        target2 = target.Last.Previous.Value;
        this.lerp = lerp;
    }

    /// <summary>
    /// 添加目标
    /// </summary>
    public void AddTarget(Transform transform){
        target.AddLast(transform);
    }

    /// <summary>
    /// 移除目标
    /// </summary>
    public void RemoveTarget(Transform transform){
        target.Remove(transform);
    }

    /// <summary>
    /// 移除最后一个目标
    /// </summary>
    public void RemoveLastTarget(Transform transform){
        target.RemoveLast();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(followTarget, Vector3.up, targetMarkSize);
        Handles.DrawWireDisc(followTarget, Vector3.forward, targetMarkSize);
        Handles.DrawWireDisc(followTarget, Vector3.right, targetMarkSize);
        Handles.BeginGUI();//这样可以让渲染效果保持在最前方
        Handles.EndGUI();
    }
#endif
}
