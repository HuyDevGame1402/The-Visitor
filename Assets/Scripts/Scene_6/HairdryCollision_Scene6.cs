using UnityEngine;

public class HairdryCollision_Scene6 : MonoBehaviour
{
    public BackgroundScene_6 bg;
    public HairdryAnimation_Scene6 hairdry;
    private void OnMouseDown()
    {
        hairdry.PlayAnimationPullOutDryer();
        bg.PlayAnimationBreakVer1();
        gameObject.SetActive(false);
    }
}
