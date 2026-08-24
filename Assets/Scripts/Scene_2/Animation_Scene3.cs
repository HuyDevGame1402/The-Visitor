using UnityEngine;
using System;
using System.Collections;
using FTRuntime;
public class Animation_Scene3 : MonoBehaviour
{
    [Header("Cấu hình Loop & Callback")]
    [Tooltip("Chế độ lặp animation (Once, Loop, v.v.)")]
    public SwfClipController.LoopModes loopMode = SwfClipController.LoopModes.Once;

    [Tooltip("Khoảng thời gian (giây) kích hoạt callback trước khi animation kết thúc. 0 = chạy hết.")]
    public float triggerOffsetTime = 0f;

    private SwfClipController _controller;
    private Coroutine _trackAnimationCoroutine;

    public Visitor_Scene3 visitor;

    public GameObject scene3;

    private readonly string[] _sequences = new string[]
    {
        "N_Init",   // Phím 1 (Index 0)
        "L_P1",     // Phím 2 (Index 1)
        "O_P1P4",   // Phím 3 (Index 2)
        "L_P4",     // Phím 4 (Index 3)
        "O_P1P3",   // Phím 5 (Index 4)
        "L_P3",     // Phím 6 (Index 5)
        "O_P1P2",   // Phím 7 (Index 6)
        "L_P2",     // Phím 8 (Index 7)
        "O_P1P1",   // Phím 9 (Index 8)
        "O_P4P1",   // Phím 0 (Index 9)
        "O_P4P2",   // Phím A (Index 10)
        "O_P4P3",   // Phím B (Index 11)
        "O_P4P4",   // Phím C (Index 12)
        "O_P3P1",   // Phím D (Index 13)
        "O_P3P2",   // Phím E (Index 14)
        "O_P3P4",   // Phím F (Index 15)
        "O_P3P3",   // Phím G (Index 16)
        "O_P2P5",   // Phím H (Index 17)
        "O_P2P1",   // Phím I (Index 18)
        "O_P2P3",   // Phím J (Index 19)
        "O_P2P2"    // Phím K (Index 20)
    };

    private readonly KeyCode[] _testKeys = new KeyCode[]
    {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5,
        KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0,
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K
    };

    private void Awake()
    {
        _controller = GetComponent<SwfClipController>();
        if (_controller == null)
        {
            Debug.LogError("[Animation_Scene3] Không tìm thấy SwfClipController trên GameObject này!");
        }
    }

    private void Update()
    {
        if (_controller == null) return;

        for (int i = 0; i < _testKeys.Length; i++)
        {
            if (Input.GetKeyDown(_testKeys[i]))
            {
                PlayAnimationByIndex(i);
                break;
            }
        }
    }

    public void PlayAnimationByIndex(int index, Action onComplete = null, float? offsetTime = null)
    {
        if (index < 0 || index >= _sequences.Length) return;
        PlayAnimationByName(_sequences[index], onComplete, offsetTime);
    }

    public void PlayAnimationByName(string seqName, Action onComplete = null, float? offsetTime = null)
    {
        if (_controller == null) return;

        if (string.IsNullOrEmpty(seqName))
        {
            Debug.LogWarning("[Animation_Scene3] Tên sequence đang bị trống!");
            return;
        }
        if (_trackAnimationCoroutine != null)
        {
            StopCoroutine(_trackAnimationCoroutine);
            _trackAnimationCoroutine = null;
        }

        _controller.loopMode = loopMode;
        _controller.GotoAndPlay(seqName, 0);

        Debug.Log($"[Animation_Scene3] Đang phát Sequence: {seqName} | LoopMode: {loopMode}");

        float actualOffset = offsetTime ?? triggerOffsetTime;

        if (onComplete != null)
        {
            _trackAnimationCoroutine = StartCoroutine(TrackAnimationCompleteRoutine(seqName, onComplete, actualOffset));
        }
    }

    private IEnumerator TrackAnimationCompleteRoutine(string seqName, Action callback, float offset)
    {
        yield return null;

        if (_controller == null || _controller.clip == null) yield break;

        float frameRate = _controller.clip.frameRate > 0 ? _controller.clip.frameRate : 30f;
        float totalDuration = _controller.clip.frameCount / frameRate;

        float targetTime = Mathf.Max(0f, totalDuration - offset);
        if (targetTime <= 0f)
        {
            _controller.Stop(false);
            Debug.Log($"[Animation_Scene3] Sequence {seqName} bị dừng ngay lập tức do offsetTime >= thời lượng animation!");
            callback?.Invoke();
            _trackAnimationCoroutine = null;
            yield break;
        }

        float elapsedTime = 0f;
        while (_controller != null && _controller.isPlaying && elapsedTime < targetTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (offset > 0f && _controller != null && _controller.isPlaying)
        {
            _controller.Stop(false);
        }

        Debug.Log($"[Animation_Scene3] Sequence {seqName} đã hoàn thành (hoặc chạm mốc Offset: {offset}s)!");
        callback?.Invoke();
        _trackAnimationCoroutine = null;
    }

    // gọi khi visitor đang ở khí 0 -> 3 và bấm van vàng
    public void PlayAnimationJumpValve_1_4()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[2], () => {
            PlayAnimationByName(_sequences[3], () =>
            {
                visitor.isReady = true;
            });
        });
    }

    public void PlayAnimationJumpValve_1_2()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[6], () => {
            PlayAnimationByName(_sequences[7], () =>
            {
                visitor.isReady = true;
            });
        });
    }

    public void PlayAnimationJumpValve_1_3()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[4], () => {
            PlayAnimationByName(_sequences[5], () =>
            {
                visitor.isReady = true;
            });
        });
    }

    public void PlayAnimationJumpValve_4_1()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[9], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_3_1()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[13], () => {
            visitor.isReady = true;
        });
    }

    public void PlayAnimationJumpValve_2_1()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[18], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_3_4()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[15], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_4_3()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[11], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_3_2()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[14], () => {
            visitor.isReady = true;
        });
    }

    public void PlayAnimationJumpValve_2_3()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[19], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_4_2()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[10], () => {
            visitor.isReady = true;
        });
    }

    public void PlayAnimationJumpValve_2_2()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[20], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_3_3()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[16], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_4_4()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[12], () => {
            visitor.isReady = true;
        });
    }
    public void PlayAnimationJumpValve_2_5()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[17], () => {
            visitor.isReady = true;
            scene3.SetActive(false);
        });
    }
    public void PlayAnimationJumpValve_1_1()
    {
        visitor.isReady = false;
        PlayAnimationByName(_sequences[9], () => {
            PlayAnimationByName(_sequences[1], () =>
            {
                visitor.isReady = true;
            });
        });
    }
}