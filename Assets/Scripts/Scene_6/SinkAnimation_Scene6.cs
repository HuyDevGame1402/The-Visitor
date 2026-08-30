using System.Collections;
using UnityEngine;

public class SinkAnimation_Scene6 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;

    public GameObject hairdry;
    public float timeDelay;
    public GameObject collisionWaterInSink;

    private readonly string[] _sequences = new string[]
    {
        "S_Idle",         // Phím 1
        "S_OpenCabinet",  // Phím 2
        "S_FillSink",     // Phím 3
        "S_BreakDoor",    // Phím 4
        "S_GunGone"       // Phím 5
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
            Debug.LogError("[SinkAnimation_Scene6] Chưa gán TestSwfAnimation vào Inspector!");
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
            //PlaySequenceAtIndex(4);
            //PlayAnimationOpenCabinet();
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
            Debug.Log($"[SinkAnimation_Scene6] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationOpenCabinet()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.PlayTestAnimation();
        StartCoroutine(CoroutineShowCollisionDoorSink());
    }

    private IEnumerator CoroutineShowCollisionDoorSink()
    {
        yield return new WaitForSeconds(timeDelay);
        hairdry.SetActive(true);
    }

    public void PlayAnimationFillSink()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.PlayTestAnimation(() =>
        {
            collisionWaterInSink.SetActive(true);
        });
    }

    public void PlayAnimationBreakDoor()
    {
        animationSwf.sequenceName = _sequences[3];
        animationSwf.PlayTestAnimation(() =>
        {

        });
    }

    public void PlayAnimationGunGone()
    {
        animationSwf.sequenceName = _sequences[4];
        animationSwf.PlayTestAnimation();
    }
}