using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteIsOpacitible : MonoBehaviour
{

    private SpriteRenderer[] sprites;

    private void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();

        SpriteOpacityManager.Instance.AddSprite(this);
    }

    public SpriteRenderer[] GetSprites()
    {
        return sprites;
    }

}
