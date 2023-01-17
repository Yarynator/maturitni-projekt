using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{

    public abstract ActionType GetActionType();

    public abstract string GetName();

    public abstract GridSystemVisual.GridVisualType GetGridVisualType();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete, bool isInit = false);

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract bool IsValidActionGridPosition(GridPosition gridPosition);

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach(GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            if (enemyAIAction != null)
            {
                enemyAIActionList.Add(enemyAIAction);
            }
        }

        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }

        return null;
    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

}

public enum ActionType
{
    Move,
    Attack,
    Magic,
    Interact
}