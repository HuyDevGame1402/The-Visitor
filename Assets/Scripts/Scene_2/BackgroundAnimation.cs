using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimation : MonoBehaviour
{
    public TestSwfAnimation animationSwf;
    private readonly string[] _backgroundSequences = new string[]
    {
        "L_Idle",
        "O_Spider",
        "N_SpiderAttack",
        "L_Flies",      
    };

    public void PlayAnimationSpiderAttack()
    {
        animationSwf.sequenceName = _backgroundSequences[2];
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _backgroundSequences[3];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
        });
    }
}
