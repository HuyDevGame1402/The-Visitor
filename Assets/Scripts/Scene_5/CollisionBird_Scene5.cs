using UnityEngine;

public class CollisionBird_Scene5 : MonoBehaviour
{
    public bool hasBirdInCage;
    public BirdAnimation_Scene5 bird;

    private void OnMouseDown()
    {
        if (hasBirdInCage)
        {
            bird.PlayAnimationFly(this);
            hasBirdInCage = false;
        }
    }
}
