using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInfiniteScroll_V : InfiniteScroll_V<TextScrollItemData>
{
    protected override void UpdateScrollData()
    {
        var data = TestInfiniteScrollData.data;
        var count = data.Length;
        scrollData = new TextScrollItemData[count];
        for (int i = 0; i < count; i++)
        {
            scrollData[i] = new TextScrollItemData(data[i]);
        }
    }
}
