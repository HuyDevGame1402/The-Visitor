using FTRuntime;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class Lady_Scene4 : MonoBehaviour
{
    private readonly string[] _sequences = new string[]
    {
        "L_IdleWash",            // Phím 1 (Index 0)
        "N_OpenDrawer",          // Phím 2 (Index 1)
        "O_CloseDrawer",         // Phím 3 (Index 2)
        "O_TurnOffThermo",     // Phím 4 (Index 3)
        "N_NoticeMess",         // Phím 5 (Index 4)
        "L_Clean",               // Phím 6 (Index 5)
        "N_CleanDrawerOpen",     // Phím 7 (Index 6)
        "L_CleanDrawerOpen",     // Phím 8 (Index 7)
        "O_BackToDishes",      // Phím 9 (Index 8)
        "S_SlipDrawerKnife",    // Phím 0 (Index 9)
        "O_Slip",                // Phím Q (Index 10)
        "O_SlipDrawer",         // Phím W (Index 11)
        "O_Drawer",              // Phím E (Index 12)
        "O_Look",                // Phím R (Index 13)
        "O_DrawerKnife"          // Phím T (Index 14)
    };

    public TestSwfAnimation animationSwf;

    public bool isClear = false;
    public bool isClearWaterOrrange = false;
    public bool isCloseDraw;
    public bool isDie;

    public SwfClip swfClip;

    public CollisionDrawer drawer;
    public bool isLockOpenDrawer;
    public WaterTapLock waterTapLock;
    public Background_Scene4 bg;
    public KnifeAnimation_Scene4 knife;
    public KnifeAnimationVisualMove knifeAnimation;

    private void Start()
    {
        // Mặc định chạy animation đầu tiên khi bắt đầu test
        PlayAnimationByIndex(0);
    }

    private void Update()
    {
        // Đọc phím số hàng trên bàn phím (1-9, 0)
        if (Input.GetKeyDown(KeyCode.Alpha1)) PlayAnimationByIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) PlayAnimationByIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) PlayAnimationByIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) PlayAnimationByIndex(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) PlayAnimationByIndex(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) PlayAnimationByIndex(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) PlayAnimationByIndex(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) PlayAnimationByIndex(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) PlayAnimationByIndex(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) PlayAnimationByIndex(9);

        // Đọc các phím chữ cái tiếp theo cho các animation còn lại
        if (Input.GetKeyDown(KeyCode.Q)) PlayAnimationByIndex(10);
        if (Input.GetKeyDown(KeyCode.W)) PlayAnimationByIndex(11);
        if (Input.GetKeyDown(KeyCode.E)) PlayAnimationByIndex(12);
        if (Input.GetKeyDown(KeyCode.R)) PlayAnimationByIndex(13);
        if (Input.GetKeyDown(KeyCode.T)) PlayAnimationByIndex(14);
    }

    private void PlayAnimationByIndex(int index)
    {
        if (index >= 0 && index < _sequences.Length)
        {
            string seqName = _sequences[index];
            animationSwf.sequenceName = seqName;

            // Cài đặt chế độ lặp: Nếu là Idle hoặc Clean thì để Loop, còn lại chạy Once
            if (seqName.StartsWith("L_"))
            {
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            }
            else
            {
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
            }

            animationSwf.PlayTestAnimation();
            Debug.Log($"Đang phát Animation [{index}]: {seqName}");
        }
    }

    public void PlayAnimationImpactWithThermostat(Thermostat_Scene4 thermostat_Scene4)
    {
        swfClip.sortingOrder = 30;
        animationSwf.sequenceName = _sequences[3];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            thermostat_Scene4.isReady = true;
            animationSwf.sequenceName = _sequences[0];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
        });
    }

    public void PlayAnimationTurnAround()
    {
        swfClip.sortingOrder = 30;
        animationSwf.sequenceName = _sequences[13];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[0];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
        });
    }

    // 5-6 đi ra lau, 8-1 quay về

    public void PlayAnimationClear(BlenderAnimation_Scene4 blender)
    {
        isClear = true;
        isClearWaterOrrange = true;
        isLockOpenDrawer = true;
        waterTapLock.isReady = true;
        swfClip.sortingOrder = 30;
        animationSwf.sequenceName = _sequences[4];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            blender.PlayAnimationByIndex(0);
            animationSwf.sequenceName = _sequences[5];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
            animationSwf.PlayTestAnimation();
            isLockOpenDrawer = false;
            StartCoroutine(CoroutineClearAnimation(blender));
        });
    }

    private IEnumerator CoroutineClearAnimation(BlenderAnimation_Scene4 blender)
    {
        yield return new WaitForSeconds(2.5f);
        isClearWaterOrrange = false;
        PlayAnimationMoveToKitchenSink(blender);
    }

    private void PlayAnimationMoveToKitchenSink(BlenderAnimation_Scene4 blender)
    {
        swfClip.sortingOrder = 30;
        if (drawer.isOpenDraw)
        {
            if (bg.isWaterInGround)
            {
                if (drawer.isHasKnife)
                {
                    // trượt té khi có dao ở ngăn kéo
                    knife.gameObject.SetActive(false);
                    animationSwf.sequenceName = _sequences[9];
                    isDie = true;
                }
                else
                {
                    animationSwf.sequenceName = _sequences[11];
                }
            }
            else
            {
                if (drawer.isHasKnife)
                {
                    // đẩy con dao về vị trí cũ
                    knife.gameObject.SetActive(false);
                    animationSwf.sequenceName = _sequences[14];
                }
                else
                {
                    animationSwf.sequenceName = _sequences[12];
                }
            }
        }
        else
        {
            if (bg.isWaterInGround)
            {
                animationSwf.sequenceName = _sequences[10];
            }
            else
            {
                animationSwf.sequenceName = _sequences[8];
            }
        }
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isClear = false;
            blender.isHasOrrange = false;
            blender.isAnimationRunning = false;
            blender.isOpen = false;

            if (drawer.isHasKnife && isDie == false)
            {
                knifeAnimation.PlayAnimation();
                drawer.isHasKnife = false;
            }

            if (bg.isWaterInGround && isDie == false)
            {
                bg.PlayAnimationClear();
                bg.isWaterInGround = false;
            }
            if (drawer.isOpenDraw && isDie == false)
            {
                swfClip.sortingOrder = 15;
                animationSwf.sequenceName = _sequences[2];
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
                animationSwf.PlayTestAnimation(() =>
                {
                    PlayAnimationByIndex(0);
                    swfClip.sortingOrder = 30;
                    drawer.isOpenDraw = false;
                });
            }
            else
            {
                if (isDie) return;
                PlayAnimationByIndex(0);
            }
        });
    }

    public void PlayAnimationOpenDraw(CollisionDrawer draw)
    {
        swfClip.sortingOrder = 15;
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            animationSwf.sequenceName = _sequences[2];
            animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
            animationSwf.PlayTestAnimation(() =>
            {
                PlayAnimationByIndex(0);
                swfClip.sortingOrder = 30;
                draw.isOpenDraw = false;
            });
        });
    }

    public void PlayAnimationOpenDrawWithClear()
    {
        //PlayAnimationByIndex(6);

        animationSwf.sequenceName = _sequences[6];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            if (isClearWaterOrrange)
            {
                animationSwf.sequenceName = _sequences[7];
                animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
                animationSwf.PlayTestAnimation();
            }
        });

        swfClip.sortingOrder = 15;
    }
}