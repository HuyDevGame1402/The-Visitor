using UnityEngine;

public class Background_Scene4 : MonoBehaviour
{
    public TestSwfAnimation animationSwf;
    public bool isWaterInGround;

    private readonly string[] _sequences = new string[]
    {
        "L_Idle",            
        "S_Spill",         
        "O_Clean",        
    };

    public void PlayAnimationWater()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
        isWaterInGround = true;
    }

    public void PlayAnimationIdle()
    {
        animationSwf.sequenceName = _sequences[0];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationClear()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            PlayAnimationIdle();
        });
    }
}
