using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Example{
    public class Window2_C : Window_VC<Window2_V, Window2_C>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("Window2");
        }

        protected override void PrepareForShwo(){
            base.PrepareForShwo();
            window_V.transform.localScale = Vector2.zero;
            window_V.transform.DOScale(Vector2.one, 0.3f).OnComplete(ShwoFinish);
        }

        protected override void PrepareForHide(){
            base.PrepareForHide();
            window_V.transform.DOScale(Vector2.zero, 0.3f).OnComplete(HideFinish);
        }
    }
}