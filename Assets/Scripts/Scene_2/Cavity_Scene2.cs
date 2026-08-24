using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cavity_Scene2 : MonoBehaviour
{
    public Visitor_Scene2 visitor;
    public Cat_Scene2 cat;

    private void OnMouseDown()
    {
        if (visitor.isToFaceCat)
        {
            visitor.isReadyAttackTree = false;
            visitor.isNearDoor = false;

            visitor.PlayAnimationMovetoCavity();
        }
    }
}
