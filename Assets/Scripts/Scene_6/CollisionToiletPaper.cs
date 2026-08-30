using UnityEngine;

public class CollisionToiletPaper : MonoBehaviour
{
    public ToiletAnimation_Scene6 toilet;
    public MonsterLogic_Scene6 monster;

    public MonsterAnimation_Scene6 monsterAnimation;
    public BackgroundScene_6 bg;

    private void OnMouseDown()
    {
        if (bg.isEndGame || monsterAnimation.isEndGame) return;

        toilet.PlayAnimationLiftTP();
        monster.isSetup = true;
        gameObject.SetActive(false);
    }
}
