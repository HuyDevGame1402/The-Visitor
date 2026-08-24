using UnityEngine;
using System;
using System.Collections;
using FTRuntime; // Namespace bắt buộc của FlashTools

public class TestSwfAnimation : MonoBehaviour
{
    [Header("Cấu hình Animation")]
    [Tooltip("Nhập tên Sequence cần test (ví dụ: S_Pickup, L_Idle, ...)")]
    public string sequenceName = "S_Pickup";

    [Tooltip("Chế độ lặp animation")]
    public SwfClipController.LoopModes loopMode = SwfClipController.LoopModes.Once;

    [Tooltip("Khoảng thời gian (giây) kích hoạt callback trước khi animation kết thúc. 0 = chạy hết.")]
    public float triggerOffsetTime = 0f;

    /// <summary>
    /// Callback gọi khi animation chạy xong. Mặc định là null.
    /// </summary>
    public Action onComplete = null;

    private SwfClipController _controller;
    private Coroutine _trackAnimationCoroutine;

    public bool isTestAnimation = true;

    private void Awake()
    {
        _controller = GetComponent<SwfClipController>();
        if (_controller == null)
        {
            Debug.LogError("[TestSwfAnimation] Không tìm thấy SwfClipController trên GameObject này!");
        }
    }

    private void Update()
    {
        // Bấm phím Space để test animation
        if (Input.GetKeyDown(KeyCode.Space) && isTestAnimation)
        {
            PlayTestAnimation();
        }
    }

    /// <summary>
    /// Phát animation test
    /// </summary>
    /// <param name="customOnComplete">Action gọi khi hoàn tất (nếu truyền sẽ ghi đè onComplete)</param>
    /// <param name="offsetTime">Thời gian kích hoạt trước khi kết thúc (giây). Nếu null sẽ lấy triggerOffsetTime từ Inspector</param>
    public void PlayTestAnimation(Action customOnComplete = null, float? offsetTime = null)
    {
        if (_controller == null) return;

        if (string.IsNullOrEmpty(sequenceName))
        {
            Debug.LogWarning("[TestSwfAnimation] Tên sequence đang bị trống!");
            return;
        }

        // Ngắt coroutine cũ nếu đang chạy
        if (_trackAnimationCoroutine != null)
        {
            StopCoroutine(_trackAnimationCoroutine);
            _trackAnimationCoroutine = null;
        }

        _controller.loopMode = loopMode;
        _controller.GotoAndPlay(sequenceName, 0);

        Debug.Log($"[TestSwfAnimation] Đang phát Sequence: {sequenceName} | LoopMode: {loopMode}");

        Action callbackToRun = customOnComplete ?? onComplete;
        float actualOffset = offsetTime ?? triggerOffsetTime;

        // Chỉ theo dõi nếu có callback
        if (callbackToRun != null)
        {
            _trackAnimationCoroutine = StartCoroutine(TrackAnimationCompleteRoutine(callbackToRun, actualOffset));
        }
    }

    private IEnumerator TrackAnimationCompleteRoutine(Action callback, float offset)
    {
        // Chờ 1 frame để controller cập nhật clip và thông số khung hình
        yield return null;

        if (_controller == null || _controller.clip == null) yield break;

        // Tính tổng thời lượng của Sequence dựa trên frameRate và tổng số frame
        float frameRate = _controller.clip.frameRate > 0 ? _controller.clip.frameRate : 30f;
        float totalDuration = _controller.clip.frameCount / frameRate;

        // Xác định thời điểm cần stop và kích hoạt callback
        float targetTime = Mathf.Max(0f, totalDuration - offset);

        // Trường hợp offset >= thời lượng animation: dừng ngay lập tức
        if (targetTime <= 0f)
        {
            _controller.Stop(false);
            Debug.Log($"[TestSwfAnimation] Sequence {sequenceName} bị dừng ngay lập tức do offsetTime >= thời lượng animation!");
            callback?.Invoke();
            _trackAnimationCoroutine = null;
            yield break;
        }

        float elapsedTime = 0f;

        // Chờ đến thời điểm offset chỉ định hoặc cho đến khi animation ngắt giữa chừng (isPlaying == false)
        while (_controller != null && _controller.isPlaying && elapsedTime < targetTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Nếu dừng do chạm mốc offset time (trước khi animation tự hết)
        if (offset > 0f && _controller != null && _controller.isPlaying)
        {
            _controller.Stop(false);
        }

        Debug.Log($"[TestSwfAnimation] Sequence {sequenceName} đã hoàn thành (hoặc chạm mốc Offset: {offset}s)!");
        callback?.Invoke();
        _trackAnimationCoroutine = null;
    }
}