using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandle : MonoBehaviour
{

    public Cat_Scene2 cat;
    public Visitor_Scene2 visitor;

    public BoxCollider2D boxCollider2D;

    private void OnMouseDown()
    {
        if (cat.isNearDoor)
        {
            boxCollider2D.enabled = false;
            cat.PlayAnimationAttackWom(visitor, boxCollider2D);
        }
        else
        {
            visitor.PlayAnimationJumpToHandle();
            boxCollider2D.enabled = false;
        }
    }

}
