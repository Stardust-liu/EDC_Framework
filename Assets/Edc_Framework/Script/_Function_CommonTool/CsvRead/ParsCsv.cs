using System;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public abstract class ParsCsv<T> : SingleInstance<T> where T: class, new()
{ 
    /// <summary>
    /// CSV文件第一行标签
    /// </summary>
    private string csvName;
    private Dictionary<string, int> titleData;
    private Dictionary<int, List<string>> originalData;
    protected int CurrentReadRow {get; private set;}
    protected ParsCsv(){}
    protected ParsCsv(TextAsset csv){
        ParseData(csv);
    }

    /// <summary>
    /// 解析数据
    /// </summary>
    public virtual void ParseData(TextAsset csv) {
        csvName = csv.name;
        CurrentReadRow = 0;
        titleData = new Dictionary<string, int>();
        originalData = new Dictionary<int, List<string>>();
        ParseData(csv.text);

        foreach (var item in originalData.Keys)
        {
            CurrentReadRow = item;
            SetData();
        }
        titleData = null;
        originalData = null;
        csvName = null;
    }

    private void ParseData(string Text)
    {
        using var reader = new StringReader(Text);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            Delimiter = ","
        };

        using var csv = new CsvReader(reader, config);
        HashSet<int> ignoreColumns = new HashSet<int>();

        while (csv.Read())
        {
            var isIgnoreLine = false;
            for (int i = 0, j = 0; csv.TryGetField<string>(i, out var desc); i++)
            {
                if (i == 0 && desc.StartsWith("#"))
                {
                    isIgnoreLine = true;
                    break;
                }

                if (CurrentReadRow == 0)
                {
                    if (desc.StartsWith("#"))
                    {
                        ignoreColumns.Add(i);
                    }
                    else
                    {
                        titleData.Add(desc, j++);
                    }
                }
                else
                {
                    if (ignoreColumns.Contains(i))
                    {
                        continue;
                    }
                    if (!originalData.ContainsKey(CurrentReadRow))
                    {
                        originalData.Add(CurrentReadRow, new List<string>() { desc });
                    }
                    else
                    {
                        originalData[CurrentReadRow].Add(desc);
                    }
                }
            }
            if (isIgnoreLine) {
                continue;
            }
            CurrentReadRow++;
        }
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

    protected string[] GetStringArray(string columnsKey){
        return CommonTool.StringSplit(columnsKey, ";");
    }

    protected T1[] GetClassArray<T1>(string columnsKey) where T1 : BaseStringParser{
        var dataArray = CommonTool.StringSplit(columnsKey, ";");
        var count = dataArray.Length;
        var array = new T1 [dataArray.Length];
        for (var i = 0; i < count; i++)
        {
            array[i] = CommonTool.StringToClass<T1>(dataArray[i]);
        }
        return array;
    }

    protected T1 GetClass<T1>(string columnsKey) where T1 : BaseStringParser{
        return CommonTool.StringToClass<T1>(columnsKey);
    }

    protected T3 GetEnum<T3>(string columnsKey){
        return (T3)Enum.Parse(typeof(T3), GetData(columnsKey));
    }

    private string GetData(string columnsKey){
        if(!titleData.ContainsKey(columnsKey)){
            LogManager.LogError($"csv文件：{csvName} 中，不存在 {columnsKey} 数据");
        }
        return originalData[CurrentReadRow][titleData[columnsKey]];
    }
}
