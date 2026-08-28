using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBlender_Scene4 : MonoBehaviour
{
    public BlenderAnimation_Scene4 blender;
    private void OnMouseDown()
    {
        if(blender.isOpen == false)
        {
            blender.isOpen = true;

            if (blender.isHasWaterOrrange)
            {
                blender.isHasWaterOrrange = false;
                blender.PlayAnimationOpenWithWaterOrrange();
                return;
            }

            if(blender.isHasOrrange == false)
            {
                blender.PlayAnimationOpen();
            }
            else
            {
                blender.PlayAnimationOpenHasOrrange();
            }
        }
    }
}
