using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, Entity
{

    public static event EventHandler OnAnyPlayerDies;


    public static Player Create(GridPosition gridPosition, string playerName, string playerType, int level, int attack, int defense, int maxMoveDistance, int health, int maxHealth, string[] inventoryItems){
        List<string> playerTypeStringList = Resources.Load<PlayerTypeListSO>("PlayerTypeList").playerTypeStringList;
        List<PlayerTypeSO> playerTypeSOList = Resources.Load<PlayerTypeListSO>("PlayerTypeList").playerTypeSOList;

        int index = 0;
        foreach(string playerTypeString in playerTypeStringList)
        {
            if(playerTypeString == playerType)
            {
                break;
            }
            index++;
        }

        if(index > playerTypeSOList.Count - 1)
            index = 0;

        
        Player player = Instantiate(playerTypeSOList[index].playerPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity).GetComponent<Player>();
        player.SetSprite(playerTypeSOList[index].sprite);
        player.SetPlayerType(playerType);
        player.SetName(playerName);
        player.SetLevel(level);
        player.SetAttack(attack);
        player.SetDefense(defense);
        player.SetMaxMoveDistance(maxMoveDistance);

        Health h = player.GetComponent<Health>();
        player.SetHealth(h);
        h.SetHealth(health);
        h.SetMaxHealth(maxHealth);
        h.InitUI();

        Inventory inventory = player.GetComponent<Inventory>();

        List<ItemSO> itemList = Resources.Load<ItemListSO>("ItemList").list;
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            foreach(ItemSO item in itemList)
            {
                if(inventoryItems[i] == item.itemType.ToString())
                {
                    inventory.AddItemAtIndex(item, i);
                    break;
                }
            }
        }

        player.SetInventory(inventory);

        return player;
    }
    
    [SerializeField] private Sprite sprite;
    [SerializeField] private string playerType;
    [SerializeField] private string nameString;
    [SerializeField] private int level;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int maxMoveDistance = 7;

    private Transform playerListSingle;
    private Health health;
    private Inventory inventory;

    private GridPosition gridPosition;

    private void Start()
    {
        GridObject gridObject = LevelGrid.Instance.GetGridObject(LevelGrid.Instance.GetGridPosition(transform.position));
        gridObject.SetEntity(this);
        PlayerManager.Instance.AddPlayer(this);
        gridPosition = gridObject.GetGridPosition();
        Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(false);
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public string GetName()
    {
        return nameString;
    }

    public void SetName(string nameString)
    {
        this.nameString = nameString;
    }

    public string GetPlayerType()
    {
        return playerType;
    }

    public void SetPlayerType(string playerType)
    {
        this.playerType = playerType;
    }

    public int GetLevel()
    {
        return level;
    }

    public void SetLevel(int level)
    {
        this.level = level;
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

    public override string ToString()
    {
        return nameString;
    }

    public void SetGridPosition(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void SetPlayerListSingle(Transform playerListSingle)
    {
        this.playerListSingle = playerListSingle;
    }

    public Transform GetPlayerListSingle()
    {
        return this.playerListSingle;
    }

    public bool IsEnemy()
    {
        return false;
    }

    public bool canAttack()
    {
        return true;
    }

    public bool CanBeAttacked()
    {
        return true;
    }

    public void Damage(int damage, Entity sender)
    {
        health.AddHealth(-damage);
        health.UpdateBar();

        if(health.GetHealth() <= 0)
        {
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.GetGridObject(gridPosition).SetEntity(null);
            Pathfinding.Instance.GetNode(gridPosition.x, gridPosition.y).SetIsWalkable(true);
            PlayerManager.Instance.RemovePlayer(this);
            Destroy(gameObject);
            if (BattleManager.Instance.IsBattle())
            {
                BattleManager.Instance.RemoveEntityFromBattleOrderList(this);
            }
            //GridSystemVisual.Instance.ReloadGridPositionList(ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).GetValidActionGridPositionList());
            OnAnyPlayerDies?.Invoke(this, EventArgs.Empty);
        }

        Debug.Log(PlayerManager.Instance.GetPlayerList().Count);
        if(PlayerManager.Instance.GetPlayerList().Count == 0)
        {
            GameOverUI.Instance.Show();
        }
    }

    private void SetHealth(Health health)
    {
        this.health = health;
    }

    public Health GetHealth()
    {
        return health;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}
