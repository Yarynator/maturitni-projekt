using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushDoor : MonoBehaviour, Entity
{

    [SerializeField] private Sprite leftPartSprite;
    [SerializeField] private Sprite middlePartSprite;
    [SerializeField] private Sprite rightPartSprite;

    [SerializeField] private SpriteRenderer leftPartSpriteRenderer;
    [SerializeField] private SpriteRenderer middlePartSpriteRenderer;
    [SerializeField] private SpriteRenderer rightPartSpriteRenderer;

    private void Start()
    {
        GridObject gridObject = LevelGrid.Instance.GetGridObject(LevelGrid.Instance.GetGridPosition(transform.position));
        gridObject.SetEntity(this);
        GridPosition gridPosition = gridObject.GetGridPosition();
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(false);
    }

    public bool canAttack()
    {
        return false;
    }

    public bool IsEnemy()
    {
        return true;
    }

    public void Damage(int damage, Entity sender)
    {
        leftPartSpriteRenderer.sprite = leftPartSprite;
        middlePartSpriteRenderer.sprite = middlePartSprite;
        rightPartSpriteRenderer.sprite = rightPartSprite;

        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(null);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(true);
        if (sender is Player player)
        {
            ActionManager.Instance.ReloadVisuals(player);
        }
        SceneInfo.Instance.SetTutorialBushIsDestroyed(true);

        Destroy(this);
    }

    public int GetMaxMoveDistance()
    {
        return 0;
    }

    public Health GetHealth()
    {
        return null;
    }

    public bool CanBeAttacked()
    {
        return true;
    }
}
