using UnityEngine;

public class KidAnimation_Scene6 : MonoBehaviour
{
    public TestSwfAnimation animationSwf;

    private readonly string[] _sequences = new string[]
    {
        "L_IdleScared",     // Phím 1
        "S_Explode",       // Phím 2
    };

    public void PlayAnimationExplode()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.PlayTestAnimation();
    }
}
