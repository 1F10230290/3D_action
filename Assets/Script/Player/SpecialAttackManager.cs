using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackManager : MonoBehaviour
{
    public SpecialAttackGauge gauge;
    public Player player;
    public GameObject[] skillEffects; //技ごとのprefab(0:弱、1:中、2:強)


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            int level = gauge.GetSkillLevel();
            if (level > 0)
            {
                ActivateSkill(level);
            }
        }
    }

    void ActivateSkill(int level)
    {
        Vector3 spawnPos = player.transform.position + player.transform.forward * 2f;
        Quaternion spawnRot = Quaternion.LookRotation(player.transform.position);

        Instantiate(skillEffects[level - 1], spawnPos, spawnRot);

        float[] costs = { 100f, 200f, 300f };
        gauge.UseGauge(costs[level - 1]);
    }
}
