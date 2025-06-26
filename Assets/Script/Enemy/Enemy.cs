using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Player player;
    public float HP = 1.0f;
    public float maxHP;
    public Image HPBar;
    public TargetSystem targetSystem;

    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;

        if (HPBar != null)
            HPBar.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation;
        }

    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        HP = Mathf.Max(HP, 0);

        if (HPBar != null)
        {
            HPBar.fillAmount = HP / maxHP;
        }
        if (HP <= 0)
        {
            Die();
            targetSystem.CancelTarget();
        }
    }

    void Die()
    {
        // 敵が死んだ時の処理
        Destroy(gameObject);
        GameObject[] warnings = GameObject.FindGameObjectsWithTag("Warning");
        FindObjectOfType<SpecialAttackGauge>().AddGauge(100f);
        foreach (GameObject warning in warnings)
        {
            Destroy(warning);
        }
        HPBar.gameObject.SetActive(false);
    }

    public float GetHPPercent()
    {
        return Mathf.Clamp01(HP);
    }

}
