using FTRuntime;
using System.Collections;
using UnityEngine;

public class BedsAnimation_Scene5 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

    public int startFrame;
    public float speedAnimation;
    public SwfClipController swfController;
    public GameObject collisionVisitor;
    public GameObject collisionPeople;

    private readonly string[] _sequences = new string[]
    {
        "L_IdleSleeping", // Phím 1
        "S_Mouth",        // Phím 2
        "S_JumpDown",     // Phím 3
        "S_Claw"          // Phím 4
    };

    private void Start()
    {
        // Tắt chế độ test bằng phím Space của TestSwfAnimation để tránh xung đột
        if (animationSwf != null)
        {
            animationSwf.isTestAnimation = false;
        }
        else
        {
            Debug.LogError("[BedsAnimation_Scene5] Chưa gán TestSwfAnimation vào Inspector!");
        }
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm các phím từ 1 đến 4 (hỗ trợ cả bàn phím chính và phím số Keypad)
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlaySequenceAtIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlaySequenceAtIndex(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlaySequenceAtIndex(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            PlaySequenceAtIndex(3);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            PlayAnimationLoopIdle();
        }
    }

    /// <summary>
    /// Phát animation theo chỉ số trong mảng _sequences
    /// </summary>
    private void PlaySequenceAtIndex(int index)
    {
        if (index < 0 || index >= _sequences.Length) return;

        StopCustomAnimation();
        swfController.enabled = true;
        string targetSequence = _sequences[index];

        // Gán tên sequence vào controller
        animationSwf.sequenceName = targetSequence;
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;

        // Gọi hàm phát animation
        animationSwf.PlayTestAnimation(() =>
        {
            Debug.Log($"[BedsAnimation_Scene5] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationLoopIdle()
    {
        swfController.enabled = true;
        PlayAnimationCustom(_sequences[1], startFrame, -1, speedAnimation, true);
    }

    public void PlayAnimationMouth()
    {
        swfController.rateScale = 1f;
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            PlayAnimationLoopIdle();
            collisionVisitor.SetActive(true);
        });
    }

    public void PlayAnimationJumpDown()
    {
        swfController.rateScale = 1f;
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            collisionPeople.SetActive(true);
        });
    }

    public void PlayAnimationClaw()
    {
        swfController.rateScale = 1f;
        animationSwf.sequenceName = _sequences[3];
        animationSwf.loopMode = SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            // chuyển sang scene 6
        });
    }

    private Coroutine _customAnimCoroutine;

    /// <summary>
    /// Phát animation tùy chỉnh theo tên sequence, frame bắt đầu, frame kết thúc và chế độ lặp.
    /// </summary>
    /// <summary>
    /// Phát animation tùy chỉnh theo tên sequence, frame bắt đầu, frame kết thúc, tốc độ và chế độ lặp.
    /// </summary>
    public void PlayAnimationCustom(string sequenceName, int startFrame = -1, int endFrame = -1, float speedMultiplier = 1f, bool isLoop = false, System.Action onComplete = null)
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

        // Bắt đầu Coroutine điều khiển Animation có truyền speedMultiplier
        _customAnimCoroutine = StartCoroutine(CoroutinePlayCustomAnimation(clip, realStart, realEnd, speedMultiplier, isLoop, onComplete));
    }

    private IEnumerator CoroutinePlayCustomAnimation(FTRuntime.SwfClip clip, int startFrame, int endFrame, float speedMultiplier, bool isLoop, System.Action onComplete)
    {
        clip.currentFrame = startFrame;

        // Lấy FrameRate gốc của clip
        float baseFps = clip.frameRate > 0 ? clip.frameRate : 30f;

        // Tính toán FPS thực tế dựa trên hằng số tốc độ (đảm bảo không bị chia cho 0)
        float effectiveFps = baseFps * Mathf.Max(0.001f, speedMultiplier);
        float frameDuration = 1f / effectiveFps;

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