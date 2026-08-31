using System.Collections;
using UnityEngine;

public class CollisionShootAttack : MonoBehaviour
{
    public GunAnimation_Scene6 gun;
    public MonsterAnimation_Scene6 monster;
    public int bulletCount = 3;

    public float timeDelay = 0.2f;
    public GameObject collisionAttackGunCombat;

    private void OnMouseDown()
    {
        if(bulletCount > 0 && gun.isReadyShoot && monster.isReadyHitDamage)
        {
            gun.PlayAnimationShoot(bulletCount);
            monster.PlayAnimationShoot();
            bulletCount -= 1;
        }

        if(bulletCount <= 0 && gun.isReadyShoot && monster.isReadyHitDamage)
        {
            gun.PlayAnimationHitGun();
            StartCoroutine(CoroutineDelayHitMonster());
            bulletCount -= 1;
            if(bulletCount == -1)
            {
                collisionAttackGunCombat.SetActive(true);
            }
        }
    }

    private IEnumerator CoroutineDelayHitMonster()
    {
        yield return new WaitForSeconds(timeDelay);
        monster.PlayAnimationHitDamage();
        gameObject.SetActive(false);
    }
}
