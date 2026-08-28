using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollision_Scene4 : MonoBehaviour
{

    public VisitorAnimation_Scene4 visitor;
    public VentAnimation vent;

    private void OnMouseDown()
    {
        if(visitor.isInCounter && vent.isReady)
        {
            // đu dây visitor
        }
    }
}
