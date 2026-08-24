using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Scene1 : MonoBehaviour
{

    public TestSwfAnimation animationSwf;

    public GameObject birdAnimationController;

    private void Start()
    {
        animationSwf.PlayTestAnimation(() =>
        {
            birdAnimationController.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
