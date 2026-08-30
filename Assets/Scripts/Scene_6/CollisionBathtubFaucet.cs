using UnityEngine;

public class CollisionBathtubFaucet : MonoBehaviour
{
    public TubAnimation_Scene6 tub;

    private void OnMouseDown()
    {
        tub.PlayAnimationFill();
        gameObject.SetActive(false);    
    }
}
