using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAction : BaseAction
{

    public static event EventHandler OnAnyPlayerMove;

    private Entity entity;
    
    private List<GridPosition> moveList;
    private bool isMoving;
    private int moveIndex;

    private bool isInit;
    private float speed;

    private Action onActionComplete;

    private void Start()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        if (isMoving)
        {
            if(moveIndex < moveList.Count - 1)
            {
                Vector2 dir = LevelGrid.Instance.GetWorldPosition(moveList[moveIndex + 1]);
                dir -= new Vector2(transform.position.x, transform.position.y);

                transform.position += new Vector3(dir.x, dir.y).normalized * Time.deltaTime * speed;

                if (dir.x < 0)
                {
                    if (transform.localScale.x < 0)
                    {
                        Vector3 scale = transform.localScale;
                        scale.x = -scale.x;
                        transform.localScale = scale;
                    }
                }
                else
                {
                    if (transform.localScale.x > 0)
                    {
                        Vector3 scale = transform.localScale;
                        scale.x = -scale.x;
                        transform.localScale = scale;
                    }
                }

                if (Vector2.Distance(transform.position, LevelGrid.Instance.GetWorldPosition(moveList[moveIndex + 1])) < .1f)
                {
                    moveIndex++;
                }

                OnAnyPlayerMove?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                isMoving = false;
                if (TryGetComponent<Player>(out Player player))
                {
                    ActionManager.Instance.SetIsBusy(false, player);
                }
                GridObject gridObject = LevelGrid.Instance.GetGridObject(LevelGrid.Instance.GetGridPosition(transform.position));
                gridObject.SetEntity(GetComponent<Entity>());

                if(BattleManager.Instance.IsBattle())
                {
                    if (!isInit)
                    {
                        GridSystemVisual.Instance.ShowGridPositionList(GetValidActionGridPositionList(), GridSystemVisual.GridVisualType.Move);
                    }
                }
                OnAnyPlayerMove?.Invoke(this, EventArgs.Empty);
                onActionComplete?.Invoke();
            }
        }
    }

    public override string GetName()
    {
        return "Move";
    }

    public override ActionType GetActionType()
    {
        return ActionType.Move;
    }

    public override GridSystemVisual.GridVisualType GetGridVisualType()
    {
        return GridSystemVisual.GridVisualType.Move;
    }

    public override void TakeAction(GridPosition clickedGridPosition, Action onActionComplete, bool isInit)
    {
        TakeAction(clickedGridPosition, onActionComplete, isInit);
    }

    public void TakeAction(GridPosition clickedGridPosition, Action onActionComplete, bool isInit, float speed = 2f)
    {
        this.speed = speed;
        this.onActionComplete = onActionComplete;
        this.isInit = isInit;
        if (!isMoving)
        {
            if (LevelGrid.Instance.IsValidGridPosition(clickedGridPosition))
            {
                moveList = Pathfinding.Instance.FindPath(LevelGrid.Instance.GetGridPosition(transform.position), clickedGridPosition, out int pathLength);
            }
            else
            {
                moveList = null;
            }

            if (moveList != null)
            {
                if (BattleManager.Instance.IsBattle() && !isInit)
                {
                    if (!IsValidActionGridPosition(clickedGridPosition))
                    {
                        return;
                    }
                }

                GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
                LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(null);
                Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(true);

                GridPosition endGridPosition = moveList[moveList.Count - 1];
                Pathfinding.Instance.GetNode(endGridPosition.x, endGridPosition.y).SetIsWalkable(false);

                GridSystemVisual.Instance.HideAllGridPosition();

                if (BattleManager.Instance.IsBattle())
                {
                    BattleManager.Instance.DecrStemsRemains(moveList.Count - 1);
                }

                moveIndex = 0;

                isMoving = true;
                if (TryGetComponent<Player>(out Player player))
                {
                    ActionManager.Instance.SetIsBusy(true, player);
                }
            }
            else
            {
                WorldTextManager.Instance.CreateWorldText(LevelGrid.Instance.GetWorldPosition(clickedGridPosition), "You can not get out of here");
            }
        }
    }

    public override bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        int maxMoveDistance = BattleManager.Instance.GetStepsRemains();
        GridPosition playerGridPosition = new GridPosition();

        if (entity is Player player)
        {
            playerGridPosition = LevelGrid.Instance.GetGridPosition(player.transform.position);
        } 
        else if(entity is Enemy enemy)
        {
            playerGridPosition = LevelGrid.Instance.GetGridPosition(enemy.transform.position);
        }
        //GridPosition playerGridPosition = LevelGrid.Instance.GetGridPosition(entity.transform.position); // player.GetGridPosition();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int y = -maxMoveDistance; y <= maxMoveDistance; y++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, y);
                GridPosition testGridPosition = playerGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                    continue;
                if (playerGridPosition == testGridPosition)
                    continue;
                if (Pathfinding.Instance.GetNotWalkableGridPositionList().Contains(testGridPosition))
                    continue;

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtFireballGridPosition = 0;
        int targetCountAtAttackGridPosition = 0;

        if (BattleManager.Instance.CanUseMagic())
        {
            if (TryGetComponent<FireballAction>(out FireballAction fireballAction))
            {
                targetCountAtFireballGridPosition = fireballAction.GetTargetCountAtPosition(gridPosition);
            }
        }
        if (BattleManager.Instance.CanUseAttack())
        {
            if(TryGetComponent<AttackAction>(out AttackAction attackAction))
            {
                targetCountAtAttackGridPosition = attackAction.GetTargetCountAtPosition(gridPosition);
            }
        }

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 10 * targetCountAtFireballGridPosition + 10 * targetCountAtAttackGridPosition,
        };
    }

}
