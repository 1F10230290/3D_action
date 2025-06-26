using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Image Hpbar;
    public GameObject player;
    
    public float maxHP;
    public float HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        
    }

    // Update is called once per frame
    void Update()
    {
        Hpbar.fillAmount = HP/maxHP;  
        
        //倒した時の処理
        if(HP <= 0){
            Debug.Log("倒した");
        } 
    }

    //プレイヤーに触れた時の処理
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Player")
        {
        }
    }
}
