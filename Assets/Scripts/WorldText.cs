using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldText : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;

    private float timer;
    private float timerMax;

    public void Setup(string text, float lifetime)
    {
        this.text.text = text;
        timerMax = lifetime;
        timer = 0;

        GetComponent<Canvas>().sortingOrder = 20;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timerMax - 1f)
        {
            Color textColor = text.color;
            textColor.a -= Time.deltaTime;
            text.color = textColor;
        }
        if(timer >= timerMax)
        {
            Destroy(gameObject);
        }

        transform.position += new Vector3(0, Time.deltaTime * 1.5f);
    }

}
