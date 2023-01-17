using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOpacityManager : MonoBehaviour
{

    public static SpriteOpacityManager Instance { get; private set;}


    private List<SpriteIsOpacitible> spriteOpacitibleList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SpriteOpacityManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        spriteOpacitibleList = new List<SpriteIsOpacitible>();
    }

    private void Update()
    {
        Collider2D[] colliderArray = Physics2D.OverlapBoxAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0);

        foreach(SpriteIsOpacitible spriteIsOpacitible in spriteOpacitibleList)
        {
            foreach(SpriteRenderer sprite in spriteIsOpacitible.GetSprites())
            {
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
            }
        }

        foreach(Collider2D collider in colliderArray)
        {
            if(collider.TryGetComponent<SpriteIsOpacitible>(out SpriteIsOpacitible spriteIsOpacitible))
            {
                foreach(SpriteRenderer sprite in spriteIsOpacitible.GetSprites())
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, .5f);
                }
            }
        }
    }

    public void AddSprite(SpriteIsOpacitible spriteIsOpacitible)
    {
        spriteOpacitibleList.Add(spriteIsOpacitible);
    }

}
