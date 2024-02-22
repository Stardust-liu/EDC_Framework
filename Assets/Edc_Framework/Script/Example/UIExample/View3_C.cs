using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Example{
    public class View3_C : View_VC<View3_V, View3_C>
    {
        public override void SetPrefabInfo()
        {
            SetPrefabInfo("View3", true);
        }
        protected override void PrepareForShwo(){
            base.PrepareForShwo();
            view_V.gameObject.SetActive(true);
        }

        protected override void PrepareForHide(){
            base.PrepareForHide();
            view_V.gameObject.SetActive(false);
        }
    }
}
