using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    
    public static BattleManager Instance { get; private set; }


    [SerializeField] private List<UIInBattle> uiInBattleList;
    [SerializeField] private Button nextRoundButton;

    private bool isBattle;
    private bool isBattleSetuped;
    private bool isPlayerSetuped;
    private bool isPlaying;
    private List<Entity> battleOrderList;
    private List<Entity> diedEntityList;
    private int currentEntityRoundIndex;

    private int stepsRemains;
    private bool attackUsed;
    private bool magicUsed;

    private float timer;
    private float timerMax;

    private GridPosition lastDeadEnemyPosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("BattleManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        timer = 0;
        timerMax = 2f;

        SetIsBattle(false);

        nextRoundButton.onClick.AddListener(() =>
        {
            if (isPlayerSetuped)
            {
                if (battleOrderList[currentEntityRoundIndex] is Player)
                {
                    NextRound();
                }
            }
        });

        Enemy.OnAnyEnemyDies += Enemy_OnAnyEnemyDies;
    }

    private void Update()
    {
        if(isBattleSetuped)
        {
            if (isPlayerSetuped)
            {
                Entity currentEntityRound = battleOrderList[currentEntityRoundIndex];

                if (!isPlaying)
                {
                    if (currentEntityRound is Player player)
                    {
                        stepsRemains = player.GetMaxMoveDistance();
                        attackUsed = false;
                        magicUsed = false;
                        PlayerManager.Instance.SetActualPlayer(player);
                        BattleInfoUI.Instance.SetupSteps();
                        BattleInfoUI.Instance.SetAttackVisibility(true);
                        BattleInfoUI.Instance.SetMagicVisibility(true);
                        BattleInfoUI.Instance.Show();
                        nextRoundButton.gameObject.SetActive(true);

                    }
                    else if (currentEntityRound is Enemy enemy)
                    {
                        stepsRemains = enemy.GetMaxMoveDistance();
                        attackUsed = false;
                        magicUsed = false;
                        Debug.Log(currentEntityRound.ToString() + "'s round");
                        PlayerManager.Instance.SetActualPlayer(null);
                        EnemyAI.Instance.SetTurn(currentEntityRound as Enemy);
                    }
                    isPlaying = true;
                }
            }
            else
            {
                bool isAnyPlayerBusy = false;
                foreach(Player player in PlayerManager.Instance.GetPlayerList())
                {
                    if(ActionManager.Instance.IsBusy(player))
                        isAnyPlayerBusy = true;
                }
                if(!isAnyPlayerBusy)
                {
                    isPlayerSetuped = true;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (isBattle)
        {
            if (!isBattleSetuped)
            {
                Setup();
            }
        }
    }

    private void Setup()
    {
        List<Entity> allEntitySetupList = new List<Entity>();
        foreach(Player player in PlayerManager.Instance.GetPlayerList())
        {
            allEntitySetupList.Add(player);
        }
        foreach(Enemy enemy in EnemyManager.Instance.GetEnemyList())
        {
            allEntitySetupList.Add(enemy);
        }

        battleOrderList = new List<Entity>();
        diedEntityList = new List<Entity>();
        int amount = allEntitySetupList.Count;
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, allEntitySetupList.Count);
            battleOrderList.Add(allEntitySetupList[randomIndex]);
            allEntitySetupList.RemoveAt(randomIndex);
        }
        currentEntityRoundIndex = 0;
        stepsRemains = battleOrderList[currentEntityRoundIndex].GetMaxMoveDistance();
        attackUsed = false;
        magicUsed = false;
        isBattleSetuped = true;
        isPlayerSetuped = false;
        isPlaying = false;
        BattleInfoUI.Instance.Hide();
        nextRoundButton.gameObject.SetActive(false);
    }

    public void NextRound()
    {
        BattleInfoUI.Instance.Hide();
        nextRoundButton.gameObject.SetActive(false);
        currentEntityRoundIndex++;
        isPlaying = false;
        if (currentEntityRoundIndex >= battleOrderList.Count)
            currentEntityRoundIndex = 0;

        if(diedEntityList.Contains(battleOrderList[currentEntityRoundIndex]))
            NextRound();
    }

    public bool IsBattle()
    {
        return isBattle;
    }

    public void SetIsBattle(bool isBattle)
    {
        this.isBattle = isBattle;

        foreach(UIInBattle uiInBattle in uiInBattleList)
        {
            if (uiInBattle.IsVisibleInBattle)
            {
                if (isBattle)
                {
                    uiInBattle.uiGameObject.SetActive(true);
                }
                else
                {
                    uiInBattle.uiGameObject.SetActive(false);
                }
            }
            else
            {
                if (isBattle)
                {
                    uiInBattle.uiGameObject.SetActive(false);
                }
                else
                {
                    uiInBattle.uiGameObject.SetActive(true);
                }
            }
        }
    }

    private void Enemy_OnAnyEnemyDies(object sender, System.EventArgs e)
    {
        if (isBattle)
        {
            if(EnemyManager.Instance.GetEnemyList().Count == 0)
            {
                SetIsBattle(false);

                switch (SceneInfo.Instance.GetBattleIndex())
                {
                    case 0:
                        SceneInfo.Instance.SetTutorialBattleIsActive(false);
                        Instantiate(Resources.Load<Item>("Chest"), LevelGrid.Instance.GetWorldPosition(lastDeadEnemyPosition), Quaternion.identity);
                        SceneInfo.Instance.GetObjectsData().priestChestIsActive = true;
                        SceneInfo.Instance.GetObjectsData().priestChestXPosition = lastDeadEnemyPosition.x;
                        SceneInfo.Instance.GetObjectsData().priestChestYPosition = lastDeadEnemyPosition.y;
                        break;
                    case 1:
                        SceneInfo.Instance.SetPriestRestaurantIsActive(false);
                        SceneInfo.Instance.GetQuestData().priestRestaurantTalkLevel++;
                        SceneInfo.Instance.GetQuestData().priestRestarurantIsDone = true;
                        Instantiate(Resources.Load<Item>("PriestHelmet"), LevelGrid.Instance.GetWorldPosition(lastDeadEnemyPosition), Quaternion.identity);
                        SceneInfo.Instance.GetObjectsData().priestHelmetIsActive = true;
                        SceneInfo.Instance.GetObjectsData().priestHelmetXPosition = lastDeadEnemyPosition.x;
                        SceneInfo.Instance.GetObjectsData().priestHelmetYPosition = lastDeadEnemyPosition.y;
                        break;
                }
            }
        }
    }

    public int GetStepsRemains()
    {
        return stepsRemains;
    }

    public void DecrStemsRemains(int steps)
    {
        stepsRemains -= steps;
        BattleInfoUI.Instance.DisableSteps(steps);
    }

    public bool CanUseAttack()
    {
        return !attackUsed;
    }

    public void UseAttack()
    {
        attackUsed = true;
    }

    public bool CanUseMagic()
    {
        return !magicUsed;
    }

    public void UseMagic()
    {
        magicUsed = true;
    }

    public void RemoveEntityFromBattleOrderList(Entity entity)
    {
        diedEntityList.Add(entity);
    }

    public void SetLastDeadEnemyPosition(GridPosition enemyPosition)
    {
        lastDeadEnemyPosition = enemyPosition;
    }

}
