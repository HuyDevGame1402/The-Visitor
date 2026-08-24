using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Scene2 : MonoBehaviour
{
    public TestSwfAnimation animationSwf;

    public bool isNearDoor = true;

    private readonly string[] _catSequences = new string[]
    {
        "S_Init",            // Phím 1 (Index 0)
        "S_NoticeVisitor",   // Phím 2 (Index 1)
        "O_VisitorSwat",     // Phím 3 (Index 2)
        "N_Distracted",      // Phím 4 (Index 3)
        "L_Distracted",      // Phím 5 (Index 4)
        "N_Sneaky",          // Phím 6 (Index 5)
        "L_Sneaky",          // Phím 7 (Index 6)
        "S_Huh",             // Phím 8 (Index 7)  
        "N_PounceMiss",      // Phím 9 (Index 8)  
        "S_Hole",            // Phím 0 (Index 9) 
        "L_Hole",            // Phím A (Index 10)
        "S_Bum",             // Phím B (Index 11)
        "S_OpenDoor",        // Phím C (Index 12)
        "O_PounceVisitor"    // Phím D (Index 13)
    };

    [SerializeField] private GameObject _scene3;

    public bool isReadyOpenDoor;
    public BoxCollider2D collisionHandler;

    private void Start()
    {
        PlayAnimationStart();
    }

    public void PlayAnimationAttackWom(Visitor_Scene2 visitor, BoxCollider2D BoxCollider2D)
    {
        animationSwf.sequenceName = _catSequences[2];
        visitor.PlayAnimationHideWithApple();
        animationSwf.PlayTestAnimation(() =>
        {
            PlayAnimationStart();
            visitor.PlayAnimationIdle();
            visitor.isReadyEat = true;
            BoxCollider2D.enabled = true;
        });
    }

    public void PlayAnimationStart()
    {
        animationSwf.sequenceName = _catSequences[1];
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationMoveToGrass()
    {
        isNearDoor = false;
        animationSwf.sequenceName = _catSequences[3];
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _catSequences[4];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
        });
    }

    public void PlayAnimationCatAngry()
    {
        animationSwf.sequenceName = _catSequences[5];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _catSequences[6];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
        });
    }

    public void PlayAnimationHub()
    {
        animationSwf.sequenceName = _catSequences[7];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationHole()
    {
        animationSwf.sequenceName = _catSequences[8];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _catSequences[9];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation(() =>
            {
                _scene3.SetActive(true);
            });
        });
    }

    public void PlayAnimationAttackVisitor()
    {
        animationSwf.sequenceName = _catSequences[13];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            PlayAnimationCatAngry();
        });
    }

    public void PlayAnimationBum()
    {
        animationSwf.sequenceName = _catSequences[11];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isReadyOpenDoor = true;
            collisionHandler.enabled = true;
        });
    }
    public void PlayAnimationOpenDoor()
    {
        animationSwf.sequenceName = _catSequences[12];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _catSequences[13];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation(() =>
            {
                // chuyển scene 4
            });
        });
    }
}
