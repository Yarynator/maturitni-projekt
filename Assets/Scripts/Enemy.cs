using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Entity
{

    public static event EventHandler OnAnyEnemyDies;
   
    public static Enemy Create(GridPosition gridPosition, string playerType, int attack, int defense, int maxMoveDistance, List<string> actions, int health)
    {
        EnemyTypeSO enemyType = null;

        List<EnemyTypeSO> enemyTypeList = Resources.Load<EnemyTypeListSO>("EnemyTypeList").enemyTypeList;
        foreach (EnemyTypeSO enemyTypeSingle in enemyTypeList)
        {
            if(enemyTypeSingle.nameString == playerType)
            {
                enemyType = enemyTypeSingle;
            }
        }

        if(enemyType == null)
        {
            enemyType = enemyTypeList[0];
        }

        Enemy enemy = Instantiate(enemyType.enemyPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity).GetComponent<Enemy>();
        enemy.SetAttack(attack);
        enemy.SetDefense(defense);
        enemy.SetMaxMoveDistance(maxMoveDistance);
        enemy.SetName(enemyType.nameString);

        Health h = enemy.GetComponent<Health>();
        enemy.SetHealth(h);
        h.SetHealth(health);
        h.SetMaxHealth(health);
        h.InitUI();

        foreach (string action in actions)
        {
            switch (action)
            {
                case "move":
                    enemy.gameObject.AddComponent<MoveAction>();
                    break;
                case "attack":
                    enemy.gameObject.AddComponent<AttackAction>();
                    break;
                case "fireball":
                    enemy.gameObject.AddComponent<FireballAction>();
                    break;
                case "cookie":
                    enemy.gameObject.AddComponent<CookieAction>();
                    break;
            }
        }

        LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(enemy);
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(false);

        return enemy;
    }

    private int attack;
    private int defense;
    private int maxMoveDistance;
    private string nameString;

    private Health health;

    public bool canAttack()
    {
        return true;
    }

    public void Damage(int damage, Entity sender)
    {
        health.AddHealth(-damage);
        health.UpdateBar();

        if (health.GetHealth() <= 0) {
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(null);
            Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(true);
            EnemyManager.Instance.RemoveEnemyFromList(this);
            if (BattleManager.Instance.IsBattle())
            {
                BattleManager.Instance.RemoveEntityFromBattleOrderList(this);
                BattleManager.Instance.SetLastDeadEnemyPosition(LevelGrid.Instance.GetGridPosition(transform.position));
            }
            Destroy(gameObject);
            GridSystemVisual.Instance.ReloadGridPositionList(ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).GetValidActionGridPositionList());
            OnAnyEnemyDies?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return true;
    }

    public int GetAttack()
    {
        return attack;
    }

    public void SetAttack(int attack)
    {
        this.attack = attack;
    }

    public int GetDefense()
    {
        return defense;
    }

    public void SetDefense(int defense)
    {
        this.defense = defense;
    }

    public int GetMaxMoveDistance()
    {
        return maxMoveDistance;
    }

    public void SetMaxMoveDistance(int maxMoveDistance)
    {
        this.maxMoveDistance = maxMoveDistance;
    }

    public string GetName()
    {
        return nameString;
    }

    public override string ToString()
    {
        return nameString;
    }

    public void SetName(string nameString)
    {
        this.nameString = nameString;
    }

    public bool CanBeAttacked()
    {
        return true;
    }

    public Health GetHealth()
    {
        return health;
    }

    private void SetHealth(Health health)
    {
        this.health = health;
    }

}
