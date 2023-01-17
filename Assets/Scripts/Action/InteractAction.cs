using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractAction : BaseAction
{
    public override ActionType GetActionType()
    {
        return ActionType.Interact;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            actionValue = -1,
            gridPosition = gridPosition
        };
    }

    public override GridSystemVisual.GridVisualType GetGridVisualType()
    {
        return GridSystemVisual.GridVisualType.Interact;
    }

    public override string GetName()
    {
        return "Interact";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {

        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);

        int maxAttackDistance = 1;

        for (int x = -maxAttackDistance; x <= maxAttackDistance; x++)
        {
            for (int y = -maxAttackDistance; y <= maxAttackDistance; y++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, y);
                GridPosition testGridPosition = gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                if (!(LevelGrid.Instance.GetGridObject(testGridPosition).GetEntity() is Interactable))
                    continue;


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

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete, bool isInit = false)
    {
        if (IsValidActionGridPosition(gridPosition))
        {
            Interactable interactable = LevelGrid.Instance.GetGridObject(gridPosition).GetEntity() as Interactable;

            /*switch (interactableNPC.GetInteractableType())
            {
                case Interactable.InteractableType.PriestRestaurant:
                    SceneInfo.Instance.SetPriestRestaurantQuestIsActive(true);
                    break;
            }*/

            switch(interactable.GetInteractableType())
            {
                case Interactable.InteractableType.PriestRestaurant:
                    if (SceneInfo.Instance.GetQuestData().priestRestaurantShowed)
                    {
                        InteractableNPC interactableNPC = interactable as InteractableNPC;
                        
                        if(!SceneInfo.Instance.GetQuestData().priestRestaurantQuestIsActive)
                        {
                            bool hasItem = false;
                            foreach(ItemSO inventoryItem in PlayerManager.Instance.GetActualPlayer().GetInventory().GetItemsInInventory())
                            {
                                if (inventoryItem != null)
                                {
                                    if (inventoryItem.itemType == ItemSO.ItemType.PriestChest)
                                    {
                                        hasItem = true;
                                    }
                                }
                            }

                            if(!hasItem)
                            {
                                WorldTextManager.Instance.CreateWorldText(MouseWorld.Instance.GetWorldPosition(), "Player with chest should talk with this person");
                                break;
                            }
                        }
                        
                        TalkUI.Instance.Show(interactableNPC.GetQuest(), SceneInfo.Instance.GetQuestData().priestRestaurantAlreadyShowed, SceneInfo.Instance.GetQuestData().priestRestarurantIsDone, SceneInfo.Instance.GetQuestData().priestRestaurantTalkLevel, PlayerManager.Instance.GetActualPlayer().GetSprite(), PlayerManager.Instance.GetActualPlayer().GetName(), () =>
                        {
                            SceneInfo.Instance.GetQuestData().priestRestaurantAlreadyShowed = true;
                        }, () => {
                            SceneInfo.Instance.GetQuestData().priestRestaurantQuestIsActive = true;
                        });

                        if(SceneInfo.Instance.GetQuestData().priestRestarurantIsDone)
                        {
                            SceneInfo.Instance.GetQuestData().priestRestaurantShowed = false;
                        }
                    }
                    break;
                case Interactable.InteractableType.RestaurantBuilding:
                    if(SceneInfo.Instance.GetQuestData().priestRestaurantQuestIsActive && SceneInfo.Instance.GetQuestData().priestRestaurantTalkLevel == 0)
                    {
                        /*PlayerData[] playerDataArray = new PlayerData[PlayerManager.Instance.GetPlayerList().Count];

                        for (int i = 0; i < playerDataArray.Length; i++)
                        {
                            Player player = PlayerManager.Instance.GetPlayerList()[i];
                            playerDataArray[i] = new PlayerData(player.GetGridPosition().x, player.GetGridPosition().y, player.GetName(), player.GetPlayerType(), player.GetLevel(), player.GetAttack(), player.GetDefense(), player.GetMaxMoveDistance(), player.GetHealth().GetHealth(), player.GetHealth().GetMaxHealth());
                        }

                        SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"))*/

                        Debug.Log("quest completed");
                        /*SceneInfo.Instance.GetQuestData().priestRestarurantIsDone = true;
                        SceneInfo.Instance.GetQuestData().priestRestaurantTalkLevel = 1;
                        QuestListUI.Instance.UpdateListUI();*/

                        PlayerData[] playerDataArray = new PlayerData[PlayerManager.Instance.GetPlayerList().Count];

                        for (int i = 0; i < playerDataArray.Length; i++)
                        {
                            Player player = PlayerManager.Instance.GetPlayerList()[i];
                            playerDataArray[i] = new PlayerData(-1, -1, player.GetName(), player.GetPlayerType(), player.GetLevel(), player.GetAttack(), player.GetDefense(), player.GetMaxMoveDistance(), player.GetHealth().GetHealth(), player.GetHealth().GetMaxHealth(), player.GetInventory().GetItemsInInventory());
                        }

                        SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"), SceneInfo.Instance.GetSceneIndex(), 5, true, SceneInfo.Instance.TutorialBattleIsActive(), SceneInfo.Instance.PriestRestaurantIsActive(), SceneInfo.Instance.GetQuestData(), SceneInfo.Instance.GetObjectsData());
                        SceneManager.LoadScene(6);
                    }
                    break;
                case Interactable.InteractableType.LeaveRestaurant:

                    if(!BattleManager.Instance.IsBattle())
                    {
                        PlayerData[] playerDataArray = new PlayerData[PlayerManager.Instance.GetPlayerList().Count];

                        for (int i = 0; i < playerDataArray.Length; i++)
                        {
                            Player player = PlayerManager.Instance.GetPlayerList()[i];
                            playerDataArray[i] = new PlayerData(-1, -1, player.GetName(), player.GetPlayerType(), player.GetLevel(), player.GetAttack(), player.GetDefense(), player.GetMaxMoveDistance(), player.GetHealth().GetHealth(), player.GetHealth().GetMaxHealth(), player.GetInventory().GetItemsInInventory());
                        }

                        SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"), SceneInfo.Instance.GetSceneIndex(), 6, true, SceneInfo.Instance.TutorialBattleIsActive(), SceneInfo.Instance.PriestRestaurantIsActive(), SceneInfo.Instance.GetQuestData(), SceneInfo.Instance.GetObjectsData());
                        SceneManager.LoadScene(5);
                    }
                    break;
                case Interactable.InteractableType.PriestChest:

                    List<ItemSO> list = Resources.Load<ItemListSO>("ItemList").list;
                    ItemSO item = new ItemSO();

                    foreach(ItemSO i in list)
                    {
                        if(i.itemType == ItemSO.ItemType.PriestChest)
                        {
                            item = i;
                        }
                    }

                    if (PlayerManager.Instance.GetActualPlayer().GetInventory().TryAddItem(item))
                    {
                        SceneInfo.Instance.GetObjectsData().priestChestIsActive = false;
                        Entity chest = LevelGrid.Instance.GetGridObject(gridPosition).GetEntity();
                        chest.Damage(10, PlayerManager.Instance.GetActualPlayer());
                    }
                    break;
                default:
                    Debug.Log("Interactable Object Is Not Setuped");
                    break;
            }

            
        }
    }
}
