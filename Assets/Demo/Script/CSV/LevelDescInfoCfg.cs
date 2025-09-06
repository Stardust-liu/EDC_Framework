using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDescInfoVo
{
    public string levelName;
    public string levelDesc_1;

    public LevelDescInfoVo(string _levelName, string _levelDesc_1)
    {
        levelName = _levelName;
        levelDesc_1 = _levelDesc_1;
    }
}

public class LevelDescInfoCfg : ParsCsv<LevelDescInfoCfg>
{
    private Dictionary<int, LevelDescInfoVo> levelDescInfo;

    protected override void InitData()
    {
        levelDescInfo = new Dictionary<int, LevelDescInfoVo>(RowCount);
    }

    protected override void SetData()
    {
        var Keys = GetInt("ID");
        var value = new LevelDescInfoVo(GetString("LevelName"), GetString("Desc1"));
        levelDescInfo.Add(Keys, value);
    }

    public int GetLevelCount(){
        return RowCount;
    }

    public LevelDescInfoVo GetLevelDescInfo(int index) {
        return levelDescInfo[index];
    }
}
