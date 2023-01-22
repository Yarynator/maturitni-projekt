using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{

    [SerializeField] private Button continueBtn;
    [SerializeField] private Button optionsBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button cancelExitBtn;
    [SerializeField] private Button exitAnywayBtn;
    [SerializeField] private GameObject uiGameObject;
    [SerializeField] private GameObject quitInBattleWarningGameObject;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;
        Hide();
        HideBattleWarning();

        continueBtn.onClick.AddListener(() =>
        {
            isPaused = false;
            Hide();
        });

        exitBtn.onClick.AddListener(() =>
        {
            if (BattleManager.Instance.IsBattle())
            {
                ShowBattleWarning();
            }
            else
            {
                PlayerData[] playerDataArray = new PlayerData[PlayerManager.Instance.GetPlayerList().Count];

                for (int i = 0; i < playerDataArray.Length; i++)
                {
                    Player player = PlayerManager.Instance.GetPlayerList()[i];
                    playerDataArray[i] = new PlayerData(player.GetGridPosition().x, player.GetGridPosition().y, player.GetName(), player.GetPlayerType(), player.GetLevel(), player.GetAttack(), player.GetDefense(), player.GetMaxMoveDistance(), player.GetHealth().GetHealth(), player.GetHealth().GetMaxHealth(), player.GetInventory().GetItemsInInventory());
                }
                
                SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"), SceneInfo.Instance.GetSceneIndex(), -1, SceneInfo.Instance.GetInsideBuilding(), SceneInfo.Instance.TutorialBattleIsActive(), SceneInfo.Instance.PriestRestaurantIsActive(), SceneInfo.Instance.GetQuestData(), SceneInfo.Instance.GetObjectsData());
                SceneManager.LoadScene(0);
            }
        });

        cancelExitBtn.onClick.AddListener(() =>
        {
            HideBattleWarning();
        });

        exitAnywayBtn.onClick.AddListener(() =>
        {
            PlayerData[] playerDataArray = new PlayerData[PlayerManager.Instance.GetPlayerList().Count];

            for (int i = 0; i < playerDataArray.Length; i++)
            {
                Player player = PlayerManager.Instance.GetPlayerList()[i];
                playerDataArray[i] = new PlayerData(-1, -1, player.GetName(), player.GetPlayerType(), player.GetLevel(), player.GetAttack(), player.GetDefense(), player.GetMaxMoveDistance(), player.GetHealth().GetHealth(), player.GetHealth().GetMaxHealth(), player.GetInventory().GetItemsInInventory());
            }

            SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"), SceneInfo.Instance.GetSceneIndex(), 5, true, SceneInfo.Instance.TutorialBattleIsActive(), SceneInfo.Instance.PriestRestaurantIsActive(), SceneInfo.Instance.GetQuestData(), SceneInfo.Instance.GetObjectsData());

            SceneManager.LoadScene(0);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!InventoryUI.Instance.IsOpen())
            {
                isPaused = !isPaused;
                if (isPaused)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }
    }

    private void Show()
    {
        Time.timeScale = 0f;
        uiGameObject.SetActive(true);
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        uiGameObject.SetActive(false);
    }

    public void ShowBattleWarning()
    {
        quitInBattleWarningGameObject.SetActive(true);
    }

    public void HideBattleWarning()
    {
        quitInBattleWarningGameObject.SetActive(false);
    }

}
