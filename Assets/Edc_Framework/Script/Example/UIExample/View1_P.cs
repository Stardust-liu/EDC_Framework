using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class View1_P : View_VP<View1_V, View1_P>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("View1");
        }

        public void ClickChangeView2(){
            Hub.View.ChangeView(View2_P.Instance);
        }
    }
}
