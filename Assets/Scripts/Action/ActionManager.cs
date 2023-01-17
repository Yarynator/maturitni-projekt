using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    public static ActionManager Instance { get; private set; }


    /*private BaseAction actualAction;

    private bool isBusy;*/

    private Dictionary<Player, BaseAction> actualActionDictionary;
    private Dictionary<Player, bool> isBusyDictionary;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("ActionManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        actualActionDictionary = new Dictionary<Player, BaseAction>();
        isBusyDictionary = new Dictionary<Player, bool>();
        MoveAction.OnAnyPlayerMove += MoveAction_OnAnyPlayerMove;
    }

    private void MoveAction_OnAnyPlayerMove(object sender, System.EventArgs e)
    {
        if (sender is Player player)
        {
            if (!actualActionDictionary.ContainsKey(player))
            {
                actualActionDictionary.Add(player, null);
                isBusyDictionary.Add(player, false);
            }

            if (actualActionDictionary[player] != null)
            {
                GridSystemVisual.Instance.HideAllGridPosition();
                List<GridPosition> gridPositionList = actualActionDictionary[player].GetValidActionGridPositionList();
                GridSystemVisual.Instance.ShowGridPositionList(gridPositionList, actualActionDictionary[player].GetGridVisualType());

                if (!BattleManager.Instance.IsBattle())
                {
                    if (actualActionDictionary[player] is MoveAction)
                    {
                        GridSystemVisual.Instance.HideAllGridPosition();
                    }
                }

                if (isBusyDictionary[player])
                {
                    GridSystemVisual.Instance.HideAllGridPosition();
                }
            }
        }
    }

    public void SetActualAction(BaseAction baseAction, Player player)
    {
        if (!actualActionDictionary.ContainsKey(player))
        {
            actualActionDictionary.Add(player, null);
            isBusyDictionary.Add(player, false);
        }

        actualActionDictionary[player] = baseAction;

        ReloadVisuals(player);
    }

    public BaseAction GetActualAction(Player player)
    {
        if (!actualActionDictionary.ContainsKey(player))
        {
            actualActionDictionary.Add(player, null);
            isBusyDictionary.Add(player, false);
        }

        return actualActionDictionary[player];
    }

    public void SetIsBusy(bool isBusy, Player player)
    {
        if (!actualActionDictionary.ContainsKey(player))
        {
            actualActionDictionary.Add(player, null);
            isBusyDictionary.Add(player, false);
        }
        isBusyDictionary[player] = isBusy;
    }

    public bool IsBusy(Player player)
    {
        if (!actualActionDictionary.ContainsKey(player))
        {
            actualActionDictionary.Add(player, null);
            isBusyDictionary.Add(player, false);
        }

        return isBusyDictionary[player];
    }

    public void ReloadVisuals(Player player)
    {
        if (!actualActionDictionary.ContainsKey(player))
        {
            actualActionDictionary.Add(player, null);
            isBusyDictionary.Add(player, false);
        }

        if (actualActionDictionary[player] != null)
        {
            GridSystemVisual.Instance.HideAllGridPosition();
            List<GridPosition> gridPositionList = actualActionDictionary[player].GetValidActionGridPositionList();
            GridSystemVisual.Instance.ShowGridPositionList(gridPositionList, actualActionDictionary[player].GetGridVisualType());

            if (!BattleManager.Instance.IsBattle())
            {
                if (actualActionDictionary[player] is MoveAction)
                {
                    GridSystemVisual.Instance.HideAllGridPosition();
                }
            }
        }
    }

}
