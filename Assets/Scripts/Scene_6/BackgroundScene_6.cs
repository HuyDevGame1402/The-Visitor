using UnityEngine;

public class BackgroundScene_6 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

    public MonsterAnimation_Scene6 monster;
    public GameObject collisionToiletPaper;
    public bool isEndGame;

    private readonly string[] _sequences = new string[]
    {
        "L_IdleOpen",     // Phím 1
        "L_Close",        // Phím 2 // mo cua k co cua
        "O_Break",        // Phím 3 // pha cua
        "L_Banging",      // Phím 4 // dap cua
        "S_WindowBroken"  // Phím 5 // vo kinh
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
            Debug.LogError("[BackgroundScene_6] Chưa gán TestSwfAnimation vào Inspector!");
        }
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm các phím từ 1 đến 5 (hỗ trợ cả bàn phím chính và bàn phím số Keypad)
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
            PlaySequenceAtIndex(4);
        }
    }

    /// <summary>
    /// Phát animation theo chỉ số trong mảng _sequences
    /// </summary>
    private void PlaySequenceAtIndex(int index)
    {
        if (index < 0 || index >= _sequences.Length) return;

        string targetSequence = _sequences[index];

        // Gán tên sequence vào controller
        animationSwf.sequenceName = targetSequence;

        // Gọi hàm phát animation
        animationSwf.PlayTestAnimation(() =>
        {
            Debug.Log($"[BackgroundScene_6] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationWindowBroken()
    {
        animationSwf.sequenceName = _sequences[4];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationDoorClose()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            collisionToiletPaper.SetActive(true);
        });
    }

    public void PlayAnimationBreak()
    {
        isEndGame = true;
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            monster.PlayAnimationAttackHuman();
        });
    }

    public void PlayAnimationBreakVer1()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            monster.PlayAnimationSlip();
        });
    }

    public void PlayAnimationBanging()
    {
        animationSwf.sequenceName = _sequences[3];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
        animationSwf.PlayTestAnimation();
    }
}