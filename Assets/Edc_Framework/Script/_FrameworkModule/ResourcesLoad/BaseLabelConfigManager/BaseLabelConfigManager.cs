using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class BaseLabelConfigManager : BaseIOCComponent
{
    protected virtual List<string> LabelNames => null;
    private LabelManager singleAsset;
    
    protected override void Init()
    {
        base.Init();
        LabelManager.Init(out singleAsset, LabelNames);
    }

    /// <summary>
    /// 加载Label
    /// </summary>
    public async UniTask LoadLabel()
    {
        await singleAsset.LoadLabel();
    }

    /// <summary>
    /// 释放Label
    /// </summary>
    public void ReleaseLabel()
    {
        singleAsset.ReleaseLabelAll();
    }
}
