using UnityEngine;
using FTRuntime; // Namespace bắt buộc của FlashTools
using System;    // Cần thiết để sử dụng Action
using System.Collections;

public class MeteorAnimation : MonoBehaviour
{
    public enum AnimationType
    {
        Type1_Transition, // Dạng 1: 4 Sequence (S_Wait, N_Animate, N_BeginTransition, S_Clear)
        Type2_FullDrop    // Dạng 2: 6 Sequence (S_Wait, N_Init, N_Plunge, O_Resurface, L_IdleFloating, L_FloatingVisitor)
    }

    [Header("Cấu hình Dạng Animation")]
    public AnimationType animType = AnimationType.Type1_Transition;

    private SwfClipController _controller;
    private Coroutine _waitCoroutine;

    #region Dạng 1: Sequences
    public const string T1_WAIT = "S_Wait";
    public const string T1_ANIMATE = "N_Animate";
    public const string T1_BEGIN_TRANSITION = "N_BeginTransition";
    public const string T1_CLEAR = "S_Clear";
    #endregion

    #region Dạng 2: Sequences
    public const string T2_WAIT = "S_Wait";
    public const string T2_INIT = "N_Init";
    public const string T2_PLUNGE = "N_Plunge";
    public const string T2_RESURFACE = "O_Resurface";
    public const string T2_IDLE_FLOATING = "L_IdleFloating";
    public const string T2_FLOATING_VISITOR = "L_FloatingVisitor";
    #endregion

    public StartGameLogic startGameLogic;

    private void Awake()
    {
        _controller = GetComponent<SwfClipController>();
        if (_controller == null)
        {
            Debug.LogError("[MeteorAnimation] Thiếu SwfClipController trên GameObject này!");
        }
    }

    private void Start()
    {
        // Chạy thử Action 2 và đăng ký callback khi hoàn thành
        PlayAction2(() =>
        {
            Debug.Log("[MeteorAnimation] Action 2 đã hoàn thành xong!");
            startGameLogic.MoveCameraToTarget();
            gameObject.SetActive(false);
            // Đặt logic tiếp theo của bạn ở đây (ví dụ: tự chuyển qua Action 3)
        });
    }

    private void Update()
    {
        // Điều khiển phím bấm 1 -> 6 (Không dùng Keypad)
        if (Input.GetKeyDown(KeyCode.Alpha1)) PlayWait();
        if (Input.GetKeyDown(KeyCode.Alpha2)) PlayAction2();
        if (Input.GetKeyDown(KeyCode.Alpha3)) PlayAction3();
        if (Input.GetKeyDown(KeyCode.Alpha4)) PlayAction4();
        if (Input.GetKeyDown(KeyCode.Alpha5)) PlayAction5();
        if (Input.GetKeyDown(KeyCode.Alpha6)) PlayAction6();
    }

    #region Logic Điều Khiển Theo Dạng (Type)

    public void PlayWait(Action onComplete = null)
    {
        string seq = (animType == AnimationType.Type1_Transition) ? T1_WAIT : T2_WAIT;
        PlaySequence(seq, SwfClipController.LoopModes.Once, onComplete);
    }

    public void PlayAction2(Action onComplete = null)
    {
        string targetSeq = (animType == AnimationType.Type1_Transition) ? T1_ANIMATE : T2_INIT;
        PlaySequence(targetSeq, SwfClipController.LoopModes.Once, onComplete);
    }

    public void PlayAction3(Action onComplete = null)
    {
        string targetSeq = (animType == AnimationType.Type1_Transition) ? T1_BEGIN_TRANSITION : T2_PLUNGE;
        PlaySequence(targetSeq, SwfClipController.LoopModes.Once, onComplete);
    }

    public void PlayAction4(Action onComplete = null)
    {
        string targetSeq = (animType == AnimationType.Type1_Transition) ? T1_CLEAR : T2_RESURFACE;
        PlaySequence(targetSeq, SwfClipController.LoopModes.Once, onComplete);
    }

    public void PlayAction5(Action onComplete = null)
    {
        if (animType == AnimationType.Type2_FullDrop)
            PlaySequence(T2_IDLE_FLOATING, SwfClipController.LoopModes.Loop, onComplete);
        else
            Debug.LogWarning("[MeteorAnimation] Dạng 1 chỉ có 4 animation (Dùng phím 1->4)!");
    }

    public void PlayAction6(Action onComplete = null)
    {
        if (animType == AnimationType.Type2_FullDrop)
            PlaySequence(T2_FLOATING_VISITOR, SwfClipController.LoopModes.Once, onComplete);
        else
            Debug.LogWarning("[MeteorAnimation] Dạng 1 chỉ có 4 animation (Dùng phím 1->4)!");
    }

    /// <summary>
    /// Phát một sequence với chế độ lặp và hỗ trợ callback khi chạy xong.
    /// </summary>
    public void PlaySequence(string sequenceName, SwfClipController.LoopModes loopMode, Action onComplete = null)
    {
        if (_controller == null) return;

        // Dừng Coroutine chờ cũ nếu có để tránh trùng lặp callback
        if (_waitCoroutine != null)
        {
            StopCoroutine(_waitCoroutine);
            _waitCoroutine = null;
        }

        _controller.loopMode = loopMode;
        _controller.GotoAndPlay(sequenceName, 0);

        Debug.Log($"[{animType}] Chạy Sequence: {sequenceName} | LoopMode: {loopMode}");

        // Kích hoạt Coroutine chờ nếu có truyền callback và không phải chế độ Loop
        if (onComplete != null && loopMode != SwfClipController.LoopModes.Loop)
        {
            _waitCoroutine = StartCoroutine(WaitSequenceComplete(onComplete));
        }
    }

    private IEnumerator WaitSequenceComplete(Action onComplete)
    {
        // Chờ 1 frame để controller chuyển trạng thái sang playing
        yield return null;

        // Vòng lặp chờ cho đến khi controller dừng phát animation (isPlaying = false)
        while (_controller != null && _controller.isPlaying)
        {
            yield return null;
        }

        _waitCoroutine = null;
        onComplete?.Invoke();
    }

    #endregion
}