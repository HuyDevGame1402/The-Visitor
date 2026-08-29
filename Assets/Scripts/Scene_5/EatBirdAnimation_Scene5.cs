using UnityEngine;

public class EatBirdAnimation_Scene5 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

    public bool isReadyAttack = true;

    public GameObject collisionBird1;
    public bool isFaceToBird;
    public bool isEatBird;

    public BirdAnimation_Scene5 bird;
    public GameObject collisionMouth;

    private readonly string[] _sequences = new string[]
    {
        "S_Clothes",               // Phím 1
        "S_Move",                  // Phím 2
        "N_JumpDown",              // Phím 3
        "L_ClothesWaterVisitor",   // Phím 4
        "S_Lash",                  // Phím 5
        "S_MoveVisitor",           // Phím 6
        "S_Dive",                  // Phím 7
        "N_Attack",                // Phím 8
        "L_Clothes Feathers",      // Phím 9
        "N_FlyUp",                 // Phím 0
        "L_ClothesSkyVisitor"      // Phím - (Minus)
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
            Debug.LogError("[EatBirdAnimation_Scene5] Chưa gán TestSwfAnimation vào Inspector!");
        }
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm các phím số từ 1 đến 9
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                PlaySequenceAtIndex(i);
                return;
            }
        }

        // Phím 0 (Index 9 -> "N_FlyUp")
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            PlaySequenceAtIndex(9);
        }
        // Phím - / Dấu trừ (Index 10 -> "L_ClothesSkyVisitor")
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            PlaySequenceAtIndex(10);
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
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
        // Gọi hàm phát animation
        animationSwf.PlayTestAnimation(() =>
        {
            Debug.Log($"[EatBirdAnimation_Scene5] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationJumpDown()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[3];
            animationSwf.PlayTestAnimation(() =>
            {
                collisionBird1.SetActive(true);
                isFaceToBird = true;
            });
        });
    }

    public void PlayAnimationLash()
    {
        isReadyAttack = false;
        animationSwf.sequenceName = _sequences[4];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isReadyAttack = true;
        });
    }

    public void PlayAnimationMoveVisitor()
    {
        animationSwf.sequenceName = _sequences[6];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            bird.PlayAnimationEat();
        });
    }
    public void PlayAnimationAttack()
    {
        animationSwf.sequenceName = _sequences[7];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isEatBird = true;
        });
    }

    public void PlayAnimationFlyUp()
    {
        animationSwf.sequenceName = _sequences[9];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[10];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
            collisionMouth.SetActive(true);
        });
    }
}