using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTreeScene2 : MonoBehaviour
{
    public Visitor_Scene2 visitor;
    public TreeAnimationScene2 tree;
    public Cat_Scene2 cat;
    public GameObject cavity;

    private void Start()
    {
        cavity.gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (visitor.isToFaceCat && visitor.isReadyAttackTree)
        {
            cavity.gameObject.SetActive(false);
            // hiệu ứng cây
            tree.PlayAnimationPull();

            // visitor đi vào hầm
            visitor.AttackBranchTree();
            cat.PlayAnimationHub();

            gameObject.SetActive(false);
        }
    }
}
