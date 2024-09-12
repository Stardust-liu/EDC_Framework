using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ParsCsv<T> : SingleInstance<T> where T: class, new()
{ 
    /// <summary>
    /// CSV文件第一行标签
    /// </summary>
    private Dictionary<string, int> titleData;
    private Dictionary<int, List<string>> originalData;
    protected int CurrentReadRow {get; private set;}
    protected int CurrentReadDataRow {get {return CurrentReadRow - 3;}}
    protected string FirstColumnInfo {get; private set;}
    protected TextAsset textAsset;
    protected ParsCsv(){}
    protected ParsCsv(TextAsset csv){
        ParseData(csv);
    }

    public virtual void ParseData(TextAsset csv){
        titleData = new Dictionary<string, int>();
        originalData = new Dictionary<int, List<string>>();
        textAsset = csv;
        CurrentReadRow = 1;
        var isIgnore = false;
        var allData = csv.text;
        var data = new StringBuilder();
        var everyLineData = new List<string>();
        foreach (var item in allData)
        {
            if(item == '"'){
                //当单元格内出现特殊符号时会自动用"号在头尾标记（如,号或换行）
                //因此当isIgnore为false时就表示标记结束
                isIgnore = !isIgnore;
            }
            if((item == ','||item =='\n')&&!isIgnore){
                if(CurrentReadRow != 2){
                    everyLineData.Add(data.ToString());
                }
                if(item =='\n'){
                    if(CurrentReadRow == 1){//读取完了第一行的数据
                        var count = everyLineData.Count;
                        for (int i = 0; i < count; i++)
                        {
                            titleData.Add(everyLineData[i], i);
                        }
                    }
                    else if(CurrentReadRow != 2){
                        originalData.Add(CurrentReadRow, new List<string>(everyLineData));
                    }
                    everyLineData.Clear();
                    CurrentReadRow++;
                }
                data.Clear();
                continue;
            }
            if(item != '\r'&& CurrentReadRow != 2){//CSV每行末尾都会自动生成一个换行符，这样做可以防止数据末尾有不应该出现的空格
                data.Append(item);
            }
        }
        foreach (var item in originalData.Keys)
        {
            CurrentReadRow = item;
            FirstColumnInfo = originalData[item][0];
            SetData();
        }
        textAsset = null;
        titleData = null;
        originalData = null;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    protected abstract void SetData();

    protected int GetInt(string columnsKey){
        var data = GetData(columnsKey);
        if(string.IsNullOrEmpty(data)){
            return 0;
        }
        else{
            return int.Parse(data);
        }
    }

    protected float GetFloat(string columnsKey){
        var data = GetData(columnsKey);
        if(string.IsNullOrEmpty(data)){
            return 0;
        }
        else{
            return float.Parse(data);
        }
    }

    protected bool GetBool(string columnsKey){
        var data = GetData(columnsKey);
        if(string.IsNullOrEmpty(data)){
            return false;
        }
        else{
            return bool.Parse(data);
        }
    }

    protected string GetString(string columnsKey){
        return GetData(columnsKey);
    }

    protected T3 GetEnum<T3>(string columnsKey){
        return (T3)Enum.Parse(typeof(T3), GetData(columnsKey));
    }

    private string GetData(string columnsKey){
        return originalData[CurrentReadRow][titleData[columnsKey]];
    }
}
