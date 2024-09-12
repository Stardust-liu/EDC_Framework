using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModule
{
    private static CameraManagerModule cameraManager;
    public static CameraManagerModule CameraManager{get{return cameraManager; }}

    public static void Init(){
        cameraManager = new CameraManagerModule();
    }
}
