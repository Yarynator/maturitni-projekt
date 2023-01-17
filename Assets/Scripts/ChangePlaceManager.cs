using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChangePlaceManager : MonoBehaviour
{

    [SerializeField] private List<ChangePlace> changePlaceList;


    private ChangePlace changePlace;
    private List<Player> changedPlayerPlaceList;

    private bool shouldMove;

    private void Start()
    {
        PlayerCursor.Instance.SetPlayerCursorInfo(MouseWorld.Instance.GetWorldPosition(), "Nejaky random text");
        
        if(!SceneInfo.Instance.GetInsideBuilding())
        {
            shouldMove = true;
        }
    }

    private void Update()
    {
        if(shouldMove)
        {
            if (changePlace != null)
            {
                CheckPlayers();
            }
            else
            {
                Collider2D[] colliderArray = Physics2D.OverlapBoxAll(MouseWorld.Instance.GetWorldPosition(), Vector2.zero, 0f);

                foreach (Collider2D collider in colliderArray)
                {
                    foreach (ChangePlace changePlace in changePlaceList)
                    {
                        if (changePlace.IsThisCollider(collider))
                        {
                            if (!EventSystem.current.IsPointerOverGameObject())
                            {
                                PlayerCursor.Instance.SetActivePlayerCursorInfo(true);
                                PlayerCursor.Instance.SetPlayerCursorInfo(MouseWorld.Instance.GetWorldPosition(), $"Go to {changePlace.GetPlaceName()}");

                                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                                {
                                    bool playersAreNotBusy = true;
                                    bool canChangePosition = true;
                                    Player player = null;
                                    foreach (Player p in PlayerManager.Instance.GetPlayerList())
                                    {
                                        if (Pathfinding.Instance.FindPath(p.GetGridPosition(), changePlace.GetSceneData().touchGridPosition, out int pathLength) == null)
                                        {
                                            canChangePosition = false;
                                            player = p;
                                        }
                                        if (ActionManager.Instance.IsBusy(p))
                                        {
                                            playersAreNotBusy = false;
                                        }
                                    }
                                    if (canChangePosition && playersAreNotBusy)
                                    {
                                        if (!BattleManager.Instance.IsBattle())
                                        {
                                            this.changePlace = changePlace;
                                            changedPlayerPlaceList = new List<Player>();

                                            LevelGrid.Instance.GetGridObject(changePlace.GetSceneData().spawnGridPosition).SetEntity(null);
                                            Pathfinding.Instance.SetIsWalkableGridPosition(changePlace.GetSceneData().spawnGridPosition, true);
                                            List<GridPosition> nearestGridPositionList = Pathfinding.Instance.GetNearestGridPositionList(changePlace.GetSceneData().spawnGridPosition, PlayerManager.Instance.GetPlayerList().Count);
                                            int index = 0;
                                            foreach (Player p in PlayerManager.Instance.GetPlayerList())
                                            {
                                                if (p.TryGetComponent<MoveAction>(out MoveAction moveAction))
                                                {
                                                    moveAction.TakeAction(nearestGridPositionList[index], () => { Debug.Log("move ended"); }, false);
                                                    index++;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            WorldTextManager.Instance.CreateWorldText(MouseWorld.Instance.GetWorldPosition(), "You can not leave battle!");
                                        }
                                    }
                                    else if (!canChangePosition)
                                    {
                                        WorldTextManager.Instance.CreateWorldText(MouseWorld.Instance.GetWorldPosition(), player.GetName() + " can not reach this position!");
                                    }
                                    else
                                    {
                                        WorldTextManager.Instance.CreateWorldText(MouseWorld.Instance.GetWorldPosition(), "Aait until everyone has finished their action!");
                                    }
                                }
                            }

                            return;
                        }
                    }
                }
            }
        }
        else
        {
            if (!SceneInfo.Instance.GetInsideBuilding())
            {
                shouldMove = true;
            }
        }

        PlayerCursor.Instance.SetActivePlayerCursorInfo(false);
    }

    private void CheckPlayers()
    {
        foreach(Player player in PlayerManager.Instance.GetPlayerList())
        {
            if(LevelGrid.Instance.GetGridPosition(player.transform.position) == changePlace.GetSceneData().touchGridPosition)
            {
                if(!changedPlayerPlaceList.Contains(player))
                    changedPlayerPlaceList.Add(player);
            }
        }

        if(changedPlayerPlaceList.Count == PlayerManager.Instance.GetPlayerList().Count)
        {
            PlayerData[] playerDataArray = new PlayerData[PlayerManager.Instance.GetPlayerList().Count];
            for (int i = 0; i < playerDataArray.Length; i++)
            {
                Player player = PlayerManager.Instance.GetPlayerList()[i];
                playerDataArray[i] = new PlayerData(0, 0, player.GetName(), player.GetPlayerType(), player.GetLevel(), player.GetAttack(), player.GetDefense(), player.GetMaxMoveDistance(), player.GetHealth().GetHealth(), player.GetHealth().GetMaxHealth(), player.GetInventory().GetItemsInInventory());
            }
            SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"), changePlace.GetSceneIndex(), SceneInfo.Instance.GetSceneIndex(), false, SceneInfo.Instance.TutorialBattleIsActive(), SceneInfo.Instance.PriestRestaurantIsActive(), SceneInfo.Instance.GetQuestData(), SceneInfo.Instance.GetObjectsData());
            SceneManager.LoadScene(changePlace.GetSceneIndex());
        }
    }
}
