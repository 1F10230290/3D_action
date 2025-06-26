using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    private EnemyAttackBase attackLogic;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        attackLogic = GetComponent<EnemyAttackBase>();
        player = GameObject.FindWithTag("Player")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackLogic != null && player != null)
        {
            attackLogic.TryAttack(player);
        }
    }
}
