using System.Collections;
using UnityEngine;

public class Background_Scene5 : MonoBehaviour
{
    public TestSwfAnimation animationSwf;

    public GameObject wall;
    public float timeDelay;

    public bool isReady;

    private readonly string[] _sequences = new string[]
    {
        "S_Idle",
        "S_Fall",
        "S_Empty",
    };

    private void Start()
    {
        StartCoroutine(CoroutineStartScene5());
    }

    public void PlayAnimationFall()
    {
        wall.gameObject.SetActive(false);
        animationSwf.sequenceName = _sequences[1];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation(() =>
        {
            isReady = true;
        });
        //StartCoroutine(CoroutineShowWall());
    }

    private IEnumerator CoroutineStartScene5()
    {
        yield return new WaitForSeconds(timeDelay);
        PlayAnimationFall();
    }

    public void PlayAnimationEmpty()
    {
        animationSwf.sequenceName = _sequences[2];
        animationSwf.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
        animationSwf.PlayTestAnimation();
    }

    private IEnumerator CoroutineShowWall()
    {
        yield return new WaitForSeconds(timeDelay);
        wall.gameObject.SetActive(true);
    }
}
