using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTime
{
    public const float fast = 0.15f;
    public const float mediumSpeed = 0.3f;
    public const float slow = 0.5f;
    public const float viewGradientTime = 0.3f;//view渐变时间
    public static Dictionary <int, WaitForSeconds> waitForSecondsDictionary = new();

    /// <summary>
    /// 获取协程等待时间
    /// </summary>
    public static WaitForSeconds GetWait(float seconde){
        var integer = (int)(seconde *1000);
        if(!waitForSecondsDictionary.ContainsKey(integer)){
            waitForSecondsDictionary.Add(integer, new WaitForSeconds(seconde));
        }
        return waitForSecondsDictionary[integer];
    }
}
