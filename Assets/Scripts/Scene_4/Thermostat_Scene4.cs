using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermostat_Scene4 : MonoBehaviour
{

    public TestSwfAnimation animationSwf;
    public Lady_Scene4 lady;
    public bool isReady = true;

    private void OnMouseDown()
    {
        if (lady.isClear == true) return;
        if (isReady)
        {
            isReady = false;
            lady.PlayAnimationImpactWithThermostat(this);
        }
    }
}
