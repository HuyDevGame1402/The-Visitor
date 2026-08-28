using UnityEngine;

public class CollisionOrranges : MonoBehaviour
{
    public BlenderAnimation_Scene4 blender;
    private void OnMouseDown()
    {
        if (blender.isHasOrrange == true) return;
        if (blender.isOpen)
        {
            blender.PlayAnimationAddOrrangeForBlender();
        }
        else
        {
            blender.PlayAnimationEmptyOrrange();
        }
    }
}
