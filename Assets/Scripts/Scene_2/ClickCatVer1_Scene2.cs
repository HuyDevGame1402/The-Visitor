using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCatVer1_Scene2 : MonoBehaviour
{
    public Cat_Scene2 cat;
    public Visitor_Scene2 visitor;
    public GameObject collisionTree;

    private void OnMouseDown()
    {
        if (visitor.isNearDoor)
        {
            cat.PlayAnimationCatAngry();
            if(visitor.TryGetComponent(out SwfFrameRangeController visitorAnimationUpdate))
            {
                visitorAnimationUpdate.PlayAnimationUpdate();
                visitor.isToFaceCat = true;
                collisionTree.SetActive(true);
            }
        }
    }
}
