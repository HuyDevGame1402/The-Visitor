using UnityEngine;

public class ToiletAnimation_Scene6 : MonoBehaviour
{
    [Header("References")]
    public TestSwfAnimation animationSwf;
    public GameObject collisionToilet, collisionWaterDischargeValve, collisionBathtubFaucet;

    private readonly string[] _sequences = new string[]
    {
        "L_Idle",
        "S_LiftTP",
        "S_DunkTP",
        "S_Overflow",  
    };

    public void PlayAnimationLiftTP()
    {
        animationSwf.sequenceName = _sequences[1];
        animationSwf.PlayTestAnimation(() =>
        {
            collisionToilet.SetActive(true);
        });
    }

    public void PlayAnimationDunkTP()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.PlayTestAnimation(() =>
        {
            collisionWaterDischargeValve.SetActive(true);
        });
    }
    public void PlayAnimationOverflow()
    {
        animationSwf.sequenceName = _sequences[3];
        animationSwf.PlayTestAnimation(() =>
        {
            collisionBathtubFaucet.SetActive(true);
        });
    }

}
