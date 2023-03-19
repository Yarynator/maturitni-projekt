using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInit : MonoBehaviour
{

    [SerializeField] private Transform cameraFollow;
    WorldData worldData;
    List<Player> playerList;

    private void Start()
    {
        int saveIndex = PlayerPrefs.GetInt("Save");
        worldData = SaveSystemWorldData.LoadWorldData(saveIndex);
        playerList = new List<Player>();
        
        if (!SceneInfo.Instance.GetInsideBuilding())
        {
            for (int i = 0; i < worldData.playerAmount; i++)
            {
                PlayerData playerData = SaveSystemWorldData.LoadPlayerData(saveIndex, i);
                if (worldData.fromSceneIndex == -1)
                {
                    string[] inventoryItems = { playerData.inventoryHead, playerData.inventoryBody, playerData.inventoryLegs, playerData.inventoryShoes, playerData.inventoryItem1, playerData.inventoryItem2, playerData.inventoryItem3, playerData.inventoryItem4, playerData.inventoryItem5, playerData.inventoryItem6, playerData.inventoryItem7, playerData.inventoryItem8 };
                    playerList.Add(Player.Create(new GridPosition(playerData.xPosition, playerData.yPosition), playerData.playerName, playerData.playerType, playerData.level, playerData.attack, playerData.defense, playerData.maxMoveDistance, playerData.health, playerData.maxHealth, inventoryItems));
                }
                else
                {
                    foreach (SceneDataSO sceneData in Resources.Load<SceneDataListSO>("SceneDataList").sceneDataList)
                    {
                        if (sceneData.fromScene == worldData.fromSceneIndex)
                        {
                            if (sceneData.toScene == worldData.sceneIndex)
                            {
                                List<GridPosition> playerGridPositionList = Pathfinding.Instance.GetNearestGridPositionList(sceneData.spawnGridPosition, worldData.playerAmount);
                                string[] inventoryItems = { playerData.inventoryHead, playerData.inventoryBody, playerData.inventoryLegs, playerData.inventoryShoes, playerData.inventoryItem1, playerData.inventoryItem2, playerData.inventoryItem3, playerData.inventoryItem4, playerData.inventoryItem5, playerData.inventoryItem6, playerData.inventoryItem7, playerData.inventoryItem8 };
                                playerList.Add(Player.Create(playerGridPositionList[i], playerData.playerName, playerData.playerType, playerData.level, playerData.attack, playerData.defense, playerData.maxMoveDistance, playerData.health, playerData.maxHealth, inventoryItems));
                            }
                        }
                    }
                }
            }

            if (SceneInfo.Instance.GetSceneIndex() == 1)
            {
                GameObject bush = GameObject.Find("TileDoorClosedMiddle");
                if (bush != null)
                {
                    if (bush.TryGetComponent<BushDoor>(out BushDoor bushDoor))
                    {
                        if (SceneInfo.Instance.GetObjectsData().tutorialBushIsDestroyed)
                        {
                            bushDoor.Damage(10, null);
                        }
                    }
                }
            }

            foreach (SceneDataSO sceneData in Resources.Load<SceneDataListSO>("SceneDataList").sceneDataList)
            {
                if (sceneData.fromScene == worldData.fromSceneIndex)
                {
                    if (sceneData.toScene == worldData.sceneIndex)
                    {
                        cameraFollow.position = LevelGrid.Instance.GetWorldPosition(sceneData.spawnGridPosition);
                    }
                }
            }
        }
        else 
        {
            if (SceneInfo.Instance.GetSceneIndex() == worldData.sceneIndex)
            {
                for (int i = 0; i < worldData.playerAmount; i++)
                {
                    PlayerData playerData = SaveSystemWorldData.LoadPlayerData(saveIndex, i);

                    string[] inventoryItems = { playerData.inventoryHead, playerData.inventoryBody, playerData.inventoryLegs, playerData.inventoryShoes, playerData.inventoryItem1, playerData.inventoryItem2, playerData.inventoryItem3, playerData.inventoryItem4, playerData.inventoryItem5, playerData.inventoryItem6, playerData.inventoryItem7, playerData.inventoryItem8 };
                    Player.Create(new GridPosition(playerData.xPosition, playerData.yPosition), playerData.playerName, playerData.playerType, playerData.level, playerData.attack, playerData.defense, playerData.maxMoveDistance, playerData.health, playerData.maxHealth, inventoryItems);
                }
            }
            else
            {
                switch (SceneInfo.Instance.GetSceneIndex())
                {
                    case 5:
                        List<GridPosition> outsideGridPositionList = Pathfinding.Instance.GetNearestGridPositionList(Resources.Load<BuildingSpawnPositionSO>("PriestRestaurantSpawnPosition").outsideSpawnPosition, worldData.playerAmount);
                        for (int i = 0; i < worldData.playerAmount; i++)
                        {
                            PlayerData playerData = SaveSystemWorldData.LoadPlayerData(saveIndex, i);

                            string[] inventoryItems = { playerData.inventoryHead, playerData.inventoryBody, playerData.inventoryLegs, playerData.inventoryShoes, playerData.inventoryItem1, playerData.inventoryItem2, playerData.inventoryItem3, playerData.inventoryItem4, playerData.inventoryItem5, playerData.inventoryItem6, playerData.inventoryItem7, playerData.inventoryItem8 };
                            Player.Create(outsideGridPositionList[i], playerData.playerName, playerData.playerType, playerData.level, playerData.attack, playerData.defense, playerData.maxMoveDistance, playerData.health, playerData.maxHealth, inventoryItems);
                        }
                        SceneInfo.Instance.SetInsideBuilding(false);
                        break;
                    case 6:
                        List<GridPosition> playerGridPositionList = Pathfinding.Instance.GetNearestGridPositionList(Resources.Load<BuildingSpawnPositionSO>("PriestRestaurantSpawnPosition").insideSpawnPosition, worldData.playerAmount);
                        for (int i = 0; i < worldData.playerAmount; i++)
                        {
                            PlayerData playerData = SaveSystemWorldData.LoadPlayerData(saveIndex, i);

                            string[] inventoryItems = { playerData.inventoryHead, playerData.inventoryBody, playerData.inventoryLegs, playerData.inventoryShoes, playerData.inventoryItem1, playerData.inventoryItem2, playerData.inventoryItem3, playerData.inventoryItem4, playerData.inventoryItem5, playerData.inventoryItem6, playerData.inventoryItem7, playerData.inventoryItem8 };
                            Player.Create(playerGridPositionList[i], playerData.playerName, playerData.playerType, playerData.level, playerData.attack, playerData.defense, playerData.maxMoveDistance, playerData.health, playerData.maxHealth, inventoryItems);
                        }
                        break;
                }
            }
        }

        BattleInit();
    }

    private void Update()
    {
        foreach (SceneDataSO sceneData in Resources.Load<SceneDataListSO>("SceneDataList").sceneDataList)
        {
            if (sceneData.fromScene == worldData.fromSceneIndex)
            {
                if (sceneData.toScene == worldData.sceneIndex)
                {
                    List<GridPosition> nearestGridPositionList = Pathfinding.Instance.GetNearestGridPositionListWithoutNotWalkablePositions(sceneData.touchGridPosition, worldData.playerAmount);

                    for (int i = 0; i < nearestGridPositionList.Count; i++)
                    {
                        if (playerList[i].TryGetComponent<MoveAction>(out MoveAction moveAction))
                        {
                            moveAction.TakeAction(nearestGridPositionList[i], () => { }, true);
                        }
                    }
                }
            }
        }
        Destroy(this);

    }

    private void BattleInit()
    {
        List<BattleInfoSO> battleInfoList = Resources.Load<BattleInfoListSO>("BattleInfoList").battleInfoList;
        for (int i = 0; i < battleInfoList.Count; i++)
        {
            switch(i)            
            {
                case 0:
                    if(SceneInfo.Instance.TutorialBattleIsActive() && SceneInfo.Instance.GetSceneIndex() == battleInfoList[0].battleScene)
                    {
                        List<BattleInfoSO.EnemyData> enemyList = battleInfoList[0].enemyList;

                        foreach(BattleInfoSO.EnemyData enemyData in enemyList)
                        {
                            Enemy enemy = Enemy.Create(enemyData.gridPosition, enemyData.playerType, enemyData.attack, enemyData.defense, enemyData.maxMoveDistance, enemyData.actions, enemyData.health);
                            EnemyManager.Instance.AddEnemyToList(enemy);
                        }

                        SceneInfo.Instance.SetBattleIndex(0);
                        BattleManager.Instance.SetIsBattle(true);
                    }
                    break;
                case 1:
                    if (SceneInfo.Instance.PriestRestaurantIsActive() && SceneInfo.Instance.GetSceneIndex() == battleInfoList[1].battleScene)
                    {
                        List<BattleInfoSO.EnemyData> enemyList = battleInfoList[1].enemyList;

                        foreach (BattleInfoSO.EnemyData enemyData in enemyList)
                        {
                            Enemy enemy = Enemy.Create(enemyData.gridPosition, enemyData.playerType, enemyData.attack, enemyData.defense, enemyData.maxMoveDistance, enemyData.actions, enemyData.health);
                            EnemyManager.Instance.AddEnemyToList(enemy);
                        }


                        SceneInfo.Instance.SetBattleIndex(1);
                        BattleManager.Instance.SetIsBattle(true);
                    }
                    break;
            }
        }
    }

}
