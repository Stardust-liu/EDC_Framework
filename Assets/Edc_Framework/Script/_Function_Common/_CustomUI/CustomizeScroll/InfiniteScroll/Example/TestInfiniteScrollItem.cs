using System.Collections;
using System.Collections.Generic;
using TMPro;


public class TestInfiniteScrollItem : BaseInfiniteScrollItem<TextScrollItemData>
{
    public TextMeshProUGUI text;
    public override void UpdateData(int index, TextScrollItemData data)
    {
        base.UpdateData(index, data);
        text.text = data.serialNumber;
    }
}
