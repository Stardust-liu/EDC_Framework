using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TestOptimizationScrollItem : BaseOptimizationScrollItem<TextScrollItemData>
{
    public TextMeshProUGUI text;
    public override void UpdateData(int index, TextScrollItemData testData)
    {
        base.UpdateData(index, testData);
        text.text = testData.serialNumber;
    }
}
