using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TabGroup<T> : MonoBehaviour
where T: TabOption<T>
{
    [LabelText("默认选择索引")]
    public int defaultSelectIndex = 0;
    public T[] TabOptionArray;

    protected virtual void Start(){
        if(defaultSelectIndex != -1){
            TabOptionArray[defaultSelectIndex].OnSelect();
        }
    }

    /// <summary>
    /// 选择标签
    /// </summary>
    public void SelectTab(int index){
        if(TabOptionArray[0].GetCurrentSelect() == null){
            var count = TabOptionArray.Length;
            for (var i = 0; i < count; i++)
            {
                if(i == index){
                    TabOptionArray[index].OnSelect();
                }
                else{
                    TabOptionArray[index].OnCancelSelect();
                }
            }
        }
        else{
            TabOptionArray[index].OnSelect();
        }
    }
}
