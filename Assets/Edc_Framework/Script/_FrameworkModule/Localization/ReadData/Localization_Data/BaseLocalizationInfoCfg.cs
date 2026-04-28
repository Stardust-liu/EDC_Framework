using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLocalizationInfoCfg<T> : ParsCsv<T>  where T: class, new()
{
    private readonly Dictionary<string, List<string>> fileKeyInfo = new ();
    private List<string> idList;
    public void AddLocalizationAsset(TextAsset csv)
    {
        ParseData(csv);
    }

    protected override void InitData()
    {
        idList = new List<string>(RowCount);
        fileKeyInfo[CsvName] = idList; 
    }

    protected override void SetData()
    {
        var id = GetString("ID");
        idList.Add(id);
        SetData(id);
    }

    protected override void SetDataComplete()
    {
        idList = null;
    }

    public void RemoveLocalizationAsset(TextAsset csv){
        var name = csv.name;
        if (fileKeyInfo.TryGetValue(name, out var value))
        {
            foreach (var item in value)
            {
                RemoveLocalizationData(item);
            }
            fileKeyInfo.Remove(name);
        }
    }

    public virtual void CleanLocalizationData()
    {
        fileKeyInfo.Clear();
    }

    protected abstract void SetData(string id);
    protected abstract void RemoveLocalizationData(string key);
}
