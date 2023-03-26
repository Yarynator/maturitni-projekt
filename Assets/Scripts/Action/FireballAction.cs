using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAction : BaseAction
{

    private GridPosition startGridPosition;
    private GridPosition endGridPosition;

    private bool isAttacking;

    private Bullet bullet;

    Action onActionComplete;

    private void Start()
    {
        SoundManager.Instance.OnVolumeChange += Instance_OnVolumeChange;
    }

    private void Instance_OnVolumeChange(object sender, EventArgs e)
    {
        GetComponent<AudioSource>().volume = SoundManager.Instance.GetVolume();
    }

    private void Update()
    {
        if (isAttacking)
        {
            Vector2 dir = (LevelGrid.Instance.GetWorldPosition(endGridPosition) - LevelGrid.Instance.GetWorldPosition(startGridPosition)).normalized;

            float bulletSpeed = 8f;
            bullet.transform.position += new Vector3(dir.x, dir.y) * Time.deltaTime * bulletSpeed;

            if(Vector2.Distance(bullet.transform.position, LevelGrid.Instance.GetWorldPosition(endGridPosition)) < .1f)
            {
                isAttacking = false;
                if (TryGetComponent<Player>(out Player player))
                {
                    ActionManager.Instance.SetIsBusy(false, player);
                }
                bullet.Complete();
                onActionComplete?.Invoke();
                LevelGrid.Instance.GetGridObject(endGridPosition).GetEntity().Damage(10, GetComponent<Entity>());
            }
        }
    }

    public override GridSystemVisual.GridVisualType GetGridVisualType()
    {
        return GridSystemVisual.GridVisualType.Fireball;
    }

    public override string GetName()
    {
        return "Fireball";
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

            bullet = Bullet.Create(LevelGrid.Instance.GetWorldPosition(startGridPosition));

            isAttacking = true;
            if(TryGetComponent<Player>(out Player player))
            {
                ActionManager.Instance.SetIsBusy(true, player);
            }

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = SoundManager.Instance.GetAudioClip(SoundManager.SoundType.Fireball, out float volume);
            audioSource.volume = volume;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Entity targetEnemy = LevelGrid.Instance.GetGridObject(gridPosition).GetEntity();
        
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
