using UnityEngine;

public class KnifeAnimationVisualMove : MonoBehaviour
{
    public Animator animator;

    public KnifeAnimation_Scene4 knife;

    public void PlayAnimation()
    {
        animator.SetTrigger("MoveToInit");
    }

    public void TriggerEventAnimation()
    {
        knife.PlayAnimationIdle();
        knife.gameObject.SetActive(true);
    }
}
