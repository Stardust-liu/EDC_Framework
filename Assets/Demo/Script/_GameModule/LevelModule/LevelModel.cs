using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelModel : BaseIOCComponent
{
    protected override void Init()
    {
        base.Init();
        InitInfoCfg();
    }

    private void InitInfoCfg(){
        LevelDescInfoCfg.Instance.ParseData(Hub.Resources.Get<TextAsset>("LevelDesc"));
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
