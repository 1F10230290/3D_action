using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_Stamp : EnemyAttackBase
{
    public GameObject warningPrefab;
    public GameObject attackPrefab;
    public float stampSpeed = 10f;
    public float warningTine = 1.5f; //予告から攻撃までの時間
    public float cooldownTime = 3f;
    private bool isAttacking = false;
    private float attackOffetY = 10f;

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

        Vector3 targetPosition = player.position;
        Vector3 warningPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        GameObject warning = Instantiate(warningPrefab, warningPosition, Quaternion.identity);
        warning.transform.localScale = new Vector3(4f, 1f, 4f);

        Debug.Log(warningPosition);
        yield return new WaitForSeconds(warningTine);
        Destroy(warning);

        Vector3 attackSpawnPosition = new Vector3(targetPosition.x, targetPosition.y + attackOffetY, targetPosition.z);
        GameObject attackObj = Instantiate(attackPrefab, attackSpawnPosition, Quaternion.identity);
       Debug.Log(attackSpawnPosition);
        Rigidbody rb = attackObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.down * stampSpeed;
        }

        yield return new WaitForSeconds(cooldownTime); //クールタイム
        isAttacking = false;
    }

    bool PlayerIsInRange(Transform player)
    {
        return Vector3.Distance(transform.position, player.position) < 30f;
    }

}
