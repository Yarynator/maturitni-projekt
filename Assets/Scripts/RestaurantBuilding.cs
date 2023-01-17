using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantBuilding : MonoBehaviour, Entity, Interactable
{

    private void Start()
    {
        LevelGrid.Instance.GetGridObject(LevelGrid.Instance.GetGridPosition(transform.position)).SetEntity(this);
    }

    public bool canAttack()
    {
        return false;
    }

    public void Damage(int damage, Entity sender)
    {

    }

    public Health GetHealth()
    {
        return null;
    }

    public Interactable.InteractableType GetInteractableType()
    {
        return Interactable.InteractableType.RestaurantBuilding;
    }

    public int GetMaxMoveDistance()
    {
        return 0;
    }

    public bool IsEnemy()
    {
        return false;
    }

    public bool CanBeAttacked()
    {
        return false;
    }
}
