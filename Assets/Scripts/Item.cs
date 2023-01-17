using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ItemBase, Entity, Interactable
{

    [SerializeField] private Interactable.InteractableType interactableType;
    [SerializeField] private Sprite sprite;

    private void Start()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(this);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(false);
    }

    public bool canAttack()
    {
        return false;
    }

    public bool CanBeAttacked()
    {
        return false;
    }

    public void Damage(int damage, Entity sender)
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(true);
        LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(null);
        Destroy(gameObject);
    }

    public Health GetHealth()
    {
        return null;
    }

    public Interactable.InteractableType GetInteractableType()
    {
        return interactableType;
    }

    public int GetMaxMoveDistance()
    {
        return 0;
    }

    public bool IsEnemy()
    {
        return false;
    }

}
