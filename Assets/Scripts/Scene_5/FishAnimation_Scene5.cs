using UnityEngine;

public class FishAnimation_Scene5 : MonoBehaviour
{
    [Header("Ref References")]
    public TestSwfAnimation animationSwf;

    public bool isReady;

    public GameObject collisionInit;
    public GameObject collisionFishVisitor;

    private readonly string[] _sequences = new string[]
    {
        "L_IdleFish",  // Phím 1
        "S_Splash",    // Phím 2
        "S_IdleBlood"  // Phím 3
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
            Debug.LogError("[FishAnimation_Scene5] Chưa gán TestSwfAnimation vào Inspector!");
        }
        PlayAnimationIdle();
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm phím 1 (hoặc Keypad 1) -> Phát "L_IdleFish"
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            PlaySequenceAtIndex(0);
        }
        // Bấm phím 2 (hoặc Keypad 2) -> Phát "S_Splash"
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlaySequenceAtIndex(1);
        }
        // Bấm phím 3 (hoặc Keypad 3) -> Phát "S_IdleBlood"
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlaySequenceAtIndex(2);
        }
    }

    /// <summary>
    /// Phát animation theo chỉ số trong mảng _sequences
    /// </summary>
    /// <param name="index">Chỉ số phần tử (0, 1, 2)</param>
    private void PlaySequenceAtIndex(int index)
    {
        if (index < 0 || index >= _sequences.Length) return;

        string targetSequence = _sequences[index];

        // Gán tên sequence vào controller
        animationSwf.sequenceName = targetSequence;

        // Gọi hàm phát animation (có thể kèm callback kết thúc nếu muốn)
        animationSwf.PlayTestAnimation(() =>
        {
            Debug.Log($"[FishAnimation_Scene5] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationSplash()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isReady = true;
            collisionInit.SetActive(false);
            collisionFishVisitor.SetActive(true);
        });
    }

    public void PlayAnimationEmpty()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationIdle()
    {
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
        PlaySequenceAtIndex(0);
    }
}