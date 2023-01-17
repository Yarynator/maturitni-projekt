using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public static Bullet Create(Vector2 position)
    {
        return Instantiate(Resources.Load<Transform>("FireballBullet"), position, Quaternion.identity).GetComponent<Bullet>();
    }


    [SerializeField] private Transform bulletHead;
    [SerializeField] private Transform bulletTrail;

    private bool useTimer;
    private float timer;
    private float timerMax;

    private void Update()
    {
        if (useTimer)
        {
            timer += Time.deltaTime;
            if(timer >= timerMax)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Complete()
    {
        bulletHead.gameObject.SetActive(false);
        timer = 0;
        timerMax = 1f;
        useTimer = true;
    }

}
