using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InputInfo{
    [LabelText("是否可修改键位")]
    [GUIColor(0.65f, 1, 0.65f)]
    [ShowIf("inputType", InputType.Key)]
    public bool isRemappable;
    public InputType inputType;

    [ShowIf("inputType", InputType.Mouse)]
    public int button;

    [ShowIf("inputType", InputType.Key)]
    public KeyCode keyCode;

    [ShowIf("CheckIsDisplayAxisName")]
    public string axisName;

    /// <summary>
    /// 检查是否需要显示axisName
    /// </summary>
    private bool CheckIsDisplayAxisName(){
        if(inputType == InputType.Axis || inputType == InputType.AxisRaw){
            return true;
        }
        else{
            return false;
        }
    }
}

public enum InputType{
    Key,
    Mouse,
    Axis,
    AxisRaw,
}

[CreateAssetMenu(fileName = "InputSetting", menuName = "创建.Assets文件/InputSetting")]
public class InputSetting : SerializedScriptableObject
{
   [LabelText("按键信息")]
   [OnValueChanged("CheckIsCompliance", true)]
    public Dictionary<InputEnum, InputInfo> keyInfoDictionary;

   [LabelText("禁用的按键")]
   [GUIColor(1, 0.6f, 0.6f)]
    public HashSet<KeyCode> disableKey;

    /// <summary>
    /// 检查是否是不可使用按键
    /// </summary>
    public bool IsDisableKey(KeyCode key){
        return disableKey.Contains(key);
    }

    /// <summary>
    /// 获取按键信息
    /// </summary>
    public InputInfo GetInputInfo(InputEnum inputEnum){
        return keyInfoDictionary[inputEnum];
    }

    private void CheckIsCompliance(){
        foreach (var item in keyInfoDictionary)
        {
            if(item.Value == null){
                continue;
            }
            if(item.Value.inputType != InputType.Axis || item.Value.inputType != InputType.AxisRaw){
                item.Value.axisName = null;
            }
            if(item.Value.inputType == InputType.Key && IsDisableKey(item.Value.keyCode)){
                LogManager.LogError($"按键 {item.Key} 中含有禁用按键");
            }
        }
    }
}
