using UnityEngine;

public class BirdAnimation_Scene5 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

    public bool isReadyAnimationMove;
    public bool eating;

    public GameObject collisionBird2;

    private readonly string[] _sequences = new string[]
    {
        "L_IdleCage", // Phím 1
        "S_Fly",      // Phím 2
        "S_Move",     // Phím 3
        "S_CanTip",   // Phím 4
        "N_Eat",      // Phím 5
        "L_IdleEat",  // Phím 6
        "S_BirdGone"  // Phím 7
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
            Debug.LogError("[BirdAnimation_Scene5] Chưa gán TestSwfAnimation vào Inspector!");
        }
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm các phím số từ 1 đến 7 (hỗ trợ cả bàn phím chính và bàn phím số Keypad)
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
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            PlaySequenceAtIndex(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            PlaySequenceAtIndex(6);
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
            Debug.Log($"[BirdAnimation_Scene5] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationFly(CollisionBird_Scene5 birdCollision)
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        // Gọi hàm phát animation
        animationSwf.PlayTestAnimation(() =>
        {
            birdCollision.hasBirdInCage = false;
            birdCollision.visitor.isReady = true;
        });
    }

    public void PlayAnimationCanTip()
    {
        animationSwf.sequenceName = _sequences[3];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationMove()
    {
        isReadyAnimationMove = false;
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isReadyAnimationMove = true;
        });
    }

    public void PlayAnimationEat()
    {
        animationSwf.sequenceName = _sequences[4];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            eating = true;
            animationSwf.sequenceName = _sequences[5];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
            collisionBird2.SetActive(true);
        });
    }

    public void PlayAnimationBirdGone()
    {
        animationSwf.sequenceName = _sequences[6];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }
}