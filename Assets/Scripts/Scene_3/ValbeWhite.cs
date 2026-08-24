using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValbeWhite : Valve
{
    protected override void OnMouseDown()
    {
        if (visitor.isReady == false) return;
        if (visitor.indexValve == 1)
        {
            animationScene3.PlayAnimationJumpValve_1_1();
        }
        else if (visitor.indexValve == 2)
        {
            animationScene3.PlayAnimationJumpValve_2_2();
        }
        else if (visitor.indexValve == 3)
        {
            animationScene3.PlayAnimationJumpValve_3_3();
        }
        else if (visitor.indexValve == 4)
        {
            animationScene3.PlayAnimationJumpValve_4_4();
        }
    }
}
