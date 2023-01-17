using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListUI : MonoBehaviour
{

    public static PlayerListUI Instance { get; private set; }


    [SerializeField] private Button chooseAllPlayers;
    [SerializeField] private Transform playerInfoPrefab;

    private List<Transform> playerInfoList;

    private bool isChoosenAll;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("PlayerListUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        playerInfoList = new List<Transform>();

        PlayerManager.Instance.OnPlayerListChange += Instance_OnPlayerListChange;

        chooseAllPlayers.onClick.AddListener(() =>
        {
            PlayerManager.Instance.SetActualPlayer(null);

            SetIsChoosenAll(!isChoosenAll);
        });

        isChoosenAll = false;
    }

    private void Instance_OnPlayerListChange(object sender, System.EventArgs e)
    {
        for(int i = 0; i < playerInfoList.Count; i++)
        {
            Destroy(playerInfoList[i].gameObject);
        }
        playerInfoList.Clear();

        foreach(Player player in PlayerManager.Instance.GetPlayerList())
        {
            Transform playerInfo = Instantiate(playerInfoPrefab, transform);
            playerInfo.Find("playerImage").GetComponent<Image>().sprite = player.GetSprite();
            playerInfo.Find("playerName").GetComponent<TextMeshProUGUI>().text = player.ToString();
            playerInfo.Find("selected").gameObject.SetActive(false);
            playerInfo.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!BattleManager.Instance.IsBattle())
                {
                    SetIsChoosenAll(false);
                    PlayerManager.Instance.SetActualPlayer(player);
                }
            });
            player.SetPlayerListSingle(playerInfo);
            playerInfoList.Add(playerInfo);
        }
    }

    public void ResetSelectedList()
    {

        for (int i = 0; i < playerInfoList.Count; i++)
        {
            playerInfoList[i].Find("selected").gameObject.SetActive(false);
        }
    }

    public void SetIsChoosenAll(bool isChoosenAll)
    {
        this.isChoosenAll = isChoosenAll;

        if (isChoosenAll)
        {
            foreach (Transform playerInfo in playerInfoList)
            {
                playerInfo.Find("selected").gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (Transform playerInfo in playerInfoList)
            {
                playerInfo.Find("selected").gameObject.SetActive(false);
            }
        }
    }

    public bool IsChoosenAll()
    {
        return isChoosenAll;
    }
}
