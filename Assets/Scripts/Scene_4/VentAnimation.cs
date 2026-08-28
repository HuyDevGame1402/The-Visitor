using UnityEngine;

public class VentAnimation : MonoBehaviour
{
    public TestSwfAnimation animationSwf;
    public bool isReady;
    public Lady_Scene4 lady;
    public VisitorAnimation_Scene4 visitor;
    public bool isBreak;

    private void OnMouseDown()
    {
        if(visitor.isInFridge && isBreak)
        {
            // chuyển sang scene 5
        }
        if (lady.isDie && isBreak == false)
        {
            PlayAnimationBreak();
        }
    }

    public void PlayAnimationBreak()
    {
        animationSwf.sequenceName = "S_Break";
        animationSwf.PlayTestAnimation(() =>
        {
            isReady = true;
            isBreak = true;
        });
    }
}
