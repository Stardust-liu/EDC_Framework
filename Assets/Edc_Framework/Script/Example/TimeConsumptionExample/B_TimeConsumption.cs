using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class B_TimeConsumption : TimeConsumptionDetection
{
    private void Start() {
        ProxyStart();
    }

    protected override void HandleStart()
    {
        Thread.Sleep(3000);
    }
}
