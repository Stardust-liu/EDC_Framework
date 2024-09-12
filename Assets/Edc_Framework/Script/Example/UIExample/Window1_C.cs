using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Example{
    public class Window1_C : Window_VP<Window1_V, Window1_C>
    {
        public override void CreateUiPrefab()
        {
            CreateUiPrefab("Window1");
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