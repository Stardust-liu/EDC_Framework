using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Example{
    public class View3_P : View_VP<View3_V, View3_P>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("View3");
        }

        public void OpenWindow1(){
            Hub.Window.OpenWindow(Window1_P.Instance);
        }
    }
}
