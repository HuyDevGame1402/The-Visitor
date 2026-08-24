using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicClickBirdFlag : MonoBehaviour
{
    public string animationNameIdle;
    public string animationNameO_StumpShake;
    public string animationNameL_StumpEmpty;
    public string animationNameN_StumpBreak;

    public bool isReadyFly;
    public bool isHasBird = true;

    public CapsuleCollider2D capsuleCollider2;
    public TestSwfAnimation animationSwf;

    public GameObject bird;
    public FishingRod fishRod;

    private void OnMouseDown()
    {
        if (isHasBird)
        {
            capsuleCollider2.enabled = false;
            animationSwf.sequenceName = animationNameO_StumpShake;
            isReadyFly = true;
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
            animationSwf.PlayTestAnimation(() =>
            {
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
                animationSwf.sequenceName = animationNameIdle;
                animationSwf.PlayTestAnimation();
                capsuleCollider2.enabled = true;
                isReadyFly = false;
            });
        }
        else
        {
            isReadyFly = false;
            capsuleCollider2.enabled = false;
            // animation khi thân cây k có bird
            fishRod.isReady = true;
            animationSwf.sequenceName = animationNameN_StumpBreak;
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
            animationSwf.PlayTestAnimation();
        }
    }

    public void PlayAnimationStumpEmpty()
    {
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.sequenceName = animationNameL_StumpEmpty;
        animationSwf.PlayTestAnimation();
        capsuleCollider2.enabled = true;
        isReadyFly = false;
        isHasBird = false;
        bird.gameObject.SetActive(true);
    }
}
