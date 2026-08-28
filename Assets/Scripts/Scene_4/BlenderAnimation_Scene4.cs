using FTRuntime;
using System.Collections;
using UnityEngine;

public class BlenderAnimation_Scene4 : MonoBehaviour
{
    private readonly string[] _sequences = new string[]
    {
        "L_EmptyLidOn",            // Phím 1 (Index 0)
        "O_EmptyLidOnOrange",          // Phím 2 (Index 1) *
        "S_EmptyLidOffOrange",         // Phím 3 (Index 2)
        "S_OrangeBlendingLidOn",     // Phím 4 (Index 3) bấm máy khi đạy
        "N_OrangeBlendingLidOff",         // Phím 5 (Index 4) mở ra khi xay xong cam có nc 
        "L_OrangeBlendingLidOff",               // Phím 6 (Index 5)  ngược lại 4
        "S_EmptyLidOff",     // Phím 7 (Index 6) *
        "S_OrangeLidOff",     // Phím 8 (Index 7) mở ra khi có cam 
        "O_OrangeBlendingLidOff",      // Phím 9 (Index 8)
    };

    public TestSwfAnimation animationSwf;

    public bool isOpen;
    public bool isHasOrrange;
    public bool isAnimationRunning;
    public bool isHasWaterOrrange;

    public Lady_Scene4 lady;

    public int startFrameRun;

    public SwfClipController swfClipController;

    private void Start()
    {
        // Mặc định chạy animation đầu tiên khi bắt đầu test
        PlayAnimationByIndex(0);
    }

    private void Update()
    {
        // Đọc phím số hàng trên bàn phím (1-9, 0)
        if (Input.GetKeyDown(KeyCode.Alpha1)) PlayAnimationByIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) PlayAnimationByIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) PlayAnimationByIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) PlayAnimationByIndex(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) PlayAnimationByIndex(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) PlayAnimationByIndex(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) PlayAnimationByIndex(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) PlayAnimationByIndex(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) PlayAnimationByIndex(8);

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            PlayAnimationCustom(_sequences[3], startFrameRun, 10, true);
        }
    }

    /// <summary>
    /// Dừng Coroutine chạy custom animation và trả lại quyền điều khiển cho SwfClipController
    /// </summary>
    public void StopCustomAnimation()
    {
        if (_customAnimCoroutine != null)
        {
            StopCoroutine(_customAnimCoroutine);
            _customAnimCoroutine = null;
        }

        if (animationSwf != null)
        {
            FTRuntime.SwfClipController controller = animationSwf.GetComponent<FTRuntime.SwfClipController>();
            if (controller != null)
            {
                controller.enabled = true;
            }
        }
    }

    public void PlayAnimationByIndex(int index)
    {
        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        if (index >= 0 && index < _sequences.Length)
        {
            string seqName = _sequences[index];
            animationSwf.sequenceName = seqName;

            // Cài đặt chế độ lặp: Nếu là Idle hoặc Clean thì để Loop, còn lại chạy Once
            if (seqName.StartsWith("L_"))
            {
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            }
            else
            {
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
            }

            animationSwf.PlayTestAnimation();
            Debug.Log($"Đang phát Animation [{index}]: {seqName}");
        }
    }

    public void PlayAnimationOpen()
    {
        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        animationSwf.sequenceName = _sequences[6];
        Debug.LogWarning("Chạy mở nắp");
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationOpenHasOrrange()
    {
        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        animationSwf.sequenceName = _sequences[7];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationEmptyOrrange()
    {
        if (isAnimationRunning == true) return;

        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        isAnimationRunning = true;
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isAnimationRunning = false;
        });
    }

    public void PlayAnimationAddOrrangeForBlender()
    {
        if (isAnimationRunning == true) return;

        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        isAnimationRunning = true;
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isAnimationRunning = false;
            isHasOrrange = true;
            isOpen = false;
        });
    }

    public void PlayAnimationRunningWithOpen()
    {
        if (isAnimationRunning == true) return;

        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        isAnimationRunning = true;
        animationSwf.sequenceName = _sequences[5];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
        if (swfClipController != null) swfClipController.enabled = true;
        animationSwf.PlayTestAnimation();
        StartCoroutine(CoroutineAnimationRunningWithOpen());
    }

    public void PlayAnimationOpenWithWaterOrrange()
    {
        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        isAnimationRunning = false;
        animationSwf.sequenceName = _sequences[4];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            PlayAnimationRunningWithOpen();
        });
    }

    public void PlayAnimationRunningWithClose()
    {
        if (isAnimationRunning == true) return;

        StopCustomAnimation(); // <-- Đảm bảo dừng Custom Anim cũ

        isAnimationRunning = true;
        animationSwf.sequenceName = _sequences[3];
        isHasWaterOrrange = true;
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            PlayAnimationCustom(_sequences[3], startFrameRun, 10, true);
        });
    }

    private IEnumerator CoroutineAnimationRunningWithOpen()
    {
        yield return new WaitForSeconds(.2f);
        lady.PlayAnimationClear(this);
    }

    // ========================================================================
    // HÀM BỔ SUNG CỦA BLENDERANIMATION_SCENE4
    // ========================================================================

    private Coroutine _customAnimCoroutine;

    /// <summary>
    /// Phát animation tùy chỉnh theo tên sequence, frame bắt đầu, frame kết thúc và chế độ lặp.
    /// </summary>
    public void PlayAnimationCustom(string sequenceName, int startFrame = -1, int endFrame = -1, bool isLoop = false, System.Action onComplete = null)
    {
        if (animationSwf == null) return;

        // Dừng Coroutine tùy chỉnh cũ nếu đang chạy
        StopCustomAnimation();

        // Tắt bộ Controller tự động của FlashTools để tự điều khiển frame qua Coroutine
        FTRuntime.SwfClipController controller = animationSwf.GetComponent<FTRuntime.SwfClipController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Lấy component SwfClip gốc
        FTRuntime.SwfClip clip = animationSwf.GetComponent<FTRuntime.SwfClip>();
        if (clip == null) return;

        // Gán Sequence cần chạy
        clip.sequence = sequenceName;

        // Tính toán Frame Bắt đầu & Kết thúc hợp lệ
        int maxFrame = clip.frameCount - 1;
        if (maxFrame < 0) maxFrame = 0;

        int realStart = (startFrame < 0) ? 0 : Mathf.Clamp(startFrame, 0, maxFrame);
        int realEnd = (endFrame < 0 || endFrame > maxFrame) ? maxFrame : endFrame;

        // Nếu start > end thì tự đảo lại cho đúng
        if (realStart > realEnd)
        {
            int temp = realStart;
            realStart = realEnd;
            realEnd = temp;
        }

        // Bắt đầu Coroutine điều khiển Animation
        _customAnimCoroutine = StartCoroutine(CoroutinePlayCustomAnimation(clip, realStart, realEnd, isLoop, onComplete));
    }

    private IEnumerator CoroutinePlayCustomAnimation(FTRuntime.SwfClip clip, int startFrame, int endFrame, bool isLoop, System.Action onComplete)
    {
        clip.currentFrame = startFrame;

        // Lấy FrameRate của clip (mặc định 30fps nếu không lấy được)
        float fps = clip.frameRate > 0 ? clip.frameRate : 30f;
        float frameDuration = 1f / fps;

        while (true)
        {
            yield return new WaitForSeconds(frameDuration);

            // Tiến tới frame tiếp theo
            if (clip.currentFrame < endFrame)
            {
                clip.currentFrame++;
            }
            else
            {
                // Khi đã chạm mốc endFrame
                if (isLoop)
                {
                    // Lặp lại từ startFrame
                    clip.currentFrame = startFrame;
                }
                else
                {
                    // Chạy Once -> Dừng lại và hoàn tất
                    _customAnimCoroutine = null;

                    // Bật lại controller mặc định khi chạy xong Once
                    FTRuntime.SwfClipController controller = animationSwf.GetComponent<FTRuntime.SwfClipController>();
                    if (controller != null) controller.enabled = true;

                    onComplete?.Invoke();
                    yield break;
                }
            }
        }
    }
}