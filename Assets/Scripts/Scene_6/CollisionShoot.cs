using UnityEngine;

public class CollisionShoot : MonoBehaviour
{
    public SinkAnimation_Scene6 sink;

    private void OnMouseDown()
    {
        sink.PlayAnimationGunGone();
        gameObject.SetActive(false);
    }
}
