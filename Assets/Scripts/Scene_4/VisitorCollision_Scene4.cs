using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorCollision_Scene4 : MonoBehaviour
{
    public VisitorAnimation_Scene4 visitor;
    public float timeDelayLadyTurnAround = 0.1f;

    public Lady_Scene4 lady;

    private void OnMouseDown()
    {
        if (lady.isDie)
        {
            visitor.PlayAnimationCounter();
            return;
        }

        if (lady.isClear == true) return;
        if (visitor.isReadyJumpUp)
        {
            visitor.PlayAnimationJumpUp();
            StartCoroutine(CoroutineDelayTurnAround());
        }
    }

    private IEnumerator CoroutineDelayTurnAround()
    {
        yield return new WaitForSeconds(timeDelayLadyTurnAround);
        lady.PlayAnimationTurnAround();
    }
}
