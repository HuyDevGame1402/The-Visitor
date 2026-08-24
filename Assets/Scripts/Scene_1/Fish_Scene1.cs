using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Scene1 : MonoBehaviour
{
    public string animationMove;

    public TestSwfAnimation animationSwf;

    public LogicMeteoClick logicMeteoClick;
    public FishingRod_Ver2 fishingRod_Ver2;

    public void PlayAnimationMove()
    {
        animationSwf.sequenceName = animationMove;
        animationSwf.PlayTestAnimation(() =>
        {
            if (fishingRod_Ver2.isReady)
            {
                logicMeteoClick.isLock = true;
                fishingRod_Ver2.PlayAnimationBite();
            }
        });
    }
}
