using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum KeyUpdateResultType{
    Success,                    //无冲突，修改成功
    DisableKey,                 //禁用的按键
    ModifiableConflicts,        //冲突但可修改
    UnmodifiableConflicts,      //冲突但不可修改
    SameDefinition,             //定义的快捷键与原来一样
}

public class InputManager : BaseIOCComponent, IBindEvent
{
    private static HashSet<KeyCode> immutableKey;
    private static Dictionary<KeyCode, Key> changeableKey;
    private static Dictionary<InputEnum, Key> changeableKeyInfo;
    private static InputSetting inputSetting;

    protected override void Init()
    {
        var resourcePath = new ResourcePath("InputSetting","Assets/Edc_Framework/Sources/AssetFile/FrameworkSetting/Input/InputSetting.asset");
        inputSetting = Hub.Resources.GetScriptableobject<InputSetting>(resourcePath);
        immutableKey = new HashSet<KeyCode>();
        changeableKey = new Dictionary<KeyCode, Key>();
        changeableKeyInfo = new Dictionary<InputEnum, Key>();//如果数据持久化功能做好，则需要判断是否有这个保存数据，没有的话再创建
    }

    /// <summary>
    /// 获取可修改键位的按键信息
    /// </summary>
    public Key GetChangeableKeyInfo(InputEnum inputEnum){
        return changeableKeyInfo[inputEnum];
    }

    /// <summary>
    /// 尝试修改按键并返回修改结果
    /// </summary>
    public KeyUpdateResultType TryUpdateNewKey(Key targetKey, KeyCode newKey, out Key conflictsKey){
        conflictsKey = null;
        if(newKey == targetKey.CurrentKey){
            return KeyUpdateResultType.SameDefinition;
        }
        if(IsDisableKey(newKey)){
            return KeyUpdateResultType.DisableKey;
        }
        if(IsImmutableKeyConflicts(newKey)){
            return KeyUpdateResultType.UnmodifiableConflicts;
        }

        var keyConflictsState = CheckKeyConflicts(newKey);
        if(keyConflictsState == null){
            UpdateKey(targetKey.CurrentKey, newKey, targetKey);
            targetKey.UpdateKey(newKey);
            return KeyUpdateResultType.Success;
        }
        else{
            conflictsKey = keyConflictsState;
            return KeyUpdateResultType.ModifiableConflicts;
        }
    }

    /// <summary>
    /// 交换按键
    /// </summary>
    public void SwapSKey(Key swap1, Key swap2){
        if(immutableKey.Contains(swap1.CurrentKey)||
           immutableKey.Contains(swap2.CurrentKey)){
            LogManager.LogError("交换的按键其中一个是不可修改按键");
            return;
        }
        var swap1Key = swap1.CurrentKey;
        var swap2Key = swap2.CurrentKey;
        changeableKey[swap1Key] = swap2;
        changeableKey[swap2Key] = swap1;
        swap1.UpdateKey(swap2Key);
        swap2.UpdateKey(swap1Key);
    }

    /// <summary>
    /// 重置按键
    /// </summary>
    public void ResetKey(){
        var shortcutKeyList = new List<Key>(changeableKey.Count);
        foreach (var item in changeableKey.Values)
        {
            item.ResetKey();
            shortcutKeyList.Add(item);
        }
        changeableKey.Clear();
        foreach (var item in shortcutKeyList)
        {
            changeableKey.Add(item.CurrentKey, item);
        }
    }

    /// <summary>
    /// 更新的按键是否是禁用按键
    /// </summary>
    public bool IsDisableKey(KeyCode newKey){
        return inputSetting.IsDisableKey(newKey);
    }

    /// <summary>
    /// 更新的按键是否与不可变按键冲突
    /// </summary>
    private bool IsImmutableKeyConflicts(KeyCode newKey){
        return immutableKey.Contains(newKey);
    }

    /// <summary>
    /// 检查按键冲突
    /// </summary>
    private Key CheckKeyConflicts(KeyCode newKey){
        if(changeableKey.ContainsKey(newKey)){
            return changeableKey[newKey];
        }
        else{
            return null;
        }
    }

    /// <summary>
    /// 更新按键
    /// </summary>
    private void UpdateKey(KeyCode oldKey, KeyCode newKey, Key index){
        changeableKey.Remove(oldKey);
        changeableKey.Add(newKey, index);
    }

    public Key GetKey(InputEnum inputEnum){
        var inputInfo = inputSetting.GetInputInfo(inputEnum);
        if(inputInfo.isRemappable){
            var key = new Key(inputInfo.keyCode, inputInfo.keyCode);
            changeableKey.Add(inputInfo.keyCode, key);
            changeableKeyInfo.Add(inputEnum, key);
            return key;
        }
        else{
            immutableKey.Add(inputInfo.keyCode);
            return new Key(inputInfo.keyCode);
        }
    }

    public Mouse GetMouse(InputEnum inputEnum){
        var inputInfo = inputSetting.GetInputInfo(inputEnum);
        return new Mouse(inputInfo.button);
    }

    public AxisRaw GetInputInfo(InputEnum inputEnum){
        var inputInfo = inputSetting.GetInputInfo(inputEnum);
        return new AxisRaw(inputInfo.axisName);
    }

    public Axis GetAxis(InputEnum inputEnum){
        var inputInfo = inputSetting.GetInputInfo(inputEnum);
        return new Axis(inputInfo.axisName);
    }
    
    public bool GetKey(KeyCode keyCode){
        return Hub.Interaction.InteractionState ? Input.GetKey(keyCode) : false;
    }

    public bool GetKeyDown(KeyCode keyCode){
        return Hub.Interaction.InteractionState ? Input.GetKeyDown(keyCode) : false;
    }

    public bool GetKeyUp(KeyCode keyCode){
        return Hub.Interaction.InteractionState ? Input.GetKeyUp(keyCode) : false;
    }

    public bool GetMouseButton(int button){
        return Hub.Interaction.InteractionState ? Input.GetMouseButton(button) : false;
    }

    public bool GetMouseButtonDown(int button){
        return Hub.Interaction.InteractionState ? Input.GetMouseButtonDown(button) : false;
    }

    public bool GetMouseButtonUp(int button){
        return Hub.Interaction.InteractionState ? Input.GetMouseButtonUp(button) : false;
    }

    public float GetAxisRaw(string axisName){
        return Hub.Interaction.InteractionState ? Input.GetAxisRaw(axisName) : 0;
    }

    public float GetAxis(string axisName){
        return Hub.Interaction.InteractionState ? Input.GetAxis(axisName) : 0;
    }
}
