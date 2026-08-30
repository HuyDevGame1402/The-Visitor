using UnityEngine;

public class CollisionWaterDischargeValve : MonoBehaviour
{
    public ToiletAnimation_Scene6 toilet;

    private void OnMouseDown()
    {
        toilet.PlayAnimationOverflow();
        gameObject.SetActive(false);
    }
}
