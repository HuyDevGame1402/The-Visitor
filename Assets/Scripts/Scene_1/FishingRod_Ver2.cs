using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class FishingRod_Ver2 : MonoBehaviour
{

    public string animationPickUp;
    public string animationIdle;
    public string animationS_Bite;
    public string animationN_JumpVisitor;
    public string animationS_VisitorRod;
    public string animationS_FlingVisitor;
    public string animationO_Bend;

    public TestSwfAnimation animationSwf;

    public bool isReady;
    public bool isFishReturn;

    private string animationFinalVisitorRod;

    private void Start()
    {
        animationSwf.sequenceName = animationPickUp;

        animationSwf.PlayTestAnimation(() => 
        {
            animationSwf.sequenceName = animationIdle;
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
            isReady = true;
        });
    }

    public void PlayAnimationBite()
    {
        animationSwf.sequenceName = animationS_Bite;
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isFishReturn = true;
        });
    }

    public void PlayAnimationJumpVisitor()
    {
        animationSwf.sequenceName = animationN_JumpVisitor;
        animationSwf.PlayTestAnimation(() =>
        {
            animationFinalVisitorRod = animationS_VisitorRod + 1;
            animationSwf.sequenceName = animationFinalVisitorRod;
            animationSwf.PlayTestAnimation();
        });
    }

    public void PlayAnimationVisitorRod(int index)
    {
        if (isFishReturn == false) return;

        if(index >= 5)
        {
            // bật quái vào nhà
            animationSwf.sequenceName = animationS_FlingVisitor;
            animationSwf.PlayTestAnimation(() =>
            {
                animationSwf.sequenceName = animationO_Bend;
                animationSwf.PlayTestAnimation(null, 0.2f);
            });
            return;
        }

        animationFinalVisitorRod = animationS_VisitorRod + index;
        animationSwf.sequenceName = animationFinalVisitorRod;
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }
}
