using UnityEngine;

public class CollisionToilet : MonoBehaviour
{
    public ToiletAnimation_Scene6 toilet;

    private void OnMouseDown()
    {
        toilet.PlayAnimationDunkTP();
        gameObject.SetActive(false);
    }
}
