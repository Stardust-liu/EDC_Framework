using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class ParallaxEffectGroup : MonoBehaviour, ILateUpdate, ISerializationCallbackReceiver, ISupportsPrefabSerialization
{
    public class ParallaxEffectInfo{
        public Transform target;
        public Vector2 offset;
    }
    public ParallaxEffectInfo[] parallaxEffectArray;

    [OnValueChangedAttribute("ChangeAllOffset")]
    [Range(0.5f,1.5f), LabelText("全局 水平 偏移量")]
    public float allOffsetX = 1;
    [OnValueChangedAttribute("ChangeAllOffset")]
    [Range(0.5f,1.5f), LabelText("全局 垂直 偏移量")]
    public float allOffsety = 1;
    private Transform cameraTransform;
    private Vector3 movement;
    private Vector3 lastCameraPos;

    private void Start() {
        markOffset = null;
        cameraTransform = FrameworkManager.MainCamera.transform;
        lastCameraPos = cameraTransform.position;
        UpdateManager.AddLateUpdate(this);
    }

    private void OnDestroy()
    {
        if(FrameworkManager.isInitFinish){
            UpdateManager.RemoveLateUpdate(this);
        }
    }

    public void OnLateUpdate()
    {
        movement = cameraTransform.position - lastCameraPos;
        foreach (var item in parallaxEffectArray)
        {
            var targetPos = new Vector3(movement.x * item.offset.x, movement.y * item.offset.y);
            item.target.position += targetPos;
        }
        lastCameraPos = cameraTransform.position;
    }

    [ReadOnly]
    [LabelText("标记偏移量")]
    public Vector2[] markOffset;

    [Button("重置并重新标记当前偏移量"), GUIColor(0.8f, 0.3f, 0.3f)]
    public void ResetOffset(){
        var temporaryOffset = new Vector2[parallaxEffectArray.Length];
        var length = parallaxEffectArray.Length;
        for (int i = 0; i < length; i++)
        {
            temporaryOffset[i] = parallaxEffectArray[i].offset;
        } 

        allOffsety = 1;
        allOffsetX = 1;
        markOffset = new Vector2[parallaxEffectArray.Length];
        for (int i = 0; i < length; i++)
        {
            parallaxEffectArray[i].offset = temporaryOffset[i];
            markOffset[i] = parallaxEffectArray[i].offset;
        }
    }


    private void ChangeAllOffset(){
        if(parallaxEffectArray == null ||
           parallaxEffectArray.Length != markOffset.Length){
            LogManager.LogError("标记偏移量数组数据和parallaxEffectArray数据对应不上，需要点击重置并重新标记全局偏移量，才能正常调整");
            return;
        }
        var length = parallaxEffectArray.Length;
        for (int i = 0; i < length; i++)
        {
            parallaxEffectArray[i].offset = new Vector2(markOffset[i].x * allOffsetX, markOffset[i].y * allOffsety);
        }
    }

    [SerializeField, HideInInspector]
    private SerializationData serializationData;
    SerializationData ISupportsPrefabSerialization.SerializationData 
    { 
        get { return serializationData; } 
        set { serializationData = value; } 
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref serializationData);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref serializationData);
    }
}
