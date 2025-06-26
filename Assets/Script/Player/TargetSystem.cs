using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetSystem : MonoBehaviour
{
    public Image targetHpBar; // UIのHPバー
    public GameObject playerObject;
    public TMP_Text text;
    public GameObject arrowPrefab;
    private GameObject activateArrow;
    public float arrowDisplayTime = 1.0f;
    private float arrowTimer = 0f;
    private Enemy currentTarget; // ターゲット中の敵

    void Start()
    {
        if (targetHpBar != null)
        {
            targetHpBar.gameObject.SetActive(false); // 最初は非表示
            text.gameObject.SetActive(false);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currentTarget == null)
            {
                // ターゲット取得処理
                Enemy nearestEnemy = FindNearestEnemy();
                if (nearestEnemy != null)
                {
                    currentTarget = nearestEnemy;
                    targetHpBar.fillAmount = currentTarget.HP;
                    targetHpBar.gameObject.SetActive(true);
                    text.gameObject.SetActive(true);

                    //矢印生成
                    if (arrowPrefab != null)
                    {
                        Vector3 arrpowPos = currentTarget.transform.position + Vector3.up * 2.5f;
                        activateArrow = Instantiate(arrowPrefab, arrpowPos, Quaternion.identity);
                        activateArrow.transform.SetParent(currentTarget.transform);
                        arrowTimer = arrowDisplayTime;
                    }
                }
            }
            else
            {
                // ターゲット解除
                CancelTarget();
            }
        }

        if (currentTarget != null)
        {
            // HPバー更新
            float hpPercent = currentTarget.GetHPPercent();
            targetHpBar.fillAmount = hpPercent;

            if (playerObject != null)
            {
                Vector3 targetDir = currentTarget.transform.position - playerObject.transform.position;
                targetDir.y = 0; //上方向の回転を防ぐ

                if (targetDir != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                    playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, targetRotation, Time.deltaTime * 5f);//5fは回転スピード
                }
            }

            //矢印の時間管理
            if (activateArrow != null)
            {
                arrowTimer -= Time.deltaTime;
                if (arrowTimer <= 0f)
                {
                    Destroy(activateArrow);
                }
            }
        }
    }

    public void CancelTarget()
    {
        currentTarget = null;
        targetHpBar.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

        if (activateArrow != null)
        {
            Destroy(activateArrow);
        }
    }

    Enemy FindNearestEnemy()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        Enemy nearest = null;
        float minDist = float.MaxValue;
        Vector3 playerPos = playerObject.transform.position;

        foreach (Enemy enemy in allEnemies)
        {
            float dist = Vector3.Distance(playerPos, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }


}
