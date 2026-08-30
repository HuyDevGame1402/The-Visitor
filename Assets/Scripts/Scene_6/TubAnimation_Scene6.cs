using UnityEngine;

public class TubAnimation_Scene6 : MonoBehaviour
{
    public TestSwfAnimation animationSwf;
    public GameObject collisionDoor;

    private readonly string[] _sequences = new string[]
    {
        "S_Idle",
        "S_Fill",
        "S_Blank",
    };

    public void PlayAnimationFill()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.PlayTestAnimation(() =>
        {
            collisionDoor.SetActive(true);
        });
    }
    public void PlayAnimationBlank()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.PlayTestAnimation(() =>
        {

        });
    }
}
