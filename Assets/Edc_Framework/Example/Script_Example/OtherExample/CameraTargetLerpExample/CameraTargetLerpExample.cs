using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTargetLerpExample : MonoBehaviour
{
    public Transform a;
    public Transform b;
    public Transform c;
    public CameraTargetLerp cameraTargetLerp;
    public Button followFirstTarget;
    public Button followLastTarget;
    public Button followFirstLastTarget;
    public Button followLasttwoTarget;
    
    void Start()
    {
        cameraTargetLerp.AddTarget(a);
        cameraTargetLerp.AddTarget(b);
        cameraTargetLerp.AddTarget(c);
        cameraTargetLerp.EnableCamera();
        followFirstTarget.onClick.AddListener(FollowFirstTarget);
        followLastTarget.onClick.AddListener(FollowLastTarget);
        followFirstLastTarget.onClick.AddListener(FollowFirstLastTarget);
        followLasttwoTarget.onClick.AddListener(FollowLasttwoTarget);
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
