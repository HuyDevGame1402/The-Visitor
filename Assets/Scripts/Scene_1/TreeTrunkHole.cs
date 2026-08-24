using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTrunkHole : MonoBehaviour
{

    public LogicClickBirdFlag logicClickBirdFlag;
    public CapsuleCollider2D capsuleCollider;

    private void OnMouseDown()
    {
        if(logicClickBirdFlag.isHasBird && logicClickBirdFlag.isReadyFly)
        {
            logicClickBirdFlag.PlayAnimationStumpEmpty();
            capsuleCollider.enabled = false;
        }
    }
}
