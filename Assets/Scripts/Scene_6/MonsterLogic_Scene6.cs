using System.Collections;
using UnityEngine;

public class MonsterLogic_Scene6 : MonoBehaviour
{
    public float timeDelay = 2f;
    public bool isSetup;
    public bool isCloseDoor;
    public MonsterAnimation_Scene6 monster;
    public BackgroundScene_6 bg;
    public GameObject collisionDoor;

    private void Start()
    {
        StartCoroutine(CoroutineStart());
        //monster.PlayAnimationAttackHuman();
    }

    private IEnumerator CoroutineStart()
    {
        yield return new WaitForSeconds(timeDelay);
        collisionDoor.SetActive(false);
        if (isCloseDoor && isSetup == false)
        {
            bg.PlayAnimationBreak();
        }
        // lao vao an player luon
        if(isCloseDoor == false)
        {
            monster.PlayAnimationAttackHuman();
        }
        // đập cửa
        if(isSetup && isCloseDoor)
        {
            bg.PlayAnimationBanging();
        }
    }
}
