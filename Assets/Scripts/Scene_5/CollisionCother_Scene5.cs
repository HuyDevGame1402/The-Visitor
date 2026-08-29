using UnityEngine;

public class CollisionCother_Scene5 : MonoBehaviour
{
    public EatBirdAnimation_Scene5 eatBird;

    private void OnMouseDown()
    {
        if (eatBird.isFaceToBird)
        {
            eatBird.PlayAnimationMoveVisitor();
            eatBird.isFaceToBird = false;
        }

        if (eatBird.isEatBird)
        {
            eatBird.PlayAnimationMoveVisitor();
            gameObject.SetActive(false);
        }
    }
}
