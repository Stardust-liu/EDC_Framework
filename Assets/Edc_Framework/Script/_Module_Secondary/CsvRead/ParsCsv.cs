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
    private Dictionary<string, List<string>> originalData;
    protected TextAsset textAsset;
    protected ParsCsv(){}
    protected ParsCsv(TextAsset csv){
        ParseData(csv);
    }

    public virtual void ParseData(TextAsset csv){
        titleData = new Dictionary<string, int>();
        originalData = new Dictionary<string, List<string>>();
        textAsset = csv;
        var isIgnore = false;
        var isGetFirstRowData = false;
        var allData = csv.text;
        var data = new StringBuilder();
        var everyLineData = new List<string>();
        foreach (var item in allData)
        {
            if(item == '"'){
                //当单元格内出现特殊符号时会自动用"号在头尾标记（如,号或换行）
                //因此当isIgnore为false时就表示标记结束
                isIgnore = !isIgnore? true : false;
            }
            if((item == ','||item =='\n')&&!isIgnore){
                everyLineData.Add(data.ToString());
                if(item =='\n'){
                    if(!isGetFirstRowData){//当出现换行符，并且isGetFirstRowData为false时表示读取完了第一行的数据
                        var count = everyLineData.Count;
                        for (int i = 0; i < count; i++)
                        {
                            titleData.Add(everyLineData[i], i);
                        }
                        isGetFirstRowData = true;
                    }
                    else{
                        originalData.Add(everyLineData[0], new List<string>(everyLineData));
                    }
                    everyLineData.Clear();
                }
                data.Clear();
                continue;
            }
            if(item != '\r'){//CSV每行末尾都会自动生成一个换行符，这样做可以防止数据末尾有不应该出现的空格
                data.Append(item);
            }
        }
        foreach (var item in originalData.Keys)
        {
            SetData(item);
        }
        textAsset = null;
        titleData = null;
        originalData = null;
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    protected abstract void SetData(string key);

    protected int GetInt(string rowsKey, string columnsKey){
        return int.Parse(GetData(rowsKey, columnsKey));
    }

    protected float GetFloat(string rowsKey, string columnsKey){
        return float.Parse(GetData(rowsKey, columnsKey));
    }

    protected string GetString(string rowsKey, string columnsKey){
        return GetData(rowsKey, columnsKey);
    }

    protected T3 GetEnum<T3>(string rowsKey, string columnsKey){
        return (T3)Enum.Parse(typeof(T3), GetData(rowsKey, columnsKey));
    }

    private string GetData(string rowsKey, string columnsKey){
      
        return originalData[rowsKey][titleData[columnsKey]];
    }
}
