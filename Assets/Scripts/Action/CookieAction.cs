using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieAction : BaseAction
{
    
    private GridPosition startGridPosition;
    private GridPosition endGridPosition;

    private bool isAttacking;

    private Cookie cookie;

    Action onActionComplete;

    private void Update()
    {
        if (isAttacking)
        {
            float cookieSpeed = 7.5f;

            Vector2 dir = (LevelGrid.Instance.GetWorldPosition(endGridPosition) - LevelGrid.Instance.GetWorldPosition(startGridPosition)).normalized;
            dir = dir * Time.deltaTime * cookieSpeed;

            cookie.transform.position += new Vector3(dir.x, dir.y);

            if(Vector2.Distance(cookie.transform.position, LevelGrid.Instance.GetWorldPosition(endGridPosition)) <= .1f)
            {
                Destroy(cookie.gameObject);
                LevelGrid.Instance.GetGridObject(endGridPosition).GetEntity().Damage(1, GetComponent<Entity>());
                onActionComplete();
                isAttacking = false;
            }
        }
    }

    public override GridSystemVisual.GridVisualType GetGridVisualType()
    {
        return GridSystemVisual.GridVisualType.Cookie;
    }

    public override string GetName()
    {
        return "Cookie";
    }

    public override ActionType GetActionType()
    {
        return ActionType.Magic;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition gridPosition = new GridPosition();
        Entity entity = GetComponent<Entity>();

        if(entity is Player player)
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(player.transform.position);
        } 
        else if(entity is Enemy enemy)
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(enemy.transform.position);
        }

        return GetValidActionGridPositionList(gridPosition, entity);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition gridPosition, Entity entity)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        int maxAttackDistance = 5;

        for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
        {
            for (int y = -maxAttackDistance; y <= maxAttackDistance; y++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, y);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                if(LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity() != null)
                {
                    if (!LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity().CanBeAttacked()) continue;
                }

                if (LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity() != null)
                {
                    if(entity is Player)
                    {
                        if (!LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity().IsEnemy())
                            continue;
                    }
                    else
                    {
                        if (LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity().IsEnemy())
                            continue;
                    }
                    
                }
                else
                {
                    continue;
                }

                if (gridPosition == testGridPosition)
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        

        return validGridPositionList;
    }

    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList().Contains(gridPosition);
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete, bool isInit)
    {
        this.onActionComplete = onActionComplete;

        if(IsValidActionGridPosition(gridPosition))
        {
            startGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            endGridPosition = gridPosition;

            cookie = Cookie.Create(LevelGrid.Instance.GetWorldPosition(startGridPosition));

            isAttacking = true;
            if(TryGetComponent<Player>(out Player player))
            {
                ActionManager.Instance.SetIsBusy(true, player);
            }
        }
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Entity targetEnemy = LevelGrid.Instance.GetGridObject(gridPosition).GetEntity();
        Debug.Log(targetEnemy);
        
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetEnemy.GetHealth().GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition, GetComponent<Entity>()).Count;
    }
    
}
