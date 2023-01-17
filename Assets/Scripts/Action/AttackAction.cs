using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BaseAction
{

    private enum State
    {
        AttackStart,
        Attack,
        AttackEnd
    }

    private GridPosition startGridPosition;
    private GridPosition endGridPosition;

    private bool isAttacking;
    private State state;

    private Action onActionComplete;

    private void Update()
    {
        if (isAttacking)
        {
            Vector2 dir = (LevelGrid.Instance.GetWorldPosition(endGridPosition) - LevelGrid.Instance.GetWorldPosition(startGridPosition)).normalized;
            dir *= Time.deltaTime * 10;

            switch (state)
            {
                case State.AttackStart:
                    transform.position += new Vector3(dir.x, dir.y);

                    if(Vector2.Distance(transform.position, LevelGrid.Instance.GetWorldPosition(endGridPosition)) < .1f){
                        state = State.Attack;
                    }
                    if(Vector2.Distance(transform.position, LevelGrid.Instance.GetWorldPosition(endGridPosition)) > 2f)
                    {
                        state = State.Attack;
                    }
                    break;
                case State.Attack:
                    if (TryGetComponent<Player>(out Player player))
                    {
                        LevelGrid.Instance.GetGridObject(endGridPosition).GetEntity().Damage((int)((player.GetAttack() * .5f) * player.GetLevel() + 1), player);
                    }
                    else if (TryGetComponent<Enemy>(out Enemy enemy))
                    {
                        LevelGrid.Instance.GetGridObject(endGridPosition).GetEntity().Damage(enemy.GetAttack(), player);
                    }
                    state = State.AttackEnd;
                    break;
                case State.AttackEnd:
                    
                    transform.position += new Vector3(-dir.x, -dir.y);

                    if (Vector2.Distance(transform.position, LevelGrid.Instance.GetWorldPosition(startGridPosition)) < .1f)
                    {
                        isAttacking = false;
                        if (TryGetComponent<Player>(out Player p))
                        {
                            ActionManager.Instance.SetIsBusy(false, p);
                        }
                        onActionComplete?.Invoke();
                    }
                    if(Vector2.Distance(transform.position, LevelGrid.Instance.GetWorldPosition(startGridPosition)) > 2f)
                    {
                        transform.position = LevelGrid.Instance.GetWorldPosition(startGridPosition);

                        isAttacking = false;
                        if (TryGetComponent<Player>(out Player p))
                        {
                            ActionManager.Instance.SetIsBusy(false, p);
                        }
                        onActionComplete?.Invoke();
                    }
                    break;
            }
        }
    }

    public override string GetName()
    {
        return "Attack";
    }

    public override ActionType GetActionType()
    {
        return ActionType.Attack;
    }

    public override GridSystemVisual.GridVisualType GetGridVisualType()
    {
        return GridSystemVisual.GridVisualType.Attack;
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

        int maxAttackDistance = 1;

        for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
        {
            for (int y = -maxAttackDistance; y <= maxAttackDistance; y++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, y);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                if (LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity() != null)
                {
                    if (entity is Player)
                    {
                        if (!LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity().IsEnemy())
                            continue;
                    }
                    else if(entity is Enemy)
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

        if (IsValidActionGridPosition(gridPosition))
        {
            startGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            endGridPosition = gridPosition;
            state = State.AttackStart;
            isAttacking = true;
            if (TryGetComponent<Player>(out Player player))
            {
                ActionManager.Instance.SetIsBusy(true, player);
            }
        }
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Entity targetEntity = LevelGrid.Instance.GetGridObject(gridPosition).GetEntity();

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 50 + Mathf.RoundToInt((1 - targetEntity.GetHealth().GetHealthNormalized()) * 50f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition, GetComponent<Entity>()).Count;
    }

}
