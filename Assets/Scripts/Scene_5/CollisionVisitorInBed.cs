using UnityEngine;

public class CollisionVisitorInBed : MonoBehaviour
{
    public BedsAnimation_Scene5 bed;

    private void OnMouseDown()
    {
        bed.PlayAnimationJumpDown();
        gameObject.SetActive(false);
    }
}
