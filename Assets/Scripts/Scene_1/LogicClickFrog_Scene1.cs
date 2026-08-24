using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicClickFrog_Scene1 : MonoBehaviour
{
    public string animationIdleSleep;
    public string animationClickNormal;
    public string animationClickAttack;
    public string animationGutsVisitor;
    public string animationGuts;
    public GameObject womGameObject;
    public TestSwfAnimation animationSwfAnimation;
    public bool isTarget;
    public bool isParasitized;

    public FishingRod_Ver2 fishingRodVer2;

    public CircleCollider2D circleCollider;

    private void OnMouseDown()
    {
        // đã xong khi bị ký sinh
        if (isParasitized && fishingRodVer2.isFishReturn)
        {
            circleCollider.enabled = false;
            animationSwfAnimation.sequenceName = animationGuts;
            animationSwfAnimation.PlayTestAnimation();
            fishingRodVer2.PlayAnimationJumpVisitor();
        }
        if (isParasitized)
        {
            return;
        }

        if (isTarget)
        {
            animationSwfAnimation.sequenceName = animationClickAttack;
            circleCollider.enabled = false;

            Vector3 newPos = transform.position;
            newPos.x += 0.1f;
            transform.position = newPos;

            animationSwfAnimation.PlayTestAnimation(() =>
            {
                circleCollider.enabled = true;
                animationSwfAnimation.sequenceName = animationGutsVisitor;
                animationSwfAnimation.loopMode = FTRuntime.SwfClipController.LoopModes.Loop;
                animationSwfAnimation.PlayTestAnimation();
                isParasitized = true;
            });
            womGameObject.gameObject.SetActive(false);
        }
        else
        {
            animationSwfAnimation.sequenceName = animationClickNormal;
            circleCollider.enabled = false;
            animationSwfAnimation.PlayTestAnimation(() =>
            {
                animationSwfAnimation.sequenceName = animationIdleSleep;
                animationSwfAnimation.PlayTestAnimation();
                circleCollider.enabled = true;
            });
        }
    }
}
