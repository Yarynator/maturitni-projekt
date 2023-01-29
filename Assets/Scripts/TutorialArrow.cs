using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{

    private Vector3 startPosition;
    private Vector2 startRectPosition;
    private bool dirForward;
    [SerializeField] private bool isCanvas = false;

    private void Start()
    {
        if (isCanvas)
        {
            startRectPosition = GetComponent<RectTransform>().anchoredPosition;
        }
        else
        {
            startPosition = transform.position;
        }

        dirForward = true;
    }

    private void Update()
    {

        float angle = transform.rotation.eulerAngles.z;
        Vector3 lDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));

        if (!isCanvas)
        {
            if (dirForward)
            {
                transform.position += lDirection * Time.deltaTime * .5f;

                if (Vector3.Distance(startPosition, transform.position) >= .5f)
                {
                    dirForward = false;
                }
            }
            else
            {
                transform.position -= lDirection * Time.deltaTime * .5f;
                if (Vector3.Distance(startPosition, transform.position) <= .1f)
                {
                    dirForward = true;
                }
            }
        }
        else
        {
            RectTransform tran = GetComponent<RectTransform>();
            if (dirForward)
            {
                tran.anchoredPosition += new Vector2(lDirection.x, lDirection.y) * Time.deltaTime * 50f;

                if (Vector2.Distance(startRectPosition, tran.anchoredPosition) >= 100f)
                {
                    dirForward = false;
                }
            }
            else
            {
                tran.anchoredPosition -= new Vector2(lDirection.x, lDirection.y) * Time.deltaTime * 50f;
                if (Vector3.Distance(startRectPosition, tran.anchoredPosition) <= 5f)
                {
                    dirForward = true;
                }
            }
        }
    }

}
