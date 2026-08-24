using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickFishingVer2 : MonoBehaviour
{

    public int index = 1;
    public FishingRod_Ver2 fisgingRodVer2;

    private void OnMouseDown()
    {
        if (fisgingRodVer2.isFishReturn)
        {
            index += 1;
            fisgingRodVer2.PlayAnimationVisitorRod(index);
        }
        else
        {

        }
    }
}
