using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimationScene2 : MonoBehaviour
{
    private readonly string[] _sequences = new string[]
    {
        "L_Idle",
        "O_PullBranch",                    
    };

    public TestSwfAnimation animationSwf;

    public void PlayAnimationPull()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;

        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[0];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
        });
    }
}
