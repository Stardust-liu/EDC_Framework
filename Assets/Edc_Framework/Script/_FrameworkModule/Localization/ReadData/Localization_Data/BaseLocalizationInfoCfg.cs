using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLocalizationInfoCfg<T> : ParsCsv<T>  where T: class, new()
{
    protected bool isAddInfo;

    public void AddLocalizationAsset(TextAsset csv)
    {
        isAddInfo = true;
        ParseData(csv);
    }

    public void RemoveLocalizationAsset(TextAsset csv){
        isAddInfo = false;
        ParseData(csv);
    }

    public abstract void CleanLocalizationData();
}
