using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFish_Scene5 : MonoBehaviour
{
    public Background_Scene5 bg;
    public FishAnimation_Scene5 fish;

    public bool isDie;

    private void OnMouseDown()
    {
        if (bg.isReady && isDie == false)
        {
            bg.PlayAnimationEmpty();
            fish.PlayAnimationSplash();
            isDie = true;
        }
    }
}
