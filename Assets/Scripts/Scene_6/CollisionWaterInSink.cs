using UnityEngine;

public class CollisionWaterInSink : MonoBehaviour
{
    public HairdryAnimation_Scene6 hairdry;

    private void OnMouseDown()
    {
        hairdry.PlayAnimationDryerInSink();
        gameObject.SetActive(false);
    }
}
