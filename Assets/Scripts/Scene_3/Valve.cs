using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Valve : MonoBehaviour
{
    public Visitor_Scene3 visitor;
    public Animation_Scene3 animationScene3;

    protected virtual void OnMouseDown()
    {

    }
}
