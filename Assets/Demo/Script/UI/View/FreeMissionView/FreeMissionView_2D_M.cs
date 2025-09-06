using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMissionView_2D_M : BaseUI_Model
{
    protected override void Init()
    {
        RegisterData<LevelModel>(GameModule.Level);
    }

    public int GetLevelCount()
    {
        return Get<LevelModel>().GetLevelCount();
    }

    public LevelDescInfoVo GetLevelDescInfo(int index) {
        return Get<LevelModel>().GetLevelDescInfo(index);
    }
}
