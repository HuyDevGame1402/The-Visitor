using UnityEngine;
using FTRuntime; // Namespace của FlashTools

public class TestSwfAnimationKeys : MonoBehaviour
{
    [Header("Cấu hình Loop")]
    [Tooltip("Chế độ lặp animation (Once, Loop, v.v.)")]
    public SwfClipController.LoopModes loopMode = SwfClipController.LoopModes.Once;

    private SwfClipController _controller;

    // Danh sách 17 animation theo thứ tự yêu cầu
    private readonly string[] _sequences = new string[]
    {
        "S_Init",                      // Phím 1 (Index 0)
        "N_Arrive",                    // Phím 2 (Index 1)
        "L_WaitingVisitorApple",       // Phím 3 (Index 2)
        "L_WaitingNoVisitorApple",    // Phím 4 (Index 3)
        "S_EatApple",                  // Phím 5 (Index 4)
        "N_JumpAgain",                 // Phím 6 (Index 5)
        "S_KnobSlipDown",              // Phím 7 (Index 6)
        "O_KnobJumpUp",                // Phím 8 (Index 7)
        "S_Turn Around",               // Phím 9 (Index 8)
        "N_Slideln",                   // Phím 0 (Index 9)
        "S_CatPounceMiss",             // Phím A (Index 10)
        "S_SlideOut",                  // Phím B (Index 11)
        "N_VisitorKnock",              // Phím C (Index 12)
        "S_CatPounceHit",              // Phím D (Index 13)
        "N_VisitorBranch",             // Phím E (Index 14)
        "O_CatHuh",                    // Phím F (Index 15)
        "S_WindowSplatBarf"            // Phím G (Index 16)
    };

    // Bảng ánh xạ phím tương ứng từ 1..0 -> A..G
    private readonly KeyCode[] _testKeys = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0,
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G
    };

    private void Awake()
    {
        _controller = GetComponent<SwfClipController>();
        if (_controller == null)
        {
            Debug.LogError("[TestSwfAnimationKeys] Không tìm thấy SwfClipController trên GameObject này!");
        }
    }

    private void Update()
    {
        if (_controller == null) return;

        // Lặp qua danh sách phím bấm để kiểm tra
        for (int i = 0; i < _testKeys.Length; i++)
        {
            if (Input.GetKeyDown(_testKeys[i]))
            {
                PlayAnimationByIndex(i);
                break;
            }
        }
    }

    private void PlayAnimationByIndex(int index)
    {
        if (index < 0 || index >= _sequences.Length) return;

        string seqName = _sequences[index];
        _controller.loopMode = loopMode;
        _controller.GotoAndPlay(seqName, 0);

        Debug.Log($"[TestSwfAnimationKeys] [Phím: {_testKeys[index]}] Đang phát Sequence: {seqName}");
    }
}