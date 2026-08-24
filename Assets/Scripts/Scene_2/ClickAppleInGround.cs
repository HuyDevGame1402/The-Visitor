using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAppleInGround : MonoBehaviour
{

    public Visitor_Scene2 visitor;

    private void OnMouseDown()
    {
        if (visitor.isReadyEat)
        {
            visitor.PlayAnimationEatApple();
            gameObject.SetActive(false);
        }
    }

}
