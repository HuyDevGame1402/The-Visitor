using UnityEngine;

public class CollisionDoor : MonoBehaviour
{
    public MonsterLogic_Scene6 monsterLogic;
    public BackgroundScene_6 bg;

    private void OnMouseDown()
    {
        monsterLogic.isCloseDoor = true;
        bg.PlayAnimationDoorClose();
        gameObject.SetActive(false);
    }
}
