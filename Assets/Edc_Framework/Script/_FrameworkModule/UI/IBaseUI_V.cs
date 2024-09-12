using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseUI_V
{
    void Init(BaseUI_P baseUI_P);
    void Destroy();
}

public interface IBaseUIChild
{
    void Init(BaseUI_P baseUI_P);
}
