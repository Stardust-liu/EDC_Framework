using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Example{
    public class Window2_C : Window_VP<Window2_V, Window2_C>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("Window2");
        }

        protected override void StartShow(){
            base.StartShow();
            window_V.transform.localScale = Vector2.zero;
            window_V.transform.DOScale(Vector2.one, 0.3f).OnComplete(ShowFinish);
        }

        protected override void StartHide(){
            base.StartHide();
            window_V.transform.DOScale(Vector2.zero, 0.3f).OnComplete(HideFinish);
        }
    }
}