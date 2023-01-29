using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public event EventHandler OnPlayerListChange;
    
    public static PlayerManager Instance { get; private set; }


    private Player actualPlayer;
    private List<Player> playerList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("PlayerManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerList = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        playerList.Add(player);
        OnPlayerListChange?.Invoke(this, EventArgs.Empty);
        Debug.Log("player added");
    }

    public void RemovePlayer(Player player)
    {
        playerList.Remove(player);
        OnPlayerListChange?.Invoke(this, EventArgs.Empty);
    }

    public List<Player> GetPlayerList()
    {
        return playerList;
    }

    public void SetActualPlayer(Player player)
    {
        actualPlayer = player;

        PlayerListUI.Instance.ResetSelectedList();
        if (player != null)
        {
            PlayerInfo.Instance.Show(player);

            ShowActions();
            
            CameraMovement.Instance.transform.position = player.transform.position;
            player.GetPlayerListSingle().Find("selected").gameObject.SetActive(true);
        } 
        else
        {
            PlayerInfo.Instance.Hide();
            ActionsUI.Instance.Hide();

            GridSystemVisual.Instance.HideAllGridPosition();
        }
    }

    public Player GetActualPlayer()
    {
        return actualPlayer;
    }

    public void ShowActions()
    {
        BaseAction[] actions = actualPlayer.GetComponents<BaseAction>();
        ActionsUI.Instance.Show(actions);
    }

}
