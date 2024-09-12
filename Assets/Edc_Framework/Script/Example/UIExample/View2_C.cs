using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class View2_C : View_VP<View2_V, View2_C>
    {
        public override void SetPrefabInfo()
        {
            SetPrefabInfo("View2");
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