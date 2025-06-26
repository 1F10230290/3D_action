using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGrower : MonoBehaviour
{
    public float growDuration = 0.5f; //どのくらいの時間で伸びきるか
    public float maxLength = 5f;  //z方向に最終的にどれくらい伸びるか

    private Vector3 initialScale;  //初期のスケール
    private Vector3 targetScale;  //目標スケール

    void Start()
    {
        initialScale = transform.localScale;  //初期化
        targetScale = new Vector3(initialScale.x, initialScale.y, maxLength); //z方向だけmaxLengthに変えたスケールを目標とする
        StartCoroutine(GrowOverTime());  //徐々にスケールを変えるコルーチン開始

    }

    IEnumerator GrowOverTime()
    {
        float time = 0f;

        while (time < growDuration)
        {
            float t = time / growDuration;
            float currentLength = Mathf.Lerp(0.1f, maxLength, t);
            transform.localScale = new Vector3(initialScale.x, initialScale.y, currentLength);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        DestroyAttack();
    }


    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            DestroyAttack();
        }
    }

    public void DestroyAttack()
    {
        Destroy(gameObject);
    }

}
