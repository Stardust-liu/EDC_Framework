using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetLerpExample : MonoBehaviour
{
    public Transform a;
    public Transform b;
    public Transform c;
    public CameraTargetLerp cameraTargetLerp;
    public CustomClickEffectsBtn followFirstTarget;
    public CustomClickEffectsBtn followLastTarget;
    public CustomClickEffectsBtn followFirstLastTarget;
    public CustomClickEffectsBtn followLasttwoTarget;
    
    void Start()
    {
        cameraTargetLerp.AddTarget(a);
        cameraTargetLerp.AddTarget(b);
        cameraTargetLerp.AddTarget(c);
        cameraTargetLerp.EnableCamera();
        followFirstTarget.onClickEvent.AddListener(FollowFirstTarget);
        followLastTarget.onClickEvent.AddListener(FollowLastTarget);
        followFirstLastTarget.onClickEvent.AddListener(FollowFirstLastTarget);
        followLasttwoTarget.onClickEvent.AddListener(FollowLasttwoTarget);
    }

    private void FollowFirstTarget(){
        cameraTargetLerp.FollowFirstTarget();
    }

    private void FollowLastTarget(){
        cameraTargetLerp.FollowLastTarget();
    }

    private void FollowFirstLastTarget(){
        cameraTargetLerp.FollowFirstLastTarget(0.5f);
    }

    private void FollowLasttwoTarget(){
        cameraTargetLerp.FollowLasttwoTarget(0.5f);
    }
}
