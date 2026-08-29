using UnityEngine;

public class CollisionMouth : MonoBehaviour
{
    public BedsAnimation_Scene5 bed;

    private void OnMouseDown()
    {
        bed.PlayAnimationMouth();
        gameObject.SetActive(false);
    }
}
