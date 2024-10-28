using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Example{
    public class Window1_P : Window_VP<Window1_V, Window1_P>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("Window1");
        }

        public void OpenWindow2(){
            Hub.Window.OpenWindow(Window2_P.Instance);
        }
    }
}