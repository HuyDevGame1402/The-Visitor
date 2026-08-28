using FTRuntime;
using UnityEngine;

public class KnifeAnimation_Scene4 : MonoBehaviour
{
    public TestSwfAnimation animationSwf;
    public SwfClip swfSclip;

    public bool isReady = true;

    public SwfClip swfClip; 

    private readonly string[] _sequences = new string[]
    {
        "L_IdleDown",                     // Phím 1 (Index 0)
        "O_Lift",                         // Phím 2 (Index 1)
        "S_Setup",                        // Phím 3 (Index 2)
        "S_Blank",                        // Phím 4 (Index 3)
    };

    private void Update()
    {
        // Kiểm tra phím 1 -> L_IdleDown (Loop/Once tùy bạn chọn)
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlayAnimationSequence(0, SwfClipController.LoopModes.Loop);
        }

        // Kiểm tra phím 2 -> O_Lift (Gọi lại hàm có sẵn của bạn để đảm bảo cờ isReady)
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlayAnimationOLift();
        }

        // Kiểm tra phím 3 -> S_Setup
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlayAnimationSequence(2, SwfClipController.LoopModes.Once);
        }

        // Kiểm tra phím 4 -> S_Blank (Gọi lại hàm Hide có sẵn của bạn)
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            PlayAnimationHide();
        }
    }

    private void PlayAnimationSequence(int index, SwfClipController.LoopModes loopMode)
    {
        if (index < 0 || index >= _sequences.Length) return;

        animationSwf.sequenceName = _sequences[index];
        animationSwf.loopMode = loopMode;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationHide()
    {
        animationSwf.sequenceName = _sequences[3];
        animationSwf.loopMode = SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationOLift()
    {
        isReady = false;
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isReady = true;
        });
    }

    public void PlayAnimationMoveToDraw()
    {
        swfClip.materialVisibilities[0] = true;
        swfClip.sortingOrder = 60;
        isReady = false;
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationIdle()
    {
        swfClip.materialVisibilities[0] = false;
        swfClip.sortingOrder = 10;
        isReady = true;
        animationSwf.sequenceName = _sequences[0];
        animationSwf.loopMode = SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }
}