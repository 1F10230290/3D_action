using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //キャラの基本機能
    public float HP = 1.0f;
    private float maxHP;
    public Image HPBar;
    public float moveSpeed = 5f;//通常の移動速度
    public float dashSpeed = 7.5f;//ダッシュ時の移動速度
    public float Max_SP = 1f;//Maxのスタミナポイント
    public float DashBarRate = 1f;
    public float currentStamina;//現在のスタミナポイント
    public Rigidbody rb;
    public GameObject hantei;
    public Enemy targetEnemy;
    private Enemy contactEnemy;
    public Animator Anim;
    public TMP_Text Enemy_Damage_text;
    public TMP_Text Player_Damage_text;
    public bool isAttack_1 = false;//Attack1をしているかのフラグ
    public bool isAttack_2 = false;//Attack2をしているかのフラグ
    public Image DashBar;//SPのバー
    public float dashRecoverRate = 1f;//SPの回復レート；
    [SerializeField] public ParticleSystem particle;
    public float staminaRecoveryDelay = 1.5f;
    private float staminaDepletedTime = -1f;

    private bool canAttack;//攻撃可能か認識するフラグ
    private float damageTextStartTime = -1f;//ダメージテキストの表示時間を数える

    private bool isDashing;//今ダッシュしているかどうか
    private bool canDash = true;//今ダッシュができるかどうか


    // Start is called before the first frame update
    void Start()
    {
        maxHP = HP;

        if (HPBar != null)
        {
            HPBar.fillAmount = 1f;
        }
        canAttack = false;
        currentStamina = Max_SP;
    }

    // Update is called once per frame
    void Update()
    {

        // ダメージ表記がされていて、1秒以上経過したら消す
        if (damageTextStartTime > 0 && Time.time - damageTextStartTime >= 1f)
        {
            Enemy_Damage_text.text = "";
            Player_Damage_text.text = "";
            damageTextStartTime = -1f; // 無効化
        }

        //プレイヤーの動き
        Move();

        //攻撃
        Attack();

        if (!isDashing && currentStamina < Max_SP)//ダッシュしていないかつ、SPがMaxじゃない時はSPが回復する
        {
            if (currentStamina <= 0)
            {
                if (staminaDepletedTime < 0f)
                {
                    staminaDepletedTime = Time.time;
                }

                if (Time.time - staminaDepletedTime >= staminaRecoveryDelay)
                {
                    currentStamina += dashRecoverRate * Time.deltaTime;
                }
            }
            else
            {
                currentStamina += dashRecoverRate * Time.deltaTime;
                staminaDepletedTime = -1;//リセット
            }


            currentStamina = Mathf.Min(currentStamina, Max_SP);
            if (currentStamina >= Max_SP)
            {
                canDash = true;
            }
        }
        DashBar.fillAmount = currentStamina / Max_SP;
    }


    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            contactEnemy = collision.gameObject.GetComponent<Enemy>();
            canAttack = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            contactEnemy = null;
            canAttack = false; // Enemyから離れたら攻撃できなくする
        }
    }

    void NormalAttack()
    {
        if (contactEnemy == null) return;

        float damage = Random.Range(0.1f, 0.15f);
        contactEnemy.TakeDamage(damage);
        Debug.Log("敵に弱攻撃！");
        damage *= 100;
        int displayDamage = Mathf.RoundToInt(damage); // 整数に変換
        ShowDamageText("<color=#ffd700>-" + displayDamage.ToString());
    }

    void StrongerAttack()
    {
        if (contactEnemy == null) return;

        float damage = Random.Range(0.2f, 0.25f);
        contactEnemy.TakeDamage(damage);
        Debug.Log("敵に強攻撃！");
        damage *= 100;
        int displayDamage = Mathf.RoundToInt(damage); // 整数に変換
        ShowDamageText("<color=#ff0000>-" + displayDamage.ToString());
    }

    void ShowDamageText(string damageValue)
    {
        Enemy_Damage_text.text = damageValue;

        // 表示位置をランダムに調整
        float randomX = Random.Range(-50f, 50f); // X方向にランダムなオフセット
        float randomY = Random.Range(-50f, 50f); // Y方向にランダムなオフセット
        Enemy_Damage_text.rectTransform.anchoredPosition = new Vector2(randomX, randomY);

        // 現在の時間を記録（1秒後に消すため）
        damageTextStartTime = Time.time;
    }

    void ShowDamageText_Player(string damageValue)
    {
        Player_Damage_text.text = damageValue;

        // 表示位置をランダムに調整
        float randomX = Random.Range(-50f, 50f); // X方向にランダムなオフセット
        float randomY = Random.Range(-50f, 50f); // Y方向にランダムなオフセット
        Player_Damage_text.rectTransform.anchoredPosition = new Vector2(randomX, randomY);

        // 現在の時間を記録（1秒後に消すため）
        damageTextStartTime = Time.time;
    }

    IEnumerator HideDamageText()
    {
        yield return new WaitForSeconds(1f);
        Enemy_Damage_text.text = "";
        Player_Damage_text.text = "";
    }

    //回避から高速移動に変更
    /*void Avoid(float moveX, float moveZ)
    {
        Vector3 avoidDirection = Vector3.zero;
        if (moveX != 0)
        {
            avoidDirection = (moveX > 0) ? transform.right : -transform.right;
        }
        else
        {
        if (moveZ == 0)
        {
            moveZ = 1f;
        }
        avoidDirection = (moveZ >= 0) ? transform.forward : -transform.forward;
        }
        
        Vector3 targetPosition = transform.position + avoidDirection*avoidDistance;
        
        RaycastHit hit;
        if(Physics.Raycast(hantei.transform.position, avoidDirection, out hit, avoidDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                targetPosition = hit.point - avoidDirection*avoidSafwMargin;
            }
        }
        rb.MovePosition(targetPosition);
        Debug.Log("回避");
    }*/

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        move = move.normalized;

        float currentSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0 && currentStamina > 0 && canDash)
        {
            isDashing = true;
            currentStamina -= Time.deltaTime * DashBarRate;
            currentStamina = Mathf.Max(currentStamina, 0);

            if (currentStamina <= 0)
            {
                canDash = false;
            }
        }
        else
        {
            isDashing = false;
        }

        if (isDashing)
        {
            currentSpeed = dashSpeed;
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }

        else
        {
            if (particle.isPlaying)
            {
                particle.Stop();
            }
        }


        if (move.magnitude > 0)
        {
            rb.MovePosition(rb.position + move * currentSpeed * Time.deltaTime);
            Anim.SetBool("isWalking", true); // isWalking を true にする
        }
        else
        {
            Anim.SetBool("isWalking", false); // 動いていないときは false にする
        }

        transform.Rotate(0, Input.GetAxis("Mouse X") * 3f, 0);

        // アニメーション速度を状態に応じて変更
        if (isDashing)
        {
            Anim.speed = 2.5f; // Dashアニメーションの再生速度
        }

        else if (move.magnitude > 0)
        {
            Anim.speed = 2f;   // Walkアニメーションの再生速度
        }

    }



    void Attack()
    {
        // Eボタンを押したとき、通常攻撃
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isAttack_1)
            {
                Anim.Play("Armature|Action_1");
                Anim.speed = 2f; // 攻撃速度を速くする
                isAttack_1 = true;
                if (canAttack)
                {
                    NormalAttack();
                }
            }
        }

        // Rボタンを押したとき、強攻撃
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isAttack_2)
            {
                Anim.Play("Armature|Action_2");
                Anim.speed = 2f; // 攻撃速度を速くする
                isAttack_2 = true;
                if (canAttack)
                {
                    StrongerAttack();
                }
            }
        }

        // 攻撃が終了したらIdleアニメーションに戻る
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Action_1") || Anim.GetCurrentAnimatorStateInfo(0).IsName("Armature|Action_2"))
        {
            if (Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) // アニメーションが終了したかを確認
            {
                Anim.Play("Armature|Idle");
                Anim.speed = 1f; // 通常のアニメーション速度に戻す
            }
        }
    }

    //ダメージを受ける
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "EnemyAttack")
        {
            float damage = Random.Range(0.1f, 0.15f);
            PlayerTakeDamage(damage);
            Debug.Log("プレイヤーに攻撃！");
            damage *= 100;
            int displayDamage = Mathf.RoundToInt(damage); // 整数に変換
            ShowDamageText_Player("<color=#191970>-" + displayDamage.ToString());
        }
    }

    public void PlayerTakeDamage(float damage)
    {
        HP -= damage;
        HP = Mathf.Max(HP, 0);

        if (HPBar != null)
        {
            HPBar.fillAmount = HP / maxHP;
        }
        if (HP <= 0)
        {
            PlayerDie();
        }
    }

    public void PlayerDie()
    {
        // プレイヤーが死んだ時の処理
        Debug.Log("Die");
    }

}