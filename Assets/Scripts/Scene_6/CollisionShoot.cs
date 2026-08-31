using UnityEngine;

public class CollisionShoot : MonoBehaviour
{
    public SinkAnimation_Scene6 sink;
    public GunAnimation_Scene6 gun;
    public MonsterAnimation_Scene6 monster;

    private void OnMouseDown()
    {
        sink.PlayAnimationGunGone();
        gun.PlayAnimationGetGun();
        monster.PlayAnimationRecover();
        gameObject.SetActive(false);
    }
}
