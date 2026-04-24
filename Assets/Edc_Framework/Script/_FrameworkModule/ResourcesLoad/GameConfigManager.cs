using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameConfigManager : BaseLabelConfigManager
{
    protected override List<string> LabelNames => new List<string>
    {
        "GameConfig",//游戏配置
        "GlobalSFX",//通用音效
    };
}
