using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttackBase : MonoBehaviour
{
    public abstract void TryAttack(Transform player);
}
