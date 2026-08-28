using UnityEngine;

public class WaterTapLock : MonoBehaviour
{
    public bool isReady;
    public Background_Scene4 bg;

    private void OnMouseDown()
    {
        if (isReady)
        {
            isReady = false;
            bg.PlayAnimationWater();
        }
    }
}
