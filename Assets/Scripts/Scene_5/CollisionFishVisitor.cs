using UnityEngine;

public class CollisionFishVisitor : MonoBehaviour
{
    public FishAnimation_Scene5 fish;
    public BirdAnimation_Scene5 bird;
    public EatBirdAnimation_Scene5 eatBird;

    public bool isReady;
    private void OnMouseDown()
    {
        if (fish.isReady && isReady)
        {
            fish.PlayAnimationEmpty();
            bird.PlayAnimationCanTip();
            eatBird.PlayAnimationJumpDown();
            gameObject.SetActive(false);
        }
    }
}
