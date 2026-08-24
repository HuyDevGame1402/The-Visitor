using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Visitor_Scene2 : MonoBehaviour
{
    private readonly string[] _sequences = new string[]
    {
        "S_Init",                      // Phím 1 (Index 0)
        "N_Arrive",                    // Phím 2 (Index 1)
        "L_WaitingVisitorApple",       // Phím 3 (Index 2)
        "L_WaitingNoVisitorApple",    // Phím 4 (Index 3)
        "S_EatApple",                  // Phím 5 (Index 4)
        "N_JumpAgain",                 // Phím 6 (Index 5)
        "S_KnobSlipDown",              // Phím 7 (Index 6)
        "O_KnobJumpUp",                // Phím 8 (Index 7)
        "S_TurnAround",               // Phím 9 (Index 8)
        "N_SlideIn",                   // Phím 0 (Index 9)
        "S_CatPounceMiss",             // Phím A (Index 10)
        "S_SlideOut",                  // Phím B (Index 11)
        "N_VisitorKnock",              // Phím C (Index 12) *
        "S_CatPounceHit",              // Phím D (Index 13)
        "N_VisitorBranch",             // Phím E (Index 14)
        "O_CatHuh",                    // Phím F (Index 15)
        "S_WindowSplatBarf"            // Phím G (Index 16)
    };

    public TestSwfAnimation animationSwf;

    public bool isReadyEat;
    public bool isNearDoor;
    public bool isToFaceCat;
    public bool isReadyAttackTree;

    public BackgroundAnimation backgroundAnimationScene2;
    public Cat_Scene2 cat;
    public GameObject collisionAttackCat;

    private void Start()
    {
        PlayAnimationStart();
    }

    public void PlayAnimationStart()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.PlayTestAnimation(() =>
        {
            PlayAnimationIdle();
        });
    }

    public void PlayAnimationHideWithApple()
    {
        animationSwf.sequenceName = _sequences[3];
        animationSwf.PlayTestAnimation();
        isReadyEat = false;
    }

    public void PlayAnimationIdle()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationEatApple()
    {
        animationSwf.sequenceName = _sequences[4];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            backgroundAnimationScene2.PlayAnimationSpiderAttack();
            cat.PlayAnimationMoveToGrass();
        });
    }

    public void PlayAnimationJumpToHandle()
    {
        animationSwf.sequenceName = _sequences[5];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[6];
            animationSwf.PlayTestAnimation(() =>
            {
                isNearDoor = true;
                isReadyAttackTree = true;
            });
        });
    }
    public void AttackBranchTree()
    {
        animationSwf.sequenceName = _sequences[14];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[15];
            animationSwf.PlayTestAnimation(() =>
            {
                animationSwf.sequenceName = _sequences[9];
                animationSwf.PlayTestAnimation(() =>
                {
                    animationSwf.sequenceName = _sequences[16];
                    animationSwf.PlayTestAnimation(() =>
                    {
                        cat.PlayAnimationHole();
                    });
                });
            });
        });
    }

    public void PlayAnimationMovetoCavity()
    {
        animationSwf.sequenceName = _sequences[12];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            // gọi animation cat attack

            cat.PlayAnimationAttackVisitor();

            animationSwf.sequenceName = _sequences[13];
            animationSwf.PlayTestAnimation(() =>
            {
                isNearDoor = true;
                isReadyAttackTree = true;
            });
        });
    }

    public void PlayAnimationSlideOut()
    {
        animationSwf.sequenceName = _sequences[11];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            collisionAttackCat.SetActive(true);
        });
    }

    public void PlayAnimationHideVisitor()
    {
        animationSwf.sequenceName = _sequences[16];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }
}
