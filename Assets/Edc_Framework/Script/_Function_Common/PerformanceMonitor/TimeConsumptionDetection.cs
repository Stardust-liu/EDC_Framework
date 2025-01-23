using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TimeConsumptionDetection : MonoBehaviour
{
# if PERFORMANCEMONITOR_MODULE
    private readonly Stopwatch awakeStopwatch = new ();
    private readonly Stopwatch startStopwatch = new ();
    private readonly Stopwatch updateStopwatch = new ();
    private readonly Stopwatch fixedUpdateStopwatch = new ();
    private readonly Stopwatch onEnableStopwatch = new ();
    private readonly Stopwatch onDisableStopwatch = new ();
    private readonly Stopwatch onDestroyStopwatch = new ();
    private string className;
    private string ClassName{
        get{
            if(className == null){
                className = GetType().Name;
            }
            return className;
        }
    }
    #endif

    protected void ProxyAwake(){
# if PERFORMANCEMONITOR_MODULE
        CheckIsInvokeRecordFunction(awakeStopwatch, HandleAwake, TimeConsumptionController.RecordAwakeTime);
#else
        HandleAwake();
#endif
    }
    
    protected void ProxyStart(){
# if PERFORMANCEMONITOR_MODULE
        CheckIsInvokeRecordFunction(startStopwatch, HandleStart, TimeConsumptionController.RecordStartTime);
#else
        HandleStart();
#endif
    }

    protected void ProxyUpdate(){
# if PERFORMANCEMONITOR_MODULE
        CheckIsInvokeRecordFunction(updateStopwatch, HandleUpdate, TimeConsumptionController.RecordUpdateTime);
#else
        HandleUpdate();
#endif
    }

    protected void ProxyFixedUpdate() {
# if PERFORMANCEMONITOR_MODULE
        CheckIsInvokeRecordFunction(fixedUpdateStopwatch, HandleFixedUpdate, TimeConsumptionController.RecordFixedUpdateTime);
#else
        HandleFixedUpdate();
#endif
    }


    protected void ProxyOnEnable(){
# if PERFORMANCEMONITOR_MODULE
        CheckIsInvokeRecordFunction(onEnableStopwatch, HandleOnEnable, TimeConsumptionController.RecordOnEnableTime);
#else
        HandleOnEnable();
#endif

    }

    protected void ProxyOnDisable(){
# if PERFORMANCEMONITOR_MODULE
        CheckIsInvokeRecordFunction(onDisableStopwatch, HandleOnDisable, TimeConsumptionController.RecordOnDisableTime);
#else
        HandleOnDisable();
#endif
    }

    protected void ProxyOnDestroy(){
# if PERFORMANCEMONITOR_MODULE
        CheckIsInvokeRecordFunction(onDestroyStopwatch, HandleOnDestroy, TimeConsumptionController.RecordOnDestroyTime);
#else
        HandleOnDestroy();
#endif
    }

# if PERFORMANCEMONITOR_MODULE
    private void CheckIsInvokeRecordFunction(Stopwatch stopwatch, Action callBack,  Action<string, long> recordFunction){
        if(TimeConsumptionController.instance != null && TimeConsumptionController.instance.IsRecording){
            stopwatch.Restart();
            callBack.Invoke();
            recordFunction.Invoke(ClassName, stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
        }
        else{
            callBack.Invoke();
        }
    }
#endif


    protected virtual void HandleAwake(){}
    protected virtual void HandleStart(){}
    protected virtual void HandleUpdate(){}
    protected virtual void HandleFixedUpdate(){}
    protected virtual void HandleOnEnable(){}
    protected virtual void HandleOnDisable(){}
    protected virtual void HandleOnDestroy(){}
}

