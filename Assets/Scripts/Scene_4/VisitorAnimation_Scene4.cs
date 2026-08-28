using System.Collections;
using UnityEngine;

public class VisitorAnimation_Scene4 : MonoBehaviour
{
    public TestSwfAnimation animationSwf;

    public bool isReadyJumpUp = true;
    public bool isInCounter;
    public bool isInFridge = false;

    private readonly string[] _sequences = new string[]
    {
        "L_Idle",
        "O_JumpUp",
        "S_Counter",
        "S_Swing",
        "S_Vent",
        "S_Vent2",
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // NÓI RÕ: Chạy sequence "S_Vent2" từ frame 1 tới cuối (endFrame = -1), không loop
            PlayAnimationCustom("S_Vent2", 1, -1, false);
        }
    }

    public void PlayAnimationJumpUp()
    {
        StopCustomAnimation(); // <-- Trả quyền cho Controller khi gọi animation thường

        isReadyJumpUp = false;
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isReadyJumpUp = true;
            animationSwf.sequenceName = _sequences[0];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
        });
    }

    public void PlayAnimationCounter()
    {
        StopCustomAnimation(); // <-- Trả quyền cho Controller khi gọi animation thường

        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isInCounter = true;
        });
    }

    public void PlayAnimationSwing()
    {
        StopCustomAnimation(); // <-- Trả quyền cho Controller khi gọi animation thường

        animationSwf.sequenceName = _sequences[3];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isInFridge = true;
        });
    }

    private Coroutine _customAnimCoroutine;

    /// <summary>
    /// Phát animation tùy chỉnh theo tên sequence, frame bắt đầu, frame kết thúc và chế độ lặp.
    /// </summary>
    public void PlayAnimationCustom(string sequenceName, int startFrame = -1, int endFrame = -1, bool isLoop = false, System.Action onComplete = null)
    {
        if (animationSwf == null) return;

        // Dừng Coroutine tùy chỉnh cũ nếu đang chạy
        StopCustomAnimation();

        // Tắt bộ Controller tự động của FlashTools để Coroutine toàn quyền kiểm soát frame
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
                    // Chạy Once -> Dừng lại ở frame cuối cùng và kết thúc Coroutine
                    _customAnimCoroutine = null;

                    // LƯU Ý: Không bật controller.enabled = true ở đây để tránh FlashTools tự lặp lại
                    onComplete?.Invoke();
                    yield break;
                }
            }
        }
    }

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
}