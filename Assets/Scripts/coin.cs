using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anime;

    private float destoryTime = 30f;

    void Start()
    {
        Destroy(gameObject, destoryTime);
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //触发被收集动画
            OnAnimationEnd();
        }
    }

    private void OnAnimationEnd()
    {
        Debug.Log("coinEat");
        anime.Play("coinEat");
        float animationDuration = 0.47f;
        // 当动画播放完成，销毁GameObject  
        Destroy(gameObject, animationDuration);
    }
}
