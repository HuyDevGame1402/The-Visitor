using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicMeteoClick : MonoBehaviour
{

    public Fish_Scene1 fish;

    public CapsuleCollider2D capsuleCollider2;

    public Animator animator;
    public float timeDelay;
    public bool isReady;
    public bool isLock;

    private void OnMouseDown()
    {
        if (isReady == false || isLock == true) return;
        capsuleCollider2.enabled = false;
        animator.SetTrigger("OnClick");
        StartCoroutine(CoroutineTimeDelay());
        fish.PlayAnimationMove();
    }

    private IEnumerator CoroutineTimeDelay()
    {
        yield return new WaitForSeconds(timeDelay);
        capsuleCollider2.enabled = true;
    }
}
