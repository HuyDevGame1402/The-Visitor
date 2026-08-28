using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDrawer : MonoBehaviour
{
    public Lady_Scene4 lady;

    public bool isOpenDraw;
    public bool isHasKnife;

    private void OnMouseDown()
    {
        if (isOpenDraw == true || lady.isLockOpenDrawer) return;
        if (lady.isClear == true && lady.isClearWaterOrrange == false) return;
        isOpenDraw = true;
        if (lady.isClear)
        {
            lady.PlayAnimationOpenDrawWithClear();
        }
        else
        {
            lady.PlayAnimationOpenDraw(this);
        }
    }
}
