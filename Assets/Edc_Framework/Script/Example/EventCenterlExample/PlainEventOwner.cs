using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlainEventOwner
{
    public void AddEvent(){
        Hub.EventCenter.AddListener<int>(EventName.testEventName, OnEvent);
    }

    public void RemoveListener(){
        Hub.EventCenter.RemoveListener<int>(EventName.testEventName, OnEvent);
    }

    private void OnEvent(int value){
        Debug.Log($"PlainEventOwner 检测到事件触发，传入参数是：{value}");
    }
}
