using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAttackBird1_Scene5 : MonoBehaviour
{
    public EatBirdAnimation_Scene5 eatBird;
    public BirdAnimation_Scene5 bird;

    private void OnMouseDown()
    {
        if(eatBird.isReadyAttack && bird.isReadyAnimationMove)
        {
            eatBird.PlayAnimationLash();
            bird.PlayAnimationMove();
        }
    }
}
