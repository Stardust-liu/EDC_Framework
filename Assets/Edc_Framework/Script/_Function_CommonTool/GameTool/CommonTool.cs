using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CommonTool
{
    /// <summary>
    /// 字符串转换为对象数据
    /// </summary>
    public static T StringToClass<T>(string value) where T : BaseStringParser{
        var obj = Activator.CreateInstance<T>();
        obj.Init(StringSplit(value, "#"));
        return obj;
    }

    /// <summary>
    /// 字符串转换为对象数据
    /// </summary>
    public static T StringToClass<T>(string[] data) where T : BaseStringParser{
        var obj = Activator.CreateInstance<T>();
        obj.Init(data);
        return obj;
    }

    /// <summary>
    /// 拆分字符串
    /// </summary>
    public static string[] StringSplit(string value, string delimiter){
        return value.Split(delimiter, System.StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>
    /// 获取触摸点的世界空间位置
    /// </summary>
    public static Vector3 GetMouseWorldPos(float depth){
        var mousePonit = GetMousePonit();
        mousePonit.z = depth;
        return ScreenToWorld(mousePonit);
    }

    /// <summary>
    /// 获取第一根手指的世界空间位置
    /// </summary>
    public static Vector3 Touch0WorldPos(float depth = 0){
        var mousePonit = (Vector3)Input.GetTouch(0).position;
        mousePonit.z = depth;
        return ScreenToWorld(mousePonit);
    }

    /// <summary>
    /// 获取第二根手指的世界空间位置
    /// </summary>
    public static Vector3 Touch1WorldPos(float depth = 0){
        var mousePonit = (Vector3)Input.GetTouch(1).position;
        mousePonit.z = depth;
        return ScreenToWorld(mousePonit);
    }

    /// <summary>
    /// 获取触摸的屏幕空间位置
    /// </summary>
    public static Vector3 GetMousePonit(){
        if (Input.touchCount == 1)
        {
            // 返回第一个触摸点的位置
            return Input.GetTouch(0).position;
        }
        else if(Input.touchCount > 1){
            return (Input.GetTouch(0).position +  Input.GetTouch(1).position) * 0.5f;
        }
        // 如果是PC或没有触摸点，返回鼠标位置
        return Input.mousePosition;
    }

    /// <summary>
    /// 检测是否开始拖拽
    /// </summary>
    public static bool IsStartDrag(){
        if(Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began){
            return true;
        }
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 检测是否正在拖拽中
    /// </summary>
    public static bool IsDrag(){
        if(Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 检测拖拽是否结束
    /// </summary>
    public static bool IsDragEnd(){
        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 开始缩放
    /// </summary>
    public static bool IsStartZoom(){
        if(Input.GetAxisRaw("Mouse ScrollWheel") != 0 || (Input.touchCount >= 2 && Input.GetTouch(1).phase == TouchPhase.Began)){
            return true;
        }
        else{
            return false;
        }
    }

    /// <summary>
    /// 缩放
    /// </summary>
    public static bool IsZoom(){
        return Input.touchCount >= 2;
    }

    /// <summary>
    /// 屏幕坐标转世界坐标
    /// </summary>
    public static Vector3 ScreenToWorld(Vector3 ScreenPos, float depth = 0){
        ScreenPos.z = depth;
        return FrameworkManager.MainCamera.ScreenToWorldPoint(ScreenPos);
    }

    /// <summary>
    /// 屏幕坐标转UI坐标
    /// </summary>
    public static Vector3 ScreenToUI(Vector3 ScreenPos, RectTransform rectParent){
        var camera = FrameworkManager.UiCamera;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectParent, ScreenPos, camera, out Vector3 anchoredPos);
        return anchoredPos;
    }

    /// <summary>
    /// 世界坐标转UI坐标
    /// </summary>
    public static Vector3 WorldToUI(Vector3 worldPos, RectTransform rectParent){
        var screenPos = FrameworkManager.MainCamera.WorldToScreenPoint(worldPos);
        return ScreenToUI(screenPos, rectParent);
    }

    /// <summary>
    /// UI坐标转世界坐标
    /// </summary>
    public static Vector3 UIToWorld(Vector3 uiPos, float depth = 0){
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(FrameworkManager.UiCamera, uiPos);
        return ScreenToWorld(screenPos, depth);
    }
}
