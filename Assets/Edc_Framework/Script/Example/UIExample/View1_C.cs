using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class View1_C : View_VP<View1_V, View1_C>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("View1");
        }

        protected override void StartShow(){
            base.StartShow();
            view_V.gameObject.SetActive(true);
        }

        protected override void StartHide(){
            base.StartHide();
            view_V.gameObject.SetActive(false);
        }
    }
}
