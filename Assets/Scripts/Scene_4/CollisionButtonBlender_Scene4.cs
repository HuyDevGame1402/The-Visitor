using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButtonBlender_Scene4 : MonoBehaviour
{
    public BlenderAnimation_Scene4 blender;

    private void OnMouseDown()
    {
        if (blender.isOpen)
        {
            blender.PlayAnimationRunningWithOpen();
        }
        else
        {
            blender.PlayAnimationRunningWithClose();
        }
    }
}
