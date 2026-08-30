using UnityEngine;

public class HairdryAnimation_Scene6 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

    public GameObject collisionWaterValveSink;
    public GameObject collisionOutlet;

    public SinkAnimation_Scene6 sink;

    private readonly string[] _sequences = new string[]
    {
        "S_Clear",            // Phím 1
        "S_DryerinDrawer",    // Phím 2
        "N_PullOutDryer",     // Phím 3
        "S_Dryer",            // Phím 4
        "S_DryerPluggedIn",   // Phím 5
        "S_DryerInTub",       // Phím 6
        "S_DryerInSink"       // Phím 7
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
            Debug.LogError("[HairdryAnimation_Scene6] Chưa gán TestSwfAnimation vào Inspector!");
        }
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm các phím từ 1 đến 7 (hỗ trợ cả bàn phím chính và bàn phím số Keypad)
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
            Debug.Log($"[HairdryAnimation_Scene6] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationPullOutDryer()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[3];
            animationSwf.PlayTestAnimation(() =>
            {
                collisionOutlet.SetActive(true);
            });
        });
    }
    public void PlayAnimationDryerPluggedIn()
    {
        animationSwf.sequenceName = _sequences[4];
        animationSwf.PlayTestAnimation(() =>
        {
            collisionWaterValveSink.SetActive(true);
        });
    }
    public void PlayAnimationDryerInSink()
    {
        animationSwf.sequenceName = _sequences[6];
        animationSwf.PlayTestAnimation(() =>
        {
            sink.PlayAnimationBreakDoor();
        });
    }
}