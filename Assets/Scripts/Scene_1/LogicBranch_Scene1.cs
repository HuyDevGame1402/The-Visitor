using UnityEngine;

public class LogicBranch_Scene1 : MonoBehaviour
{
    public TestSwfAnimation testSwfAnimation;
    public GameObject branchWater;
    public GameObject treeTrunkHole;
    public float timeOffset;

    private void OnMouseDown()
    {
        treeTrunkHole.SetActive(true);
        testSwfAnimation.PlayTestAnimation(() =>
        {
            branchWater.gameObject.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }, timeOffset);       
    }
}
