using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModel : BaseIOCComponent
{

    protected override void Init()
    {
        base.Init();
        InitInfoCfg();
    }

    private void InitInfoCfg(){
        var assetPath = "Assets/Demo/Sources/CSV/Level/LevelDesc.csv";
        var resourcePath = new ResourcePath("LevelDesc", assetPath);
        LevelDescInfoCfg.Instance.ParseData(Hub.Resources.GetCsvFile(resourcePath));
    }

    /// <summary>
    /// 获取关卡数量
    /// </summary>
    public int GetLevelCount()
    {
        return LevelDescInfoCfg.Instance.GetLevelCount();
    }

    /// <summary>
    /// 获取管卡信息
    /// </summary>
    public LevelDescInfoVo GetLevelDescInfo(int index)
    {
        return LevelDescInfoCfg.Instance.GetLevelDescInfo(index);
    }
}
