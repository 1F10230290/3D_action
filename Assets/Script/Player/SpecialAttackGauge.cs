using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackGauge : MonoBehaviour
{
    public float gauge = 0f;
    public float maxGauge = 300f;
    public Image gaugeBar1;
    public Image gaugeBar2;
    public Image gaugeBar3;

    void Start()
    {
        gauge = 0f;
        UpdateGaugeUI();
    }

    public void AddGauge(float amount)
    {
        gauge = Mathf.Min(gauge + amount, maxGauge);
        UpdateGaugeUI();
    }

    public void UseGauge(float amount)
    {
        gauge = Mathf.Max(gauge - amount, 0f);
        UpdateGaugeUI();
    }

    void UpdateGaugeUI()
    {
        // 第1段階 (0〜100)
        float g1 = Mathf.Clamp(gauge, 0, 100);
        gaugeBar1.fillAmount = g1 / 100f;

        // 第2段階 (100〜200)
        float g2 = Mathf.Clamp(gauge - 100, 0, 100);
        gaugeBar2.fillAmount = g2 / 100f;

        // 第3段階 (200〜300)
        float g3 = Mathf.Clamp(gauge - 200, 0, 100);
        gaugeBar3.fillAmount = g3 / 100f;
    }

    public int GetSkillLevel()
    {
        if (gauge >= 300) return 3;
        else if (gauge >= 200) return 2;
        else if (gauge >= 100) return 1;
        else return 0;
    }
}
