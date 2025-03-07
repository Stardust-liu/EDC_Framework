using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraModule : BaseIOCComponent
{
    private ICameraControl currentControlCamera;
    private CameraFollow cameraFollow = CameraFollow.Instance;
    private CameraTargetLerp cameraNarrative = CameraTargetLerp.Instance;

    /// <summary>
    /// 启用跟随摄像机
    /// </summary>
    public void EnableFollowCamera(){
        currentControlCamera?.DisableCamera();
        currentControlCamera = cameraFollow;
        currentControlCamera.EnableCamera();
    }

    /// <summary>
    /// 启用叙事摄像机
    /// </summary>
    public void EnableNarrationCamera(){
        currentControlCamera?.DisableCamera();
        currentControlCamera = cameraNarrative;
        currentControlCamera.EnableCamera();
    }

    protected override void Init(){}
}
