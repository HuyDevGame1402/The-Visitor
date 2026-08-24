using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public TestSwfAnimation animtionSwf;
    public CapsuleCollider2D capsuleCollider;

    public GameObject parent;
    public GameObject fishingRod_Ver2;
    public GameObject birdAndTree;

    public bool isReady;

    private void OnMouseDown()
    {
        if (isReady)
        {
            birdAndTree.SetActive(false);
            capsuleCollider.enabled = false;
            fishingRod_Ver2.SetActive(true);
            parent.SetActive(false);
        }
        else
        {
            capsuleCollider.enabled = false;
            animtionSwf.PlayTestAnimation(() =>
            {
                capsuleCollider.enabled = true;
            });
        }
    }
}
