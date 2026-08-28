using UnityEngine;

public class CollisionKnife : MonoBehaviour
{
    public KnifeAnimation_Scene4 knife;

    public Lady_Scene4 lady;
    public CollisionDrawer drawer;

    private void OnMouseDown()
    {
        if (knife.isReady)
        {
            if (lady.isClear && lady.isClearWaterOrrange && drawer.isOpenDraw)
            {
                knife.PlayAnimationMoveToDraw();
                drawer.isHasKnife = true;
            }
            else
            {
                knife.PlayAnimationOLift();
            }
        }
    }
}
