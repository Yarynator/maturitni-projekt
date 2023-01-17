using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{

    [SerializeField] private int offset = 0;
    [SerializeField] private bool destroyOnStart = true;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        float precisionMultiplier = 2f;
        spriteRenderer.sortingOrder = -(int)(transform.position.y * precisionMultiplier) + (int)(offset * precisionMultiplier);

        if(destroyOnStart)
            Destroy(this);
    }

}
