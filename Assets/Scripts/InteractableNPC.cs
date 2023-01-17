using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableNPC : MonoBehaviour, Entity, Interactable
{

    [SerializeField] private Interactable.InteractableType interactableQuest;
    [SerializeField] private QuestSO quest;

    private void Start()
    {
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(this);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(false);
    }
    
    public bool IsEnemy()
    {
        return false;
    }

    public bool canAttack()
    {
        return false;
    }

    public void Damage(int damage, Entity sender)
    {

    }

    public int GetMaxMoveDistance()
    {
        return 7;
    }

    public Health GetHealth()
    {
        return null;
    }

    public Interactable.InteractableType GetInteractableType()
    {
        return interactableQuest;
    }

    public QuestSO GetQuest()
    {
        return quest;
    }

    public bool CanBeAttacked()
    {
        return false;
    }
}
