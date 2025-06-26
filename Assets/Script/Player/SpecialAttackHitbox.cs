using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackHitbox : MonoBehaviour
{
    public float damage = 50f;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }       
    }
}
