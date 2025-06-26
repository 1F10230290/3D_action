using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : EnemyAttackBase
{
    public GameObject warningPrefab;
    public GameObject attackPrefab;
    public Transform firePoint;  //発射口
    public float warningTine = 1.5f; //予告から攻撃までの時間
    public float cooldownTime = 3f;

    private bool isAttacking = false;

    public override void TryAttack(Transform player)
    {
        if (!isAttacking && player != null && PlayerIsInRange(player))
        {
            StartCoroutine(AttackRoutine(player));
        }     
    }
    IEnumerator AttackRoutine(Transform player)
    {
        isAttacking = true;

        Vector3 direction = (player.position - firePoint.position).normalized;
        direction.y = 0;
        Quaternion fixedRotation = Quaternion.LookRotation(direction);


        GameObject warning = Instantiate(warningPrefab, firePoint.position, fixedRotation);
        warning.transform.localScale = new Vector3(1f, 1f, 5f);

        yield return new WaitForSeconds(warningTine);
        Destroy(warning);

        Instantiate(attackPrefab, firePoint.position, fixedRotation);

        yield return new WaitForSeconds(cooldownTime); //クールタイム
        isAttacking = false;
    }

    bool PlayerIsInRange(Transform player)
    {
        return Vector3.Distance(transform.position, player.position) < 10f;
    }

}
