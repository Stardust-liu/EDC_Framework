using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FreeMissionView_2D : BaseReturnableView<FreeMissionView_2D_M>
{
    public GameObject levelSelectBtnPrefab;
    public TextMeshProUGUI levelDesc2;
    public RectTransform levelContentParent;

    protected override void Init()
    {
        base.Init();
        CreateLevelSelectBtn();
    }

    private void CreateLevelSelectBtn()
    {
        var count = model.GetLevelCount();
        for (var i = 0; i < count; i++)
        {
            var item = GameObject.Instantiate(levelSelectBtnPrefab, levelContentParent).GetComponent<LevelSelectBtn>();
            var info = new LevelSelectBtnInfo(i, model.GetLevelDescInfo(i).levelName);
            item.SetInitAction(info, ChangeSelectLevel, ExitOptionItem);
            if (i == 0)
            {
                selectEffect.transform.SetParent(item.selectEffectParent, false);
                selectEffect.SetPosZero();
            }
        }
    }

    private void ChangeSelectLevel(Transform parent, int levelID)
    {
        levelDesc2.text = model.GetLevelDescInfo(levelID).levelDesc_1;
        ChangeOptionItem(parent, null);
    }

    protected override void ExitOptionItem()
    {
        levelDesc2.text = null;
        base.ExitOptionItem();
    }
}
