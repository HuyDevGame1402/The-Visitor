using UnityEngine;

public class DogAnimationAndCollision : MonoBehaviour
{
    public TestSwfAnimation animationSwf;

    public bool isSleep = true;

    private readonly string[] _sequences = new string[]
    {
        "L_Idle",            // Phím 1 (Index 0)
        "O_Growl",   // Phím 2 (Index 1)
    };

    private void OnMouseDown()
    {
        if (isSleep)
        {
            isSleep = false;
            animationSwf.sequenceName = _sequences[1];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
            animationSwf.PlayTestAnimation(() =>
            {
                isSleep = true;
                animationSwf.sequenceName = _sequences[0];
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
                animationSwf.PlayTestAnimation();
            });
        }
    }
}
