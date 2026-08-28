using UnityEngine;

public class EatBirdAnimation_Scene5 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

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

        // Gọi hàm phát animation
        animationSwf.PlayTestAnimation(() =>
        {
            Debug.Log($"[EatBirdAnimation_Scene5] Chạy xong Sequence: {targetSequence}");
        });
    }
}