using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicClickBranchInWater_Scene1 : MonoBehaviour
{

    public GameObject womSpriteAnimation;
    public GameObject womGameObject;
    public LogicClickFrog_Scene1 frog;
    public CapsuleCollider2D capsuleCollider;
    public LogicMeteoClick logicMeteoClick;

    private void OnMouseDown()
    {
        womSpriteAnimation.SetActive(false);
        womGameObject.SetActive(true);
        womGameObject.GetComponent<TestSwfAnimation>().PlayTestAnimation(
            () =>
            {
                logicMeteoClick.isReady = true;
                frog.isTarget = true;

            });
        capsuleCollider.enabled = false;
    }
}
