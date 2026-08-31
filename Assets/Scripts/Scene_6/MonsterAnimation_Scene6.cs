using UnityEngine;

public class MonsterAnimation_Scene6 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;
    public BackgroundScene_6 bg;
    public KidAnimation_Scene6 kid;

    public bool isEndGame;

    public bool isRecover;
    public int bullet;
    public bool isReadyHitDamage;
    public int hitCount;

    private readonly string[] _sequences = new string[]
    {
        "L_Idle",                 // Phím 1
        "S_Enter",                // Phím 2
        "S_Slip",                 // Phím 3
        "S_Recover",              // Phím 4
        "N_Electro",              // Phím 5
        "S_ElectroExplodeHuman",   // Phím 6
        "N_KillHuman",            // Phím 7
        "N_ExplodeHuman",         // Phím 8
        "S_BreakWindow",          // Phím 9
        "S_Gunfight",             // Phím 0
        "S_Shoot1",               // Phím - (Minus)
        "S_Shoot2",               // Phím = (Equals)
        "N_Shoot3",               // Phím Q
        "S_FallDown",             // Phím W
        "S_Hit1",                 // Phím E
        "S_Hit2"                  // Phím R
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
            Debug.LogError("[MonsterAnimation_Scene6] Chưa gán TestSwfAnimation vào Inspector!");
        }
    }

    private void Update()
    {
        if (animationSwf == null) return;

        // Bấm các phím số từ 1 đến 9 (Index 0..8)
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                PlaySequenceAtIndex(i);
                return;
            }
        }

        // Key mapping mở rộng cho các animation còn lại
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) PlaySequenceAtIndex(9);  // S_Gunfight
        else if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) PlaySequenceAtIndex(10); // S_Shoot1
        else if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus)) PlaySequenceAtIndex(11); // S_Shoot2
        else if (Input.GetKeyDown(KeyCode.Q)) PlaySequenceAtIndex(12); // N_Shoot3
        else if (Input.GetKeyDown(KeyCode.W)) PlaySequenceAtIndex(13); // S_FallDown
        else if (Input.GetKeyDown(KeyCode.E)) PlaySequenceAtIndex(14); // S_Hit1
        else if (Input.GetKeyDown(KeyCode.R)) PlaySequenceAtIndex(15); // S_Hit2
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
            Debug.Log($"[MonsterAnimation_Scene6] Chạy xong Sequence: {targetSequence}");
        });
    }

    public void PlayAnimationAttackHuman()
    {
        isEndGame = true;
        animationSwf.sequenceName = _sequences[1];
        animationSwf.PlayTestAnimation(() => 
        {
            animationSwf.sequenceName = _sequences[7];
            animationSwf.PlayTestAnimation(() =>
            {
                bg.PlayAnimationWindowBroken();
                animationSwf.sequenceName = _sequences[8];
                animationSwf.PlayTestAnimation(() => 
                { 
                    // endgame
                });
            });
            kid.PlayAnimationExplode();
        });
    }

    public void PlayAnimationSlip()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.PlayTestAnimation();
    }

    public void PlayAnimationRecover()
    {
        animationSwf.sequenceName = _sequences[3];
        animationSwf.PlayTestAnimation(() =>
        {
            isRecover = true;
        });
    }
    public void PlayAnimationShoot()
    {
        bullet += 1;
        isReadyHitDamage = false;
        animationSwf.sequenceName = _sequences[9 + bullet];
        animationSwf.PlayTestAnimation(() =>
        {
            if(bullet == 3)
            {
                PlayAnimationFallDown();
            }
            else
            {
                isReadyHitDamage = true;
            }
        });
    }
    public void PlayAnimationHitDamage()
    {
        hitCount += 1;
        isReadyHitDamage = false;
        animationSwf.sequenceName = _sequences[13 + hitCount];
        animationSwf.PlayTestAnimation(() =>
        {
            isReadyHitDamage = true;
        });
    }
    public void PlayAnimationFallDown()
    {
        animationSwf.sequenceName = _sequences[13];
        animationSwf.PlayTestAnimation(() =>
        {
            isReadyHitDamage = true;
        });
    }
}