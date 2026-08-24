using UnityEngine;

public class CollisionAttackCat : MonoBehaviour
{
    public Cat_Scene2 cat;
    public Visitor_Scene2 visitor;
    public CapsuleCollider2D capsuleCollider;


    private void OnMouseDown()
    {
        visitor.PlayAnimationHideVisitor();
        cat.PlayAnimationBum();
        capsuleCollider.enabled = false;
    }
}
