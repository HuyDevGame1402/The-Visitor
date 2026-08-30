using UnityEngine;

public class CollisionWaterValveSink : MonoBehaviour
{
    public SinkAnimation_Scene6 sink;

    private void OnMouseDown()
    {
        sink.PlayAnimationFillSink();
        gameObject.SetActive(false);
    }
}
