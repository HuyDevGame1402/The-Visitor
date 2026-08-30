using UnityEngine;

public class CollisionDoorSink : MonoBehaviour
{
    public SinkAnimation_Scene6 sink;

    private void OnMouseDown()
    {
        sink.PlayAnimationOpenCabinet();
        gameObject.SetActive(false);
    }
}
