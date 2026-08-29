using UnityEngine;

public class CollisionBird2_Scene5 : MonoBehaviour
{
    public BirdAnimation_Scene5 bird;
    public EatBirdAnimation_Scene5 eatBird;

    private void OnMouseDown()
    {
        bird.PlayAnimationBirdGone();
        eatBird.PlayAnimationAttack();
        gameObject.SetActive(false);
    }
}
