using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class View2_P : View_VP<View2_V, View2_P>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("View2");
        }

        public void ClickChangeView3(){
            Hub.View.ChangeView(View3_P.Instance);
        }
    }
}