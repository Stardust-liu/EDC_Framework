using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class View1_C : View_VP<View1_V, View1_C>
    {
        public override void SetPrefabInfo()
        {
            SetPrefabInfo("View1");
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
