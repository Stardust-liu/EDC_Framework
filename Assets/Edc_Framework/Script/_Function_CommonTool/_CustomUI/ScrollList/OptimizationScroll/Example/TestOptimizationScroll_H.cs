using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOptimizationScroll_H : OptimizationScroll_H<TextScrollItemData>
{
    protected override void UpdateScrollData()
    {
        var data = TestOptimizationScrollData.data;
        var count = data.Length;
        scrollData = new TextScrollItemData[count];
        for (int i = 0; i < count; i++)
        {
            scrollData[i] = new TextScrollItemData(data[i]);
        }
    }
}
