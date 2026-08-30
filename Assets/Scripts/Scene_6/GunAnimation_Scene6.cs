using UnityEngine;

public class GunAnimation_Scene6 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

    public bool isReadyShoot;

    private readonly string[] _sequences = new string[]
    {
        "S_Clear",     // Phím 1
        "S_Gun",       // Phím 2
        "S_ShootGun",  // Phím 3
        "S_FlipGun",   // Phím 4
        "S_HitGun1",   // Phím 5
        "S_HitGun2"    // Phím 6
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
            Debug.LogError("[GunAnimation_Scene6] Chưa gán TestSwfAnimation vào Inspector!");
        }
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm các phím từ 1 đến 6 (hỗ trợ cả bàn phím chính và bàn phím số Keypad)
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
            Debug.Log($"[GunAnimation_Scene6] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationGetGun()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.PlayTestAnimation(() =>
        {
            isReadyShoot = true;
        });
    }

    public void PlayAnimationShoot()
    {
        if (isReadyShoot == false) return;
        animationSwf.sequenceName = _sequences[2];
        animationSwf.PlayTestAnimation(() =>
        {
            
        });
    }
}