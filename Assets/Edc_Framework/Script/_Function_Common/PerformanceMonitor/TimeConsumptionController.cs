using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[System.Flags]
public enum FunctionType{
    Nothing,
    Everthing = Awake|Start|Update|FixedUpdate|OnEnable|OnDisable|OnDestroy|Custom,
    Awake = 1 << 1,
    Start = 1 << 2,
    Update = 1 << 3,
    FixedUpdate = 1 << 4,
    OnEnable = 1 << 5,
    OnDisable = 1 << 6,
    OnDestroy = 1 << 7,
    Custom = 1 << 8,
}

public enum PrintSortMethod{
    TimeRecord,//按照耗时排序
    InvokeCount//按照调用次数排序
}

public class InvokeInfo{
    public long allTimeRecord;
    public int invokeCount;
    public FunctionType functionType;

    public InvokeInfo(long _allTimeRecord, FunctionType _functionType){
        allTimeRecord = _allTimeRecord;
        functionType = _functionType;
        invokeCount = 1;
    }

    public void AddInfo(long _allTimeRecord){
        allTimeRecord += _allTimeRecord;
        invokeCount ++;
    }
}

public struct RecordingInfo{
    public FunctionType functionType;
    public string className;
    public string functionName;
    public long allTimeRecord;
    public int invokeCount;

    public RecordingInfo(string _className, string _functionName, InvokeInfo invokeInfo){
        className = _className;
        functionName = _functionName;
        allTimeRecord = invokeInfo.allTimeRecord;
        invokeCount = invokeInfo.invokeCount;
        functionType = invokeInfo.functionType;
    }
}

public class TimeConsumptionController : MonoBehaviour
{
    public static TimeConsumptionController instance;
    private readonly static Dictionary<string, Dictionary<string, InvokeInfo>> functionTimeRecord = new();
    private readonly static Dictionary<string, Dictionary<string, System.Diagnostics.Stopwatch>> customFunctionRecord = new();
    private static readonly List<RecordingInfo> recordedInfos = new();
    private static readonly List<RecordingInfo> printInfos = new();
    [HideInInspector]
    public bool isRecording;
    public bool IsRecording{get {return isRecording;}}

    [LabelText("需要记录的函数类型")]
    [SerializeField]
    private FunctionType needRecordFunctionType;

    [Button("开始记录", ButtonSizes.Medium), GUIColor(0.5f, 0.8f, 1f)]
    [HideIf("IsRecording")]
    public void StartRecording(){
        functionTimeRecord.Clear();
        recordedInfos.Clear();
        isRecording = true;
    }

    [Button("停止记录", ButtonSizes.Medium), GUIColor(1, 0.5f, 0.5f)]
    [ShowIf("IsRecording")]
    public void StopRecording(){
        isRecording = false;
    }

    [FoldoutGroup("打印设置", Order = 100)]
    [LabelText("排序方式"), SerializeField]
    private PrintSortMethod sortMethod;

    [FoldoutGroup("打印设置", Order = 100)]
    [LabelText("需要打印的函数类型"), SerializeField]
    private FunctionType needPrintFunctionType;

    [FoldoutGroup("打印设置", Order = 100)]
    [Button("打印信息", ButtonSizes.Medium)]
    [GUIColor(1f, 0.6f, 0.4f)]
    public void PrintInfo(){
        if(recordedInfos.Count == 0){
            foreach (var item in functionTimeRecord)
            {
                foreach (var InvokeInfoItem in item.Value)
                {
                    var invokeInfo = InvokeInfoItem.Value;
                    recordedInfos.Add(new RecordingInfo(item.Key, InvokeInfoItem.Key ,invokeInfo));
                }
            }
        }
        printInfos.Clear();
        foreach (var item in recordedInfos)
        {
            if(needPrintFunctionType.HasFlag(item.functionType)){
                printInfos.Add(item);
            }
        }
        if(sortMethod == PrintSortMethod.TimeRecord){
            Debug.Log("按照耗时排序");
            printInfos.Sort((x, y) => y.allTimeRecord.CompareTo(x.allTimeRecord));
        }
        else{
            Debug.Log("按调用次数排序");
            printInfos.Sort((x, y) => y.invokeCount.CompareTo(x.invokeCount));
        }
        foreach (var item in printInfos)
        {
            Debug.Log($"类名：{item.className} 方法名 ：{item.functionName} 耗时：{item.allTimeRecord} 调用次数：{item.invokeCount}");
        }
        Debug.Log("打印完成");
    }

    private void Awake(){
        instance = this;
        functionTimeRecord.Clear();
        recordedInfos.Clear();
    }

    public static void RecordAwakeTime(string className, long timeRecord){
        RecordTimeRecordInfo(FunctionType.Awake, className, "Awake", timeRecord);
    }
    public static void RecordStartTime(string className, long timeRecord){
        RecordTimeRecordInfo(FunctionType.Start, className, "Start", timeRecord);
    }
    public static void RecordUpdateTime(string className, long timeRecord){
        RecordTimeRecordInfo(FunctionType.Update, className, "Update", timeRecord);
    }
    public static void RecordFixedUpdateTime(string className, long timeRecord){
        RecordTimeRecordInfo(FunctionType.FixedUpdate, className, "FixedUpdate", timeRecord);
    }
    public static void RecordOnEnableTime(string className, long timeRecord){
        RecordTimeRecordInfo(FunctionType.OnEnable, className, "OnEnable", timeRecord);
    }
    public static void RecordOnDisableTime(string className, long timeRecord){
        RecordTimeRecordInfo(FunctionType.OnDisable, className, "OnDisable", timeRecord);
    }
    public static void RecordOnDestroyTime(string className, long timeRecord){
        RecordTimeRecordInfo(FunctionType.OnDestroy, className, "OnDestroy", timeRecord);
    }

    public static void StartRecordCustomFunctionTime(string className, string functionName){
        if(instance != null && instance.IsRecording){
            if(!customFunctionRecord.ContainsKey(className)){
                customFunctionRecord.Add(className, new Dictionary<string, System.Diagnostics.Stopwatch>());
                customFunctionRecord[className].Add(functionName, new System.Diagnostics.Stopwatch());
            }
            else{
                if(!customFunctionRecord[className].ContainsKey(functionName)){
                    customFunctionRecord[className].Add(functionName, new System.Diagnostics.Stopwatch());
                }
            }
            customFunctionRecord[className][functionName].Restart();
        }
    }

    public static void EndRecordCustomFunctionTime(string className, string functionName, FunctionType functionType = FunctionType.Custom){
        if(instance != null && instance.IsRecording){
            RecordTimeRecordInfo(functionType, className, functionName, customFunctionRecord[className][functionName].ElapsedMilliseconds);
            customFunctionRecord[className][functionName].Stop();
        }
    }

    private static void RecordTimeRecordInfo(FunctionType functionType, string className, string functionName, long timeRecord){
        if(!instance.needRecordFunctionType.HasFlag(functionType)){
            return;
        }
        if(!functionTimeRecord.ContainsKey(className)){
            functionTimeRecord.Add(className, new Dictionary<string, InvokeInfo>());
            functionTimeRecord[className].Add(functionName, new InvokeInfo(timeRecord, functionType));
        }
        else{
            if(!functionTimeRecord[className].ContainsKey(functionName)){
                functionTimeRecord[className].Add(functionName, new InvokeInfo(timeRecord, functionType));
            }
            else{
                functionTimeRecord[className][functionName].AddInfo(timeRecord);
            }
        }
    }
}
