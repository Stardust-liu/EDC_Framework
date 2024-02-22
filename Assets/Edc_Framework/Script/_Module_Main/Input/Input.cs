using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key
{
    private KeyCode defaultKey;
    private KeyCode currentKey;
    private Action down;
    private Action longPress;
    private Action up;
    public KeyCode CurrentKey {get{return currentKey; }}

    public Key(KeyCode defaultKey, KeyCode currentKey){
        this.defaultKey = defaultKey;
        this.currentKey = currentKey;
    }

    public Key(KeyCode defaultKey){
        this.defaultKey = defaultKey;
        this.currentKey = defaultKey;
    }

    public void SetAction(Action down = null, Action up = null, Action longPress = null){
        this.down = down;
        this.longPress = longPress;
        this.up = up;
    }

    public void GetKeyDown(){
        if(Hub.Input.GetKeyDown(currentKey)){
            down.Invoke();
        }
    }

    public void GetKey(){
        if(Hub.Input.GetKey(currentKey)){
            longPress.Invoke();
        }
    }

    public void GetKeyUp(){
        if(Hub.Input.GetKeyUp(currentKey)){
            up.Invoke();
        }
    }

    /// <summary>
    /// 重置按键
    /// </summary>
    public void ResetKey(){
        currentKey = defaultKey;
    }

    /// <summary>
    /// 更新按键
    /// </summary>
    public void UpdateKey(KeyCode newKey){
        currentKey = newKey;
    }
}

public class Mouse{
    private int button;
    private Action down;
    private Action longPress;
    private Action up;

    public Mouse (int button){
        this.button = button;
    }

    public void SetAction(Action down = null, Action up = null, Action longPress = null){
        this.down = down;
        this.longPress = longPress;
        this.up = up;
    }

    public void GetMouseButtonDown(){
        if(Hub.Input.GetMouseButtonDown(button)){
            down.Invoke();
        }
    }

    public void GetMouseButton(){
        if(Hub.Input.GetMouseButton(button)){
            longPress.Invoke();
        }
    }

    public void GetMouseButtonUp(){
        if(Hub.Input.GetMouseButtonUp(button)){
            up.Invoke();
        }
    }
}

public class AxisRaw : BaseAxis{
    public AxisRaw(string axisNmae):base(axisNmae){}
    public void GetAxisRaw(){
        axisInput.Invoke(Hub.Input.GetAxisRaw(axisNmae));
    }
}

public class Axis : BaseAxis{
    public Axis(string axisNmae):base(axisNmae){}
    public void GetAxis(){
        axisInput.Invoke(Hub.Input.GetAxis(axisNmae));
    }
}

public class BaseAxis{
    protected string axisNmae;
    protected Action<float> axisInput;
    public void SetAction(Action<float> axisInput){
        this.axisInput = axisInput;
    }

    public BaseAxis(string axisNmae){
        this.axisNmae = axisNmae;
    }
}
