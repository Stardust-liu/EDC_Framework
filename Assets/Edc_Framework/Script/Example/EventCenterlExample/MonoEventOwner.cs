using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoEventOwner : MonoBehaviour
{
    public void AddEvent(){
        Hub.EventCenter.AddListener(EventName.testEventName, OnEvent);
    }

    public void RemoveListener(){
        Hub.EventCenter.RemoveListener(EventName.testEventName, OnEvent);
    }

    public void Destroy(){
        Destroy(this.gameObject);
    }

    private void OnEvent(){
        Debug.Log("MonoEventOwner 检测到事件触发");
    }
}
