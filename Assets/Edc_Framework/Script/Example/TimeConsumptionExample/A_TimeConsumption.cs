using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class A_TimeConsumption : TimeConsumptionDetection
{
    private void OnEnable() {
        ProxyOnEnable();
    }

    protected override void HandleOnEnable()
    {
        Thread.Sleep(200);
    }
}
