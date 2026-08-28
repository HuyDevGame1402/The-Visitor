using UnityEngine;

public class CollisionFishVisitor : MonoBehaviour
{
    public FishAnimation_Scene5 fish;
    public BirdAnimation_Scene5 bird;
    private void OnMouseDown()
    {
        if (fish.isReady)
        {
            fish.PlayAnimationEmpty();
            bird.PlayAnimationCanTip();
            gameObject.SetActive(false);
        }
    }
}
