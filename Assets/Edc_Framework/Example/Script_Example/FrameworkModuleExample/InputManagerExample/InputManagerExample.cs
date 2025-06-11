using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerExample : MonoBehaviour
{
    public Button UpdateShortcutKeyBtn;
    public Button SwapShortcutKey;
    public Button ResetShortcutKey;
    public KeyCode updateKey;
    private Key pointA;
    private Key pointB;
    private Key pointC;

    private void Start()
    {
        pointA = Hub.Input.GetKey(InputEnum.MoveForward);
        pointB = Hub.Input.GetKey(InputEnum.MoveBackward);
        pointC = Hub.Input.GetKey(InputEnum.MoveLeft);
        pointA.SetAction(PrintAInformation);
        pointB.SetAction(PrintBInformation);
        pointC.SetAction(up:PrintCInformation);
        UpdateShortcutKeyBtn.onClick.AddListener(ClickUpdateShortcutKey);
        SwapShortcutKey.onClick.AddListener(ClickSwapShortcutKey);
        ResetShortcutKey.onClick.AddListener(ClickResetShortcutKey);
    }

    private void Update() {
        pointA.GetKeyDown();
        pointB.GetKeyDown();
        pointC.GetKeyUp();
    }
    
    private void ClickUpdateShortcutKey(){
        var result = Hub.Input.TryUpdateNewKey(pointA, updateKey, out Key conflictsKey);
        switch (result)
        {
            case KeyUpdateResultType.Success:
                Debug.Log("修改成功");
            break;
            case KeyUpdateResultType.SameDefinition:
                Debug.Log("重复定义");
            break;
            case KeyUpdateResultType.UnmodifiableConflicts:
                Debug.Log("冲突但不可修改");
            break;
            case KeyUpdateResultType.ModifiableConflicts:
                Debug.Log("冲突但可修改，执行交换键位逻辑");
                Hub.Input.SwapSKey(pointA, conflictsKey);
            break;
        }
    }

    private void ClickSwapShortcutKey(){
        Hub.Input.SwapSKey(pointA, pointB);
    }

    private void ClickResetShortcutKey(){
        Hub.Input.ResetKey();
    }

    /// <summary>
    /// 打印A信息
    /// </summary>
    private void PrintAInformation(){
        Debug.Log("A");
    }

    /// <summary>
    /// 打印B信息
    /// </summary>
    private void PrintBInformation(){
        Debug.Log("B");
    }

    /// <summary>
    /// 打印C信息
    /// </summary>
    private void PrintCInformation(){
        Debug.Log("C");
    }

}
