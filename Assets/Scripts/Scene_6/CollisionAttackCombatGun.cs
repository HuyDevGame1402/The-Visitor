using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAttackCombatGun : MonoBehaviour
{
    public GunAnimation_Scene6 gun;
    public MonsterAnimation_Scene6 monster;
    public float timeDelay = 0.2f;

    private void OnMouseDown()
    {
        if (gun.isReadyShoot && monster.isReadyHitDamage)
        {
            gun.PlayAnimationHitGun();
            StartCoroutine(CoroutineDelayHitMonster());
        }
    }

    private IEnumerator CoroutineDelayHitMonster()
    {
        yield return new WaitForSeconds(timeDelay);
        monster.PlayAnimationHitDamage();
        gameObject.SetActive(false);
    }
}
