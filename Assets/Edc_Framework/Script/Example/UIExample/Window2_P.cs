using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Example{
    public class Window2_P : Window_VP<Window2_V, Window2_P>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("Window2");
        }
    }
}