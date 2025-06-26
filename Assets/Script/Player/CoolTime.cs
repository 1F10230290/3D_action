using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : MonoBehaviour
{
    public float cooldownTime_1 = 5f;
    private float cooldownTimer_1 = 0f;
    private bool isCooldown_1 = false;
    public float cooldownTime_2 = 5f;
    private float cooldownTimer_2 = 0f;
    private bool isCooldown_2 = false;
    public Image Attack_1_Image;
    public Image Attack_2_Image;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Attack_1のクールダウン
        if(isCooldown_1)
        {
            cooldownTimer_1 -= Time.deltaTime;
            Attack_1_Image.fillAmount = 1 - (cooldownTimer_1 / cooldownTime_1);//徐々にUIを変更

            if(cooldownTimer_1 <= 0)
            {
                isCooldown_1 = false;
                Attack_1_Image.color = Color.white;//色を戻す
                player.isAttack_1 = false;
            }
        }

        if(player.isAttack_1 && !isCooldown_1)
        {
            StartCooldown_1();
        }

        void StartCooldown_1()
        {
            isCooldown_1 = true;
            cooldownTimer_1 = cooldownTime_1;
            Attack_1_Image.color = Color.gray;
        }

        //Attack_2のクールダウン
        if(isCooldown_2)
        {
            cooldownTimer_2 -= Time.deltaTime;
            Attack_2_Image.fillAmount = 1 - (cooldownTimer_2 / cooldownTime_2);//徐々にUIを変更

            if(cooldownTimer_2 <= 0)
            {
                isCooldown_2 = false;
                Attack_2_Image.color = Color.white;//色を戻す
                player.isAttack_2 = false;
            }
        }

        if(player.isAttack_2 && !isCooldown_2)
        {
            StartCooldown_2();
        }

        void StartCooldown_2()
        {
            isCooldown_2 = true;
            cooldownTimer_2 = cooldownTime_2;
            Attack_2_Image.color = Color.gray;
        }
    }
}
