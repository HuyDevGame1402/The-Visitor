using UnityEngine;
using FTRuntime; // Namespace bắt buộc để làm việc với FlashTools

public class FrogAnimation : MonoBehaviour
{
    private SwfClipController _controller;

    // Các hằng số chứa tên chính xác của 5 Sequence từ file Asset
    public const string ANIM_IDLE_SLEEPING = "L_IdleSleeping";
    public const string ANIM_WAKE = "O_Wake";
    public const string ANIM_ATTACK = "N_Attack";
    public const string ANIM_GUTS_VISITOR = "L_GutsVisitor";
    public const string ANIM_GUTS = "L_Guts";

    private void Awake()
    {
        _controller = GetComponent<SwfClipController>();
        if (_controller == null)
        {
            Debug.LogError("[FrogAnimationTest] Không tìm thấy SwfClipController trên GameObject này!");
        }
    }

    private void Start()
    {
        // Khi bắt đầu game, mặc định cho ếch ngủ
        PlayIdleSleeping();
    }

    private void Update()
    {
        // Bấm phím 1: Con ếch ngủ (Loop)
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.A))
        {
            PlayIdleSleeping();
        }

        // Bấm phím 2: Con ếch thức giấc (Phát 1 lần)
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.B))
        {
            PlayWake();
        }

        // Bấm phím 3 / Space: Con ếch tấn công
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.C))
        {
            PlayAttack();
        }

        // Bấm phím 4: Chạy animation L_GutsVisitor
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.D))
        {
            PlayGutsVisitor();
        }

        // Bấm phím 5: Chạy animation L_Guts
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.E))
        {
            PlayGuts();
        }
    }

    #region Các hàm public hỗ trợ gọi Animation từ Code ngoài hoặc UI Button

    public void PlayIdleSleeping()
    {
        PlaySequence(ANIM_IDLE_SLEEPING, SwfClipController.LoopModes.Loop);
    }

    public void PlayWake()
    {
        PlaySequence(ANIM_WAKE, SwfClipController.LoopModes.Once);
    }

    public void PlayAttack()
    {
        PlaySequence(ANIM_ATTACK, SwfClipController.LoopModes.Once);
    }

    public void PlayGutsVisitor()
    {
        PlaySequence(ANIM_GUTS_VISITOR, SwfClipController.LoopModes.Once);
    }

    public void PlayGuts()
    {
        PlaySequence(ANIM_GUTS, SwfClipController.LoopModes.Once);
    }

    /// <summary>
    /// Hàm lõi điều khiển chuyển đổi Sequence và chế độ Lặp
    /// </summary>
    public void PlaySequence(string sequenceName, SwfClipController.LoopModes loopMode)
    {
        if (_controller == null) return;

        _controller.loopMode = loopMode;
        _controller.GotoAndPlay(sequenceName, 0);

        Debug.Log($"[FrogAnimationTest] Đã kích hoạt Animation: {sequenceName} (Mode: {loopMode})");
    }

    #endregion
}