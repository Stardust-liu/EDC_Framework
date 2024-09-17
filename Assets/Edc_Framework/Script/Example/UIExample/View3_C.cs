using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Example{
    public class View3_C : PersistentView_VP<View3_V, View3_C>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("View3");
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
