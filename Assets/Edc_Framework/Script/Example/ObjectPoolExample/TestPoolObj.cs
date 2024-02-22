using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Example{
    public class TestPoolObj : BasePoolManager<TestPoolObj>
    {
        public TextMeshProUGUI number;
        public override void Init()
        {
            gameObject.SetActive(true);
            number.text = ActiveObjectCount.ToString();
        }

        public override void Recycle()
        {
            gameObject.SetActive(false);
        }
    }
}
