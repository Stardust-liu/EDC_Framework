using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example{
    public class View2_C : View_VP<View2_V, View2_C>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("View2");
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