using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerCursor : MonoBehaviour
{

    public static PlayerCursor Instance { get; private set; }


    [SerializeField] private Transform playerCursorInfo;
    private TextMeshProUGUI playerCursorInfoTextMeshPro;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("PlayerCursor already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerCursorInfo.GetComponent<Canvas>().sortingOrder = 20;
        playerCursorInfoTextMeshPro = playerCursorInfo.Find("text").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (PlayerManager.Instance.GetActualPlayer() != null)
            {
                if (!ActionManager.Instance.IsBusy(PlayerManager.Instance.GetActualPlayer()))
                {
                    if (BattleManager.Instance.IsBattle())
                    {
                        if(ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).GetActionType() == ActionType.Attack && BattleManager.Instance.CanUseAttack())
                        {
                            if (ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).GetValidActionGridPositionList().Contains(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition())))
                            {
                                ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).TakeAction(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition()), () => { });
                                BattleManager.Instance.UseAttack();
                                BattleInfoUI.Instance.SetAttackVisibility(false);
                            }
                        }
                        else if(ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).GetActionType() == ActionType.Magic && BattleManager.Instance.CanUseMagic())
                        {
                            if (ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).GetValidActionGridPositionList().Contains(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition())))
                            {
                                ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).TakeAction(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition()), () => { });
                                BattleManager.Instance.UseMagic();
                                BattleInfoUI.Instance.SetMagicVisibility(false);
                            }
                        }
                        else if(ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).GetActionType() == ActionType.Move)
                        {
                            ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).TakeAction(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition()), () => { });
                        }
                    }
                    else
                    {
                        ActionManager.Instance.GetActualAction(PlayerManager.Instance.GetActualPlayer()).TakeAction(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition()), () => { });
                    }
                }
            }
            else if (PlayerListUI.Instance.IsChoosenAll())
            {
                List<GridPosition> nearestGridPositionList = Pathfinding.Instance.GetNearestGridPositionList(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition()), PlayerManager.Instance.GetPlayerList().Count);
                int index = 0;
                foreach (Player player in PlayerManager.Instance.GetPlayerList())
                {
                    if(player.TryGetComponent<MoveAction>(out MoveAction moveAction))
                    {
                        moveAction.TakeAction(nearestGridPositionList[index], () => { }, false);
                        index++;
                    }
                }
            }

        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (LevelGrid.Instance.IsValidGridPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition())))
                {
                    PlayerListUI.Instance.SetIsChoosenAll(false);
                    if (!BattleManager.Instance.IsBattle())
                    {
                        if (LevelGrid.Instance.GetGridObject(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetWorldPosition())).GetEntity() is Player player)
                        {
                            PlayerManager.Instance.SetActualPlayer(player);
                        }
                        else
                        {
                            PlayerManager.Instance.SetActualPlayer(null);
                        }
                    }
                }
            }
        }
    }

    public void SetPlayerCursorInfo(Vector2 worldPosition, string text)
    {
        playerCursorInfoTextMeshPro.text = text;
        playerCursorInfo.transform.position = worldPosition;
    }

    public void SetActivePlayerCursorInfo(bool isActive)
    {
        playerCursorInfo.gameObject.SetActive(isActive);
    }

}
