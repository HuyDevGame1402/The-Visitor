using UnityEngine;

public class CollisionPeopleDown : MonoBehaviour
{
    public BedsAnimation_Scene5 bed;
    private void OnMouseDown()
    {
        bed.PlayAnimationClaw();
        gameObject.SetActive(false);
    }
}
