using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCenterExtensions
{
    public static void OnEvent_TestEventName(this EventCenter eventCenter, int value){
        eventCenter.OnEvent(EventName.testEventName);        
        eventCenter.OnEvent(EventName.testEventName, value);        
    }
}
