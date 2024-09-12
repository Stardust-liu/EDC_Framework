using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCamera<T> : MonoBehaviour, ICameraControl,ILateUpdate
where T : BaseCamera<T>
{
    public static T Instance;
    public bool isEnableCamera;
    public Vector3 offset;
    public Transform cameraTransform;
    public Vector3 CameraPos{get{return cameraTransform.position;}}

    protected virtual void Awake() {
        Instance = GetComponent<T>();
        if(FrameworkManager.isInitFinish){
            var position = cameraTransform.position;
            var rotation = cameraTransform.rotation;
            cameraTransform = FrameworkManager.MainCamera.transform;
            cameraTransform.position = position;
            cameraTransform.rotation = rotation;
        }
    }

    protected virtual void OnDestroy()
    {
        if(FrameworkManager.isInitFinish && isEnableCamera){
            DisableCamera();
        }
    }

    public virtual void EnableCamera(){
        Hub.Update.AddLateUpdate(Instance);
        isEnableCamera = true;
    }

    public virtual void DisableCamera(){
        Hub.Update.RemoveLateUpdate(Instance);
        isEnableCamera = false;
    }

    public abstract void OnLateUpdate();

    public void SetCameraOffset(Vector3 offset){
        this.offset = offset;
    }
}
