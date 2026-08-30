using UnityEngine;

public class CollisionOutlet : MonoBehaviour
{
    public HairdryAnimation_Scene6 hairdry;
    private void OnMouseDown()
    {
        hairdry.PlayAnimationDryerPluggedIn();
        gameObject.SetActive(false);
    }
}
