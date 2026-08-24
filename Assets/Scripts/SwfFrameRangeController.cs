using UnityEngine;
using System;
using System.Collections;
using FTRuntime; // Namespace bắt buộc của FlashTools

public class SwfFrameRangeController : MonoBehaviour
{
    [Header("Cấu hình Animation")]
    [Tooltip("Nhập tên Sequence cần chạy")]
    public string sequenceName = "S_Pickup";

    [Tooltip("TRUE: Chạy [frameCount] frame đầu tiên.\nFALSE: Chạy [frameCount] frame cuối cùng.")]
    public bool playFirstFrames = true;

    [Tooltip("Số lượng frame muốn phát")]
    public int frameCount = 3;

    [Tooltip("Callback gọi khi đoạn frame chạy xong")]
    public Action onComplete = null;

    [Header("Phím Test Inspector")]
    public bool isTestWithSpace = true;

    private SwfClipController _controller;
    private Coroutine _frameRoutine;

    private void Awake()
    {
        _controller = GetComponent<SwfClipController>();
        if (_controller == null)
        {
            Debug.LogError("[SwfFrameRangeController] Không tìm thấy SwfClipController trên GameObject này!");
        }
    }

    //private void Update()
    //{
    //    // Nhấn phím Space để test
    //    if (Input.GetKeyDown(KeyCode.Space) && isTestWithSpace)
    //    {
    //        PlayFrameRange(sequenceName, playFirstFrames, frameCount, onComplete);
    //    }
    //}

    public void PlayAnimationUpdate()
    {
        PlayFrameRange(sequenceName, playFirstFrames, frameCount, onComplete);
    }

    /// <summary>
    /// Phát một số lượng frame chỉ định từ đầu hoặc cuối Sequence
    /// </summary>
    /// <param name="seqName">Tên Sequence</param>
    /// <param name="isFirst">True = N frame đầu | False = N frame cuối</param>
    /// <param name="count">Số lượng frame</param>
    /// <param name="customOnComplete">Callback khi hoàn thành</param>
    public void PlayFrameRange(string seqName, bool isFirst, int count, Action customOnComplete = null)
    {
        if (_controller == null) return;

        if (string.IsNullOrEmpty(seqName))
        {
            Debug.LogWarning("[SwfFrameRangeController] Tên sequence đang bị trống!");
            return;
        }

        // Ngắt coroutine cũ nếu đang phát dở
        if (_frameRoutine != null)
        {
            StopCoroutine(_frameRoutine);
            _frameRoutine = null;
        }

        _frameRoutine = StartCoroutine(PlayFrameRangeRoutine(seqName, isFirst, count, customOnComplete ?? onComplete));
    }

    private IEnumerator PlayFrameRangeRoutine(string seqName, bool isFirst, int count, Action callback)
    {
        _controller.loopMode = SwfClipController.LoopModes.Once;

        // 1. Chuyển sequence và bắt đầu phát
        _controller.GotoAndPlay(seqName, 0);

        // Chờ 1 frame để SwfClipController và SwfClip cập nhật Sequence
        yield return null;

        if (_controller == null || _controller.clip == null) yield break;

        // Lấy SwfClip trực tiếp từ controller
        SwfClip runtimeClip = _controller.clip;

        int totalFrames = runtimeClip.frameCount;
        float frameRate = runtimeClip.frameRate > 0 ? runtimeClip.frameRate : 30f;

        if (totalFrames <= 0)
        {
            Debug.LogWarning($"[SwfFrameRangeController] Sequence '{seqName}' không có frame nào hoặc không tồn tại!");
            yield break;
        }

        int clampedCount = Mathf.Clamp(count, 1, totalFrames);

        // Tính thời gian cần phát cho N frame (giây)
        float targetDuration = clampedCount / frameRate;

        int startFrame;
        if (isFirst)
        {
            // Frame đầu: phát từ frame 0
            startFrame = 0;
        }
        else
        {
            // Frame cuối (Ví dụ: Total 27, Count 3 => Bắt đầu từ Frame 24)
            startFrame = Mathf.Max(0, totalFrames - clampedCount);
        }

        // 2. Nhảy đến frame bắt đầu
        _controller.GotoAndPlay(startFrame);
        Debug.Log($"[SwfFrameRangeController] Sequence: {seqName} | Tổng: {totalFrames} frames | Phát từ Frame {startFrame} trong {targetDuration:F3}s ({clampedCount} frames)");

        // 3. Theo dõi thời gian thực tế đã phát
        float elapsedTime = 0f;
        while (_controller != null && _controller.isPlaying && elapsedTime < targetDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Dừng animation sau khi chạy đủ thời gian
        if (_controller != null)
        {
            _controller.Stop(false);
        }

        Debug.Log($"[SwfFrameRangeController] Đã hoàn thành {clampedCount} frames của sequence {seqName}!");
        callback?.Invoke();
        _frameRoutine = null;
    }
}