
public class ValveYellow : Valve
{
    protected override void OnMouseDown()
    {
        if (visitor.isReady == false) return;
        if(visitor.indexValve == 1)
        {
            visitor.indexValve = 4;
            animationScene3.PlayAnimationJumpValve_1_4();
        }
        else if(visitor.indexValve == 4)
        {
            visitor.indexValve = 1;
            animationScene3.PlayAnimationJumpValve_4_1();
        }
        else if (visitor.indexValve == 3)
        {
            visitor.indexValve = 1;
            animationScene3.PlayAnimationJumpValve_3_1();
        }
        else if (visitor.indexValve == 2)
        {
            visitor.indexValve = 5;
            animationScene3.PlayAnimationJumpValve_2_5();
        }
    }
}
